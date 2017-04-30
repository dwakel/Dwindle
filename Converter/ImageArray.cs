using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Converter
{
    [Serializable()]
    public class ImageArray
    {
        [XmlElement("ImageElements")]
        public byte[] ImageArrayElements { get; set; }
    }
}
