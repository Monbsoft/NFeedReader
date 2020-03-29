using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace NFeedReader.Models
{
    public class RssFeed
    {
        public RssFeed()
        {          
            Items = new List<RssItem>();            
        }

        public string Description { get; set; }

        public List<RssItem> Items { get; }

        public string Title { get; set; }

        public string Link { get; set; }
    }
}
