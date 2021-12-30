using NAudio.Extras;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Scene_Maker.Core;
using Scene_Maker.MVVM.Model.Routine.Factory;
using Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scene_Maker.MVVM.ViewModel
{
    class RoutineCreatorViewModel : ObservableObject
    {
        private object _audioVisualizerView;
        private PolylineWaveFormControl audioVisualizerView;
        private RoutineCreator _routineCreator;

        public RoutineCreatorViewModel()
        {
            _routineCreator = new RoutineCreator();
            AudioVisualizerView = new PolylineWaveFormControl();
            AudioVisualizer = AudioVisualizerView;
        }

        public object AudioVisualizer
        {
            get { return _audioVisualizerView; }
            set
            {
                _audioVisualizerView = value;
                OnPropertyChanged();
            }
        }

        public PolylineWaveFormControl AudioVisualizerView
        {
            get => audioVisualizerView;
            set
            {
                if (value != audioVisualizerView)
                {
                    Debug.WriteLine("Visualizer view set.");
                    audioVisualizerView = value;
                    OnPropertyChanged();
                }
            }
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
            audioVisualizerView.AddValue(max, min);
        }
    }
}
