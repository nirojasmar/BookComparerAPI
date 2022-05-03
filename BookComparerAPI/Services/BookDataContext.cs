using BookComparerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookComparerAPI.Services
{
    public class BookDataContext : DbContext
    {
        public BookDataContext(DbContextOptions<BookDataContext> options):
            base(options) { } 
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<Book>? Books { get; set; }
        public DbSet<PriceDate>? PriceDates { get; set; }
    }
}
