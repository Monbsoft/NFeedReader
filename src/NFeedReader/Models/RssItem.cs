using System;

namespace NFeedReader.Models
{
    public class RssItem
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string FeedName { get; set; }
        public string ImageUri { get; set; }
        public string Link { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            RssItem other = obj as RssItem;
            if(other == null)
            {
                return false;
            }
            else
            {
                return Title.Equals(other.Title);
            }
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}