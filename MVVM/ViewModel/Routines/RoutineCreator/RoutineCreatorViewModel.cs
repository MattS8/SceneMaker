using NAudio.Extras;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Scene_Maker.Core;
using Scene_Maker.MVVM.Model.Routine.Factory;
using Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls;
using Scene_Maker.MVVM.ViewModel.Routines.RoutineCreator;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scene_Maker.MVVM.ViewModel
{
    class RoutineCreatorViewModel : ObservableObject, DragHandler, SeekBarHandler
    {
        // Model
        private RoutineCreator _routineCreator;

        // Views
        private PolylineWaveFormControl _audioVisualizerView;
        private WaveFormData _waveFormData;
        private SeekController _wavSeekController;

        public RoutineCreatorViewModel()
        {
            _waveFormData = new WaveFormData();
            _routineCreator = new RoutineCreator();
            AudioVisualizerView = new PolylineWaveFormControl();
            WavSeekController = new SeekController(); 
        }

        public SeekController WavSeekController
        {
            get => _wavSeekController;
            set
            {
                if (value != _wavSeekController)
                {
                    _wavSeekController = value;
                    _wavSeekController.SeekZoom(AudioVisualizerView.ZoomFromStart, AudioVisualizerView.ZoomFromEnd);
                    _wavSeekController.ReportZoomChanged = OnSeekBarMoved;
                    OnPropertyChanged();
                }
            }
        }

        public PolylineWaveFormControl AudioVisualizerView
        {
            get => _audioVisualizerView;
            set
            {
                if (value != _audioVisualizerView)
                {
                    Debug.WriteLine("Visualizer view set.");
                    _audioVisualizerView = value;
                    _audioVisualizerView.WavZoom = WavZoomEventCaptured;
                    
                    OnPropertyChanged();
                }
            }
        }

        private void WavZoomEventCaptured(float fromStart, float fromEnd)
        {
            if (WavSeekController != null)
                WavSeekController.SeekZoom(fromStart, fromEnd);
        }

        public void Load(string fileName)
        {
            //_routineCreator.FftCalculated += (s, a) => OnFftCalculated(a);
            _routineCreator.MaximumCalculated += (s, a) => OnMaxCalculated(a.MinSample, a.MaxSample);
            _routineCreator.LoadAudioFailure += (s, e) => OnLoadFailed(e);
            _routineCreator.LoadAudioFile(fileName);
        }

        private void OnLoadFailed(Exception exception)
        {
            MessageBox.Show(exception.Message, "Problem opening file");
        }

        public void OnFftCalculated(FftEventArgs args)
        {
            Debug.Write("OnFftCalculated: ");
            if (args != null)
                Debug.WriteLine(args.Result.Length);
            else
                Debug.WriteLine("NO RESULT");
        }

        public void OnMaxCalculated(float min, float max)
        {
            //Debug.WriteLine("Adding value: " + min + " " + max);
            _audioVisualizerView.AddValue(max, min);
        }

        // -------- Drag Handler -------- //

        public void HandleMouseMovement(object sender, MouseEventArgs e)
        {
            if (_wavSeekController != null)
                _wavSeekController.HandleMouseMovement(sender, e);
        }

        public void HandleMouseDown(object sender, MouseEventArgs e) {}

        public void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (_wavSeekController != null)
                _wavSeekController.HandleMouseUp(sender, e);
        }
        public void HandleMouseLeave(object sender, MouseEventArgs e)
        {
            if (_wavSeekController != null)
                _wavSeekController.HandleMouseUp(sender, e);
        }

        public void HandleMouseEnter(object sender, MouseEventArgs e) {}

        // -------- Seek Bar Handler -------- //
        public void OnSeekBarMoved(float fromStart, float fromEnd)
        {
            AudioVisualizerView.HandleZoomChanged(fromStart, fromEnd);
        }
    }
}
