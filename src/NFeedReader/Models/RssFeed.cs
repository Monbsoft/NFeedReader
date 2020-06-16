using System.Collections.Generic;

namespace NFeedReader.Models
{
    public class RssFeed
    {
        public string Description { get; set; }

        public List<RssItem> Items { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Url { get; set; }
    }
}
