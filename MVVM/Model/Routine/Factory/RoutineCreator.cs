using NAudio.Extras;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.Model.Routine.Factory
{
    class RoutineCreator
    {
        private WaveStream _fileStream;
        private IWavePlayer _playbackDevice;
        private SampleAggregator _aggregator;
        private float[] _buffer;

        #region Event Handlers
        public event EventHandler<FftEventArgs> FftCalculated;
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        public event EventHandler<Exception> LoadAudioFailure;
        #endregion

        public void LoadAudioFile(string fileName, bool readInData = true)
        {
            StopAudio();
            CloseFile();
            EnsureDeviceCreated();

            try
            {
                AudioFileReader inputStream = new AudioFileReader(fileName);
                _fileStream = inputStream;
                _aggregator = new SampleAggregator(inputStream);
                _aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                _aggregator.PerformFFT = true;
                _aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(s, a);
                _aggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(s, a);
                _playbackDevice.Init(_aggregator);

                if (readInData)
                    readAudioData();
            }
            catch (Exception e)
            {
                LoadAudioFailure?.Invoke(this, e);
                CloseFile();
            }
        }

        public void PauseAudio()
        {
            _playbackDevice?.Pause();
        }

        public void PlayAudio()
        {
            if (_playbackDevice != null && _fileStream != null && _playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                _playbackDevice.Play();
            }
        }

        public void StopAudio()
        {
            _playbackDevice?.Stop();
            if (_fileStream != null)
            {
                _fileStream.Position = 0;
            }
        }

        private void EnsureDeviceCreated()
        {
            if (_playbackDevice == null)
            {
                CreatePlaybackDevice();
            }
        }

        private void readAudioData()
        {
            _buffer = new float[_fileStream.Length / 2];
            _aggregator.Read(_buffer, 0, (int)_fileStream.Length / 2);
        }

        private void CreatePlaybackDevice()
        {
            _playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        private void CloseFile()
        {
            _fileStream?.Dispose();
            _fileStream = null;
            _buffer = null;
        }
    }
}
