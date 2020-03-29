using Microsoft.EntityFrameworkCore;
using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Data
{
    public class FeedRepository : IDisposable
    {
        private readonly NFeedReaderContext _context;
        private bool _disposed = false;

        public FeedRepository(NFeedReaderContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Feed feed)
        {            
            feed.CreationDate = DateTime.Today;
            feed.Name = " ";
            await _context.Feeds.AddAsync(feed);
        }

        public async Task DeleteAsync(int feedId)
        {
            var feed = await GetByIdAsync(feedId);
            _context.Feeds.Remove(feed);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ValueTask<Feed> GetByIdAsync(int feedId)
        {
            return _context.Feeds.FindAsync(feedId);
        }
        public Task<List<Feed>> GetFeedsAsync()
        {
            return _context.Feeds.OrderByDescending(f => f.CreationDate).ToListAsync();
        }
        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Update(Feed feed)
        {
            _context.Entry(feed).State = EntityState.Modified;
        }
        protected virtual void Dispose(bool dispoing)
        {
            if (_disposed)
            {
                return;
            }
            if (dispoing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}