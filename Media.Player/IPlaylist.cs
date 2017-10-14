using Media.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Player
{
    public interface IPlaylist
    {
        bool IsEmpty { get; }
        int Count { get; }
        List<DataModel.Media> List { get; }

        DataModel.Media PlayMedia();
        void AddMedia(DataModel.Media newMedia);
        void RemoveMedia(DataModel.Media oldMedia);
        void ClearAllMedia();
    }
}
