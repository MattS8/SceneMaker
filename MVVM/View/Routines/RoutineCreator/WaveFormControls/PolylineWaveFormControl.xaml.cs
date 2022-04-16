using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using Scene_Maker.Core;

namespace Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls
{
    /// <summary>
    /// Interaction logic for PolylineWaveFormControl.xaml
    /// </summary>
    public partial class PolylineWaveFormControl : UserControl, IWaveFormRender
    {
        const float ZoomMax = 0.975f;
        const float ScrollIntensity = 10000;

        int _renderPosition;
        double _yMiddle = 40;
        double _yScale = 40;
        double _xScale = 40;

        float _zoomFromStart = 0;
        public float ZoomFromStart { get { return _zoomFromStart; } }

        float _zoomFromEnd = 0;
        public float ZoomFromEnd { get { return _zoomFromEnd; } }

        public readonly Polyline TopLine = new Polyline();
        public readonly Polyline BottomLine = new Polyline();

        private List<float> _bottomLinePoints;
        private List<float> _topLinePoints;

        public delegate void ZoomEvent(float fromStart, float fromEnd);
        private ZoomEvent _wavZoom;
        public ZoomEvent WavZoom
        {
            get { return _wavZoom; }
            set 
            { 
                _wavZoom = value;
                if (_wavZoom != null)
                    _wavZoom(_zoomFromStart, _zoomFromEnd);
            }
        }

        private ZoomEvent _HandleZoomChanged;
        public ZoomEvent HandleZoomChanged
        {
            get => _HandleZoomChanged;
        }

        public PolylineWaveFormControl()
        {
            _bottomLinePoints = new List<float>();
            _topLinePoints = new List<float>();

            SizeChanged += OnSizeChanged;
            MouseWheel += (s, a) => OnScrollZoom(s, a);

            InitializeComponent();

            TopLine.Stroke = Foreground;
            BottomLine.Stroke = Foreground;
            TopLine.StrokeThickness = 1;
            BottomLine.StrokeThickness = 1;

            mainCanvas.Children.Add(TopLine);
            mainCanvas.Children.Add(BottomLine);

            WavZoom = null;
            _HandleZoomChanged = OnZoomChanged; 
        }

        #region Handlers
        void OnZoomChanged(float fromStart, float fromEnd)
        {
            _zoomFromEnd = fromEnd;
            _zoomFromStart = fromStart;

            InvalidateZoom(false);
        }

        void OnScrollZoom(object sender, MouseWheelEventArgs args)
        {
            //Debug.WriteLine("Mouse Wheel Delta: " + args.Delta);
            float deltaAdjusted = (float)args.Delta / ScrollIntensity;
            Point mousePoint = args.GetPosition(this);
            Tuple<float, float> adjustedZoom = GetMouseAdjustedZoom((float)mousePoint.X, deltaAdjusted);

            if (adjustedZoom.Item1 + adjustedZoom.Item2 > 1)
                return;

            _zoomFromStart = Extras.Clamp(adjustedZoom.Item1, 0, ZoomMax);
            _zoomFromEnd = Extras.Clamp(adjustedZoom.Item2, 0, ZoomMax);

            InvalidateZoom();
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("OnSizeChanged From: " + e.PreviousSize + " to " + e.NewSize);
            // We will remove everything as we are going to rescale vertically
            _renderPosition = 0;

            _yMiddle = ActualHeight / 2;
            _yScale = ActualHeight / 2;
            Tuple<int, int> offsets = GetZoomOffsets();
            _xScale = (offsets.Item2 - offsets.Item1) / ActualWidth;

            Debug.WriteLine("ActualHeight: " + ActualHeight + ", ActualWidth: " + ActualWidth + " (" + _xScale + " points per pixel width)");

            DrawWaveForm();
        }

        public void AddValue(float maxValue, float minValue)
        {
            Debug.WriteLine("Adding maxValue: " + maxValue + " and minValue: " + minValue);
            _topLinePoints.Add(maxValue);
            _bottomLinePoints.Add(minValue);
        }
        #endregion

