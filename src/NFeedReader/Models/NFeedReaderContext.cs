using Microsoft.EntityFrameworkCore;

namespace NFeedReader.Models
{
    public class NFeedReaderContext : DbContext
    {
        public NFeedReaderContext(DbContextOptions<NFeedReaderContext> options)
            : base(options)
        {      
        }

        public DbSet<Feed> Feeds { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
    }
}
