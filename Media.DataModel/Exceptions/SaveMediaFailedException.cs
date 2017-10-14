using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel.Exceptions
{
    public class SaveMediaFailedException: Exception
    {
        public SaveMediaFailedException(): base("The record that u wanted to save can't be saved.")
        {

        }
    }
}
