using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel.Exceptions
{
    public class MediaReadFailedException : Exception
    {
        public MediaReadFailedException(Exception e) : base($"Something went wrong while reading media items.\n{e.Message}") { }
    }
}
