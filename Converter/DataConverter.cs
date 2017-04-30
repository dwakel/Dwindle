using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;

namespace Converter
{
    public class DataConverter
    {
        public byte[] ImageToByteArray(System.Drawing.Image picture)
        {
            using (var ms = new MemoryStream())
            {
                picture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                System.Drawing.Image actualImage = System.Drawing.Image.FromStream(ms);
                return actualImage;
            }
        }
        public DataConverter()
        {

        }
    }
}
