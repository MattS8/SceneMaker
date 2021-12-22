using NAudio.Extras;
using NAudio.Wave;
using Scene_Maker.Core;
using Scene_Maker.MVVM.View.Routines.RoutineCreator.WaveFormControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Scene_Maker.MVVM.ViewModel
{
    class RoutineCreatorViewModel : ObservableObject
    {
        private IWavePlayer playbackDevice;
        private object _audioVisualizerView;
        private PolylineWaveFormControl audioVisualizerView;
        private WaveStream fileStream;

        public event EventHandler<FftEventArgs> FftCalculated;
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public object AudioVisualizer
        {
            get { return _audioVisualizerView; }
            set
            {
                _audioVisualizerView = value;
                OnPropertyChanged();
            }
        }

        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();

            try
            {
                AudioFileReader inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
                SampleAggregator aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                aggregator.MaximumCalculated += (s, a) => OnMaxCalculated(a.MinSample, a.MaxSample);
                playbackDevice.Init(aggregator);

                AudioVisualizerView = new PolylineWaveFormControl();
                AudioVisualizer = AudioVisualizerView;

                Debug.WriteLine("Loaded " + fileName);

                Play();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }

        public void OnMaxCalculated(float min, float max)
        {
            Debug.WriteLine("Adding value: " + min + " " + max);
            audioVisualizerView.AddValue(max, min);
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

        public void Pause()
        {
            playbackDevice?.Pause();
        }

        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                playbackDevice.Play();
            }
        }

        public void Stop()
        {
            playbackDevice?.Stop();
            if (fileStream != null)
            {
                fileStream.Position = 0;
            }
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }
    }
}
