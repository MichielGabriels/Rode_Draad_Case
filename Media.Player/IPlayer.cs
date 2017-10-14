using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Player
{
    public interface IPlayer: IDisposable
    {
        event IsStarted IsStarted;
        event IsFinished IsFinished;

        TimeSpan TotalTime { get; }
        TimeSpan Position { get; set; }
        float Volume { get; set; }
        bool IsPlaying { get; }
        
        void Play(byte[] media);
        void Stop();
        void Pause();
    }
}
