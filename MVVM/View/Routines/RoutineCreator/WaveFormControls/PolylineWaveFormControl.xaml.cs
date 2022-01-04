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
        const float ZoomMax = 0.475f;
        const float ScrollIntensity = 10000;

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
            MouseWheel += (s, a) => OnScrollZoom(s, a);

            InitializeComponent();

            topLine.Stroke = Foreground;
            bottomLine.Stroke = Foreground;
            topLine.StrokeThickness = 1;
            bottomLine.StrokeThickness = 1;

            mainCanvas.Children.Add(topLine);
            mainCanvas.Children.Add(bottomLine);

            //mainCanvas.MouseWheel += (s, a) => OnScrollZoom(s, a);
        }

        #region Handlers
        void OnScrollZoom(object sender, MouseWheelEventArgs args)
        {
            //Debug.WriteLine("Mouse Wheel Delta: " + args.Delta);
            float deltaAdjusted = (float)args.Delta / ScrollIntensity;

            _zoomFromStart = Extras.Clamp(_zoomFromStart + deltaAdjusted, 0, ZoomMax);
            _zoomFromEnd = Extras.Clamp(_zoomFromEnd + deltaAdjusted, 0, ZoomMax);

            Tuple<int, int> offsets = GetZoomOffsets();
            xScale = (offsets.Item2 - offsets.Item1) / ActualWidth;

            Debug.WriteLine("Zoom: " + _zoomFromStart + ", " + _zoomFromEnd);

            DrawWaveForm();
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

        public void AddValue(float maxValue, float minValue)
        {
            topLinePoints.Add(maxValue);
            bottomLinePoints.Add(minValue);
        }
        #endregion

        #region Audio Draw Functions
        private void DrawWaveForm()
        {
            _renderPosition = 0;
            ClearAllPoints();
            Tuple<int, int> offsets = GetZoomOffsets();
            for (int i = 0; i < ActualWidth; ++i)
            {
                int iRounded = (int)Math.Round(i * xScale);
                int pos = GetMaxPointPos((int) Math.Max(offsets.Item1 + iRounded - xScale, 0),  offsets.Item1 + iRounded);

                //int pos = offsets.Item1 + (int) Math.Round(i * xScale);
                if (pos > topLinePoints.Count-1)
                    pos = topLinePoints.Count-1;
                CreatePoint(topLinePoints[pos], bottomLinePoints[pos]);
            }
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

        #region Helper Functions
        private int GetMaxPointPos(int firstPos, int lastPos)
        {
            if (firstPos == lastPos)
                return firstPos;


            float maxValue = topLinePoints[firstPos];
            int maxValuePosition = firstPos;

            for (int i = 1; i <= lastPos - firstPos; ++i)
            { 
                if (topLinePoints[firstPos + i] > maxValue)
                {
                    maxValue = topLinePoints[firstPos + i];
                    maxValuePosition = firstPos + i;
                }
            }
            //Debug.WriteLine("Max Point between " + firstPos + " and " + lastPos + " is: " + maxValuePosition);

            return maxValuePosition;
        }

        private void ClearAllPoints()
        {
            topLine.Points.Clear();
            bottomLine.Points.Clear();
            _renderPosition = 0;
        }

        private double SampleToYPosition(float value)
        {
            return yMiddle + value * yScale;
        }

        #endregion
        #endregion

        #region Zoom Functions
        private Tuple<int, int> GetZoomOffsets()
        {
            int startOffset = (int)Math.Round(_zoomFromStart * topLinePoints.Count);
            int endOffset = (int)Math.Round(topLinePoints.Count - (_zoomFromEnd * topLinePoints.Count));

            return Tuple.Create(startOffset, endOffset);
        }
        #endregion

    }
}
