using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel.Exceptions
{
    public class RemoveMediaFailedException: Exception
    {
        public RemoveMediaFailedException(): base("The record that u wanted to delete can't be deleted.")
        {

        }
    }
}
