using Media.Controller.Ex01;
using Media.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Test.Ex01
{
    public class TestMediaController : MediaController
    {
        public TestMediaController() : base()
        {
        }

        public TestMediaController(int indexSelected) : base()
        {
            Selected = List[indexSelected];
        }

        public override string FileFilter()
        {
            throw new NotImplementedException();
        }

        protected override void InitializeData()
        {
            List = new List<DataModel.Media>();
            List.Add(new Song()
            {
                Id = 0,
                Title = "TestTitle",
                Singer = "TestSinger",
                File = null
            });
        }
    }
}
