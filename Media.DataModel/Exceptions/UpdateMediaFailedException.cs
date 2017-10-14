using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel.Exceptions
{
    public class UpdateMediaFailedException: Exception
    {
        public UpdateMediaFailedException(): base("The record that u wanted to update can't be updated.")
        {

        }
    }
}
