
using NFeedReader.Data;
using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace NFeedReader.Services
{
    public class RssService
    {
        private readonly FeedRepository _feedRepository;
        private readonly RssParser _rssParser;

        public RssService(FeedRepository feedRepository, RssParser parser)
        {
            _feedRepository = feedRepository;
            _rssParser = parser;
        }

        public async Task<List<RssItem>> GetAllItemsAsync(int? limit = null)
        {
            List<RssItem> items = new List<RssItem>();
            var tasks = new List<Task<List<RssItem>>>();
            foreach (var feed in await _feedRepository.GetFeedsAsync())
            {
                var task = GetRssItemsAsync(feed, limit);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            foreach (var task in tasks)
            {
                items.AddRange(task.Result);
            }
            return items
                .Where(i => i.PublicationDate >= DateTime.Now.AddDays(-1))
                .OrderByDescending(i => i.PublicationDate).ToList();
        }

        public Task<List<RssItem>> GetRssItemsAsync(Feed feed, int? limit = null)
        {
            try
            {
                var channelNode = _rssParser.ParseChannel(Open(feed.Url));
                return Task.FromResult(_rssParser.ParseItems(channelNode, feed: feed, limit: limit));
            }
            catch (WebException ex)
            {
                return Task.FromResult(new List<RssItem>());
            }
        }

        public XmlNode Open(string uri)
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
            using (var reader = new XmlTextReader(client.OpenRead(uri)))
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("media", "urn:newbooks-schema");
                return document;
            }
        }
    }
}