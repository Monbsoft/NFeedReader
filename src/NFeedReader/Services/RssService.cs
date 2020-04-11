
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
        private readonly RssReader _rssReader;

        public RssService(FeedRepository feedRepository, RssReader rssReader,  RssParser parser)
        {
            _feedRepository = feedRepository;
            _rssParser = parser;
            _rssReader = rssReader;
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
                .OrderByDescending(i => i.PublicationDate)
                .Distinct()
                .ToList();
        }

        public Task<List<RssItem>> GetRssItemsAsync(Feed feed, int? limit = null)
        {
            try
            {
                var root = _rssReader.Open(feed.Url);
                var channelNode = _rssParser.ParseChannel(root);
                return Task.FromResult(_rssParser.ParseItems(channelNode, feed: feed, limit: limit));
            }
            catch (WebException ex)
            {
                return Task.FromResult(new List<RssItem>());
            }
        }
    }
}