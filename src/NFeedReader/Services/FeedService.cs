using Microsoft.Extensions.Logging;
using NFeedReader.Data;
using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Services
{
    public class FeedService
    {
        private readonly ILogger<FeedService> _logger;
        private readonly FeedRepository _repository;
        private readonly RssReader _rssReader;
        private readonly RssParser _rssParser;

        public FeedService(FeedRepository repository, RssReader rssReader, RssParser rssParser, ILogger<FeedService> logger)
        {
            _repository = repository;
            _rssReader = rssReader;
            _rssParser = rssParser;
            _logger = logger;
        }

        public async Task AddAsync(Feed feed)
        {
            _logger.LogDebug("Adding feed...");
            var nodeRoot = _rssReader.Open(feed.Url);
            var nodeChannel =  _rssParser.ParseChannel(nodeRoot);
            var rssFeed = _rssParser.ParseFeed(nodeChannel);

            if(rssFeed != null)
            {
                feed.Description = rssFeed.Description;
                feed.ImageUrl = rssFeed.Url;               
            }
            await _repository.AddAsync(feed);
            await _repository.SaveAsync();
        }
    }
}
