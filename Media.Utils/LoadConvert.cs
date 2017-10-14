using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Utils
{
    public static class LoadConvert
    {
        public static byte[] ImportFile(string path)
        {
            byte[] bytes = null;
            using (var stream = File.OpenRead(path))
            {
                byte[] buffer = new byte[1024];

                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }
    }
}
