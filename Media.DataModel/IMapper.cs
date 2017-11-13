using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel
{
    interface IMapper
    {
        List<Media> GetAllMedia();
        byte[] GetMediaFile(int id);
        Media AddMedia(Media newMedia);

        bool UpdateMedia(Media media);
        bool DeleteMedia(Media media);
    }
}
