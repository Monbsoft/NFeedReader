using NFeedReader.Data;
using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace NFeedReader.Services
{
    public class RssService
    {
        private readonly FeedRepository _feedRepository;

        public RssService(FeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        public async Task<List<RssItem>> GetRssItemsAsync(int? limit = null)
        {
            List<RssItem> items = new List<RssItem>();
            var tasks = new List<Task<List<RssItem>>>();
            foreach(var feed in await _feedRepository.GetFeedsAsync())
            {
                var task = GetRssItemsAsync(feed, limit);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);            
            foreach(var task in tasks)
            {
                items.AddRange(task.Result);
            }
            return items;
        }

        public async Task<List<RssItem>> GetRssItemsAsync(Feed feed, int? limit = null)
        {
            var items = new List<RssItem>();
            var rssFeed = await GetRssFromUriAsync(feed.Url, limit);
            return rssFeed.Items;
        }

        public Task<RssFeed> GetRssFromUriAsync(string uri, int? limit =null)
        {
            var client = new WebClient();
            using (var reader = new XmlTextReader(client.OpenRead(uri)))
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                return Task.FromResult(ParseRssFeed(document, limit));
            }
        }

        private RssFeed ParseRssFeed(XmlNode node, int? limit)
        {
            RssFeed result = new RssFeed();

            var channelNode = node.SelectSingleNode("rss/channel");
            result.Title = channelNode.SelectSingleNode("title").InnerText;
            result.Link = channelNode.SelectSingleNode("link").InnerText;
            result.Description = channelNode.SelectSingleNode("description").InnerText;            

            var itemNodes = channelNode.SelectNodes("item");
            int itemLimit = limit ?? itemNodes.Count;
            for(int i=0; i<itemLimit; i++)
            {
                var item = ParseItem(itemNodes[i]);
                result.Items.Add(item);
            }
            return result;
        }

        private RssItem ParseItem(XmlNode node)
        {
            RssItem item = new RssItem();
            item.Description = ParseValue(node.SelectSingleNode("description"));
            item.Link = ParseValue(node.SelectSingleNode("link"));
            item.Title = ParseValue(node.SelectSingleNode("title"));
            var enclosure = node.SelectSingleNode("enclosure/@url");
            if (enclosure != null)
            {
                item.ImageUri = enclosure.InnerText;
            }
            return item;
        }

        private string ParseValue(XmlNode node)
        {
            if(node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }
    }
}
