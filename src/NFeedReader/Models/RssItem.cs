using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace NFeedReader.Models
{
    public class RssItem
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImageUri { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
    }
}
