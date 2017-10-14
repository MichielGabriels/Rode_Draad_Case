using Media.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Player
{
    public class AudioPlaylist : IPlaylist
    {
        private List<DataModel.Media> playlist;

        public bool IsEmpty { get { return Count == 0; } }
        public int Count { get { return playlist.Count(); } }
        public List<DataModel.Media> List { get { return playlist; } }

        public AudioPlaylist()
        {
            playlist = new List<DataModel.Media>();
        }

        public DataModel.Media PlayMedia()
        {
            var song = playlist.First();
            RemoveMedia(song);
            return song;
        }

        public void AddMedia(DataModel.Media newMedia)
        {
            playlist.Add(newMedia);
        }

        public void RemoveMedia(DataModel.Media oldMedia)
        {
            playlist.Remove(oldMedia);
        }

        public void ClearAllMedia()
        {
            playlist = new List<DataModel.Media>();
        }
    }
}
