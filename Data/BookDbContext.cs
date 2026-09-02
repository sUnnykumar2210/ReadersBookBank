using Microsoft.EntityFrameworkCore;

namespace ReadersBookBank.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookDetail> BookDetails { get; set; }
    }
}