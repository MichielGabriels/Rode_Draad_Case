using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel
{
    public abstract class Media
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string Title { get; set; }
    }
}
