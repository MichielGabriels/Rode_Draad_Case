using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel.Exceptions
{
    public class LoadMediaFileException: Exception
    {
        public LoadMediaFileException(): base("Something went wrong while reading a media file.")
        {

        }
    }
}
