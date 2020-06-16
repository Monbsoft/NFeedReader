using Microsoft.EntityFrameworkCore;
using NFeedReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Data
{
    public class FavoriteRepository : IDisposable
    {
        private readonly NFeedReaderContext _context;
        private bool _disposed = false;

        public FavoriteRepository(NFeedReaderContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Favorite favorite)
        {
            favorite.CreationDate = DateTime.Today;
            await _context.Favorites.AddAsync(favorite);
        }

        public async Task DeleteAsync(int favoriteId)
        {
            var favorite = await GetByIdAsync(favoriteId);
            _context.Favorites.Remove(favorite);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ValueTask<Favorite> GetByIdAsync(int favoriteId)
        {
            return _context.Favorites.FindAsync(favoriteId);
        }
        public Task<List<Favorite>> GetFavoritesAsync()
        {
            return _context.Favorites.OrderByDescending(f => f.CreationDate).ToListAsync();
        }
        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Update(Favorite favorite)
        {
            _context.Entry(favorite).State = EntityState.Modified;
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
