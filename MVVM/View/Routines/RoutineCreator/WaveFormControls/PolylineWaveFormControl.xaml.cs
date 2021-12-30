using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;
using System;
using System.Windows.Input;

namespace Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls
{
    /// <summary>
    /// Interaction logic for PolylineWaveFormControl.xaml
    /// </summary>
    public partial class PolylineWaveFormControl : UserControl, IWaveFormRender
    {
        int _renderPosition;
        double yMiddle = 40;
        double yScale = 40;
        double xScale = 40;

        float _zoomFromStart = 0;
        float _zoomFromEnd = 0;
 
        public readonly Polyline topLine = new Polyline();
        public readonly Polyline bottomLine = new Polyline();

        private List<float> bottomLinePoints;
        private List<float> topLinePoints;

        public PolylineWaveFormControl()
        {
            bottomLinePoints = new List<float>();
            topLinePoints = new List<float>();

            SizeChanged += OnSizeChanged;

            InitializeComponent();

            topLine.Stroke = Foreground;
            bottomLine.Stroke = Foreground;
            topLine.StrokeThickness = 1;
            bottomLine.StrokeThickness = 1;

            mainCanvas.Children.Add(topLine);
            mainCanvas.Children.Add(bottomLine);

            mainCanvas.MouseWheel += (s, a) => OnScrollZoom(s, a);
        }

        public void AddValue(float maxValue, float minValue)
        {
            topLinePoints.Add(maxValue);
            bottomLinePoints.Add(minValue);
        }

        public void Zoom(int delta)
        {
            Debug.WriteLine("Mouse Wheel Delta: " + delta);
        }

        void OnScrollZoom(object sender, MouseEventArgs args)
        {
            //Debug.WriteLine("delta: " + args., )
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("OnSizeChanged From: " + e.PreviousSize + " to " + e.NewSize);
            // We will remove everything as we are going to rescale vertically
            _renderPosition = 0;

            yMiddle = ActualHeight / 2;
            yScale = ActualHeight / 2;
            Tuple<int, int> offsets = GetZoomOffsets();
            xScale = (offsets.Item2 - offsets.Item1) / ActualWidth;

            Debug.WriteLine("ActualHeight: " + ActualHeight + ", ActualWidth: " + ActualWidth + " (" + xScale + " points per pixel width)");

            DrawWaveForm();
        }

        private void DrawWaveForm()
        {
            ClearAllPoints();;
            Tuple<int, int> offsets = GetZoomOffsets();
            for (int i = 0; i < ActualWidth; ++i)
            {
                int pos = offsets.Item1 + (int) Math.Round(i * xScale);
                if (pos > topLinePoints.Count-1)
                    pos = topLinePoints.Count-1;
                CreatePoint(topLinePoints[pos], bottomLinePoints[pos]);
            }
        }

        private Tuple<int, int> GetZoomOffsets()
        {
            int startOffset = (int)Math.Round(_zoomFromStart * topLinePoints.Count);
            int endOffset = (int)Math.Round(topLinePoints.Count - (_zoomFromEnd * topLinePoints.Count));

            return Tuple.Create(startOffset, endOffset);
        }

        private void ClearAllPoints()
        {
            topLine.Points.Clear();
            bottomLine.Points.Clear();
        }

        private double SampleToYPosition(float value)
        {
            return yMiddle + value * yScale;
        }

        private void CreatePoint(float topValue, float bottomValue)
        {
            double topLinePos = SampleToYPosition(topValue);
            double bottomLinePos = SampleToYPosition(bottomValue);
            if (_renderPosition >= topLine.Points.Count)
            {
                topLine.Points.Add(new Point(_renderPosition, topLinePos));
                bottomLine.Points.Add(new Point(_renderPosition, bottomLinePos));
            }
            else
            {
                topLine.Points[_renderPosition] = new Point(_renderPosition, topLinePos);
                bottomLine.Points[_renderPosition] = new Point(_renderPosition, bottomLinePos);
            }
            _renderPosition++;
        }

        public void Reset()
        {
            _renderPosition = 0;
            ClearAllPoints();
        }
    }
}
