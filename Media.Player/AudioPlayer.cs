using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Player
{
    public delegate void IsFinished();
    public delegate void IsStarted();

    public class AudioPlayer : IPlayer
    {
        private WaveOut musicOutput = null;
        private MemoryStream memStream = null;
        private Mp3FileReader mp3Reader = null;

        public event IsStarted IsStarted;
        public event IsFinished IsFinished;

        public AudioPlayer()
        {
            musicOutput = new WaveOut();
            musicOutput.Volume = 0.5F;
            musicOutput.PlaybackStopped += StoppedPlaying;
        }

        public TimeSpan TotalTime { get { return mp3Reader.TotalTime; } }
        public TimeSpan Position { get { return mp3Reader.CurrentTime; } set { mp3Reader.CurrentTime = value; } }
        public float Volume { get { return musicOutput.Volume; } set { musicOutput.Volume = value; } }
        public bool IsPlaying { get; private set; }

        private void StoppedPlaying(object sender, StoppedEventArgs e)
        {
            IsFinished();
            musicOutput = new WaveOut();
            IsPlaying = false;
        }

        public void Play(byte[] media)
        {
            if (media != null && media.Length > 0)
            {
                if (memStream != null)
                {
                    memStream.Dispose();
                    memStream = null;
                }
                if (mp3Reader != null)
                {
                    mp3Reader.Dispose();
                    mp3Reader = null;
                }

                memStream = new MemoryStream(media);
                mp3Reader = new Mp3FileReader(memStream);

                musicOutput.Init(mp3Reader);
                musicOutput.Play();
                IsPlaying = true;

                if (musicOutput.PlaybackState == PlaybackState.Playing)
                {
                    IsStarted();
                }
            }
        }

        public void Stop()
        {
            musicOutput.Stop();
            musicOutput = new WaveOut();
            IsPlaying = false;
        }

        public void Pause()
        {
            if (musicOutput.PlaybackState == PlaybackState.Playing)
            {
                musicOutput.Pause();
            }
            else
            {
                musicOutput.Play();
            }
        }

        public void Dispose()
        {
            if (musicOutput != null)
            {
                musicOutput.Dispose();
            }

            if (memStream != null)
            {
                memStream.Dispose();
            }

            if (mp3Reader != null)
            {
                mp3Reader.Dispose();
            }
        }
    }
}
