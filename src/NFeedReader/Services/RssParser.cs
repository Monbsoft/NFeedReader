using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NFeedReader.Services
{
    public class RssParser
    {
        public RssFeed Parse(XmlNode node, int? limit = null)
        {
            var channelNode = ParseChannel(node);
            var feed = ParseFeed(channelNode);
            feed.Items = ParseItems(channelNode, limit);
            return feed;
        }

        public XmlNode ParseChannel(XmlNode node)
        {
            return node.SelectSingleNode("rss/channel");
        }

        public RssFeed ParseFeed(XmlNode channelNode, int? limit = null)
        {
            RssFeed result = new RssFeed();
            result.Title = ParseText(channelNode.SelectSingleNode("title"));
            result.Link = ParseText(channelNode.SelectSingleNode("link"));
            result.Description = ParseText(channelNode.SelectSingleNode("description"));
            return result;
        }

        public List<RssItem> ParseItems(XmlNode channelNode, int? limit = null)
        {
            List<RssItem> resultList = new List<RssItem>();
            var itemNodes = channelNode.SelectNodes("item");
            int itemLimit = limit ?? itemNodes.Count;
            for (int i = 0; i < itemLimit; i++)
            {
                var item = ParseItem(itemNodes[i]);
                resultList.Add(item);
            }
            return resultList.OrderByDescending(i => i.PublicationDate).ToList();
        }

        public RssItem ParseItem(XmlNode node)
        {
            RssItem item = new RssItem();
            item.Description = ParseText(node.SelectSingleNode("description"));
            item.Link = ParseText(node.SelectSingleNode("link"));
            item.Title = ParseText(node.SelectSingleNode("title"));
            var enclosure = node.SelectSingleNode("enclosure/@url");
            if (enclosure != null)
            {
                item.ImageUri = enclosure.InnerText;
            }
            item.PublicationDate = DateTime.Today;
            string date = ParseText(node.SelectSingleNode("pubDate"));
            if(string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime pubDate))
            {
                item.PublicationDate = pubDate;
            }
                
            return item;
        }

        private string ParseText(XmlNode node)
        {
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }
    }
}