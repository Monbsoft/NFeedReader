using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Models
{
    public class NFeedReaderContext : DbContext
    {
        public NFeedReaderContext(DbContextOptions<NFeedReaderContext> options)
            : base(options)
        {
      
        }

        public DbSet<Feed> Feeds { get; set; }
    }
}
