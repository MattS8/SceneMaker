using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.Windows;
using Scene_Maker.Core;

namespace Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls
{
    /// <summary>
    /// Interaction logic for SeekController.xaml
    /// </summary>
    public partial class SeekController : UserControl
    {
        public delegate void ZoomEvent(float fromStart, float fromEnd);
        private ZoomEvent _SeekZoom;
        public ZoomEvent SeekZoom { get { return _SeekZoom; } }

        private ZoomEvent _ReportZoomChanged;
        public ZoomEvent ReportZoomChanged 
        { 
            get { return _ReportZoomChanged; }
            set { _ReportZoomChanged = value; }
        }

        private Border _SeekScrollButton;

        private float _zoomFromStart;
        private float _zoomFromEnd;

        private bool _IsDragging;

        public SeekController()
        {
            InitializeComponent();
            _IsDragging = false;
            _SeekZoom = OnSeekZoom;
            _SeekScrollButton = this.FindName("Seeker") as Border;

            SizeChanged += OnSeekControllerSizeChanged;

            _ReportZoomChanged = null;
        }

        private void OnSeekControllerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnSeekZoom(_zoomFromStart, _zoomFromEnd);
        }

        private void OnSeekZoom(float fromStart, float fromEnd)
        {
            _zoomFromStart = fromStart;
            _zoomFromEnd = fromEnd;
            double actualWidth = this.ActualWidth - 20;

            if (actualWidth <= 0)
            {
                Debug.WriteLine("SeekButton not measured yet...");
                return;
            }

            _SeekScrollButton.Width = actualWidth * (1 - (_zoomFromStart + _zoomFromEnd));
            double deltaWidth = (this.ActualWidth - 20) - (actualWidth * (1 - (_zoomFromStart + _zoomFromEnd)));
            double totalZoomMargin = _zoomFromEnd + _zoomFromStart == 0 ? 1 : _zoomFromEnd + _zoomFromStart;
            double leftMargin = (_zoomFromStart / totalZoomMargin) * deltaWidth;
            double rightMargin = (_zoomFromEnd / totalZoomMargin) * deltaWidth;
            _SeekScrollButton.Margin = new Thickness(leftMargin,0,rightMargin,0);

            Debug.WriteLine("New SeekButton leftMargin: " + leftMargin + " rightMargin: " + rightMargin);
            Debug.WriteLine("New SeekButton width set to " + (actualWidth-20) + " with _zoomFromStart: " + _zoomFromStart + " and _zoomFromEnd: " + _zoomFromEnd);
        }

        private void ReCenterSeekView(double centerOffset)
        {
            Debug.WriteLine("Offset is " + centerOffset);
            double actualWidth = this.ActualWidth - 20;
            double seekBarWidth = actualWidth * (1 - (_zoomFromStart + _zoomFromEnd));
            double deltaWidth = actualWidth - seekBarWidth;

            // Calculate new zoom offsets based on bar width and distance to edges
            double leftMargin = Extras.Clamp(deltaWidth * (1 - centerOffset), 0, deltaWidth);
            double rightMargin = Extras.Clamp(deltaWidth * centerOffset, 0, deltaWidth);
            Debug.WriteLine("Delta Width: " + deltaWidth + " New Left Margin: " + leftMargin + " New Right Margin: " + rightMargin);

            // Send update to notify other views of new seek view
            double totalZoomMargin = _zoomFromStart + _zoomFromEnd;
            double zoomFromStart = leftMargin / deltaWidth * totalZoomMargin;
            double zoomFromEnd = rightMargin / deltaWidth * totalZoomMargin;
            if (_ReportZoomChanged != null)
                ReportZoomChanged((float)zoomFromStart, (float)zoomFromEnd);
            Debug.WriteLine("Zoom From Start: " + zoomFromStart + " Zoom From End: " + zoomFromEnd);


            // Set bar width and margins
            _SeekScrollButton.Margin = new Thickness(leftMargin,0,rightMargin,0);
        }

        // -------- Drag Handler Functions -------- //
        private void SeekMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;
            _IsDragging = true;
            Debug.WriteLine("START DRAGGING! " + e.GetPosition(this));
            double offset = ((this.ActualWidth - 20) - e.GetPosition(this).X) / (this.ActualWidth - 20);
            ReCenterSeekView(offset);
        }

        internal void HandleMouseMovement(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && _IsDragging)
            {
                Debug.WriteLine("DRAGGING! " + e.GetPosition(this));
                double offset = ((this.ActualWidth - 20) - e.GetPosition(this).X) / (this.ActualWidth - 20);
                ReCenterSeekView(offset);
            }
        }

        internal void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Released || !_IsDragging)
                return;
            _IsDragging = false;
            Debug.WriteLine("STOPPED DRAGGING! " + e.GetPosition(this));
        }
    }
}