        #region Audio Draw Functions
        // Redraws WaveForm with recalculated zoom values
        private void InvalidateZoom(bool sendUpdate = true)
        {
            Tuple<int, int> offsets = GetZoomOffsets();
            _xScale = (offsets.Item2 - offsets.Item1) / ActualWidth;

            Debug.WriteLine("Zoom: " + _zoomFromStart + ", " + _zoomFromEnd);

            DrawWaveForm();

            if (WavZoom != null && sendUpdate)
            {
                WavZoom(_zoomFromStart, _zoomFromEnd);
            }
        }

        private void DrawWaveForm()
        {
            _renderPosition = 0;
            ClearAllPoints();
            Tuple<int, int> offsets = GetZoomOffsets();
            for (int i = 0; i < ActualWidth; ++i)
            {
                int iRounded = (int)Math.Round(i * _xScale);
                //int pos = iRounded;

                int pos = GetMaxPointPos((int) Math.Max(offsets.Item1 + iRounded - _xScale, 0),  offsets.Item1 + iRounded);

                //int pos = offsets.Item1 + (int) Math.Round(i * xScale);
                if (pos > _topLinePoints.Count-1)
                    pos = _topLinePoints.Count-1;
                CreatePoint(_topLinePoints[pos], _bottomLinePoints[pos]);
            }
        }
        private void CreatePoint(float topValue, float bottomValue)
        {
            double topLinePos = SampleToYPosition(topValue);
            double bottomLinePos = SampleToYPosition(bottomValue);
            if (_renderPosition >= TopLine.Points.Count)
            {
                TopLine.Points.Add(new Point(_renderPosition, topLinePos));
                BottomLine.Points.Add(new Point(_renderPosition, bottomLinePos));
            }
            else
            {
                TopLine.Points[_renderPosition] = new Point(_renderPosition, topLinePos);
                BottomLine.Points[_renderPosition] = new Point(_renderPosition, bottomLinePos);
            }
            _renderPosition++;
        }

        #region Helper Functions


        private int GetMaxPointPos(int firstPos, int lastPos)
        {
            if (firstPos == lastPos)
                return firstPos;


            float maxValue = _topLinePoints[firstPos];
            int maxValuePosition = firstPos;

            for (int i = 1; i < lastPos - firstPos; ++i)
            { 
                if (_topLinePoints[firstPos + i] > maxValue)
                {
                    maxValue = _topLinePoints[firstPos + i];
                    maxValuePosition = firstPos + i;
                }
            }
            //Debug.WriteLine("Max Point between " + firstPos + " and " + lastPos + " is: " + maxValuePosition);

            return maxValuePosition;
        }

        private void ClearAllPoints()
        {
            TopLine.Points.Clear();
            BottomLine.Points.Clear();
            _renderPosition = 0;
        }

        private double SampleToYPosition(float value)
        {
            return _yMiddle + value * _yScale;
        }

        #endregion
        #endregion

        #region Zoom Functions
        private Tuple<float, float> GetMouseAdjustedZoom(float mouseX, float delta)
        {
            double frameWidth = ActualWidth == 0 ? mouseX + 0.01 : ActualWidth;
            float mouseXDelta = (float)(mouseX / frameWidth);

            float adjustedZoomEnd = _zoomFromEnd + (delta * (1 - mouseXDelta));
            float adjustedZoomStart = _zoomFromStart + (delta * mouseXDelta);

            return Tuple.Create(adjustedZoomStart, adjustedZoomEnd);
        }

        private Tuple<int, int> GetZoomOffsets()
        {
            int startOffset = (int)Math.Round(_zoomFromStart * _topLinePoints.Count);
            int endOffset = (int)Math.Round(_topLinePoints.Count - (_zoomFromEnd * _topLinePoints.Count));

            return Tuple.Create(startOffset, endOffset);
        }
        #endregion

    }
}
