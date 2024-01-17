using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class DataDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookInfo>().HasKey(b => b.Id);
            modelBuilder.Entity<Author>().HasKey(b => b.Id);
            modelBuilder.Entity<Publisher>().HasKey(b => b.Id);
            modelBuilder.Entity<BookInStock>().HasKey(b => b.Id);
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<BookInAbonement>().HasKey(b => b.Id);
        }
        public DbSet<Abonement> Abonement { get; set; }
        public DbSet<BookInAbonement> BookAbonement { get; set; }
        public DbSet<BookInfo> BookInfo { get; set; }
        public DbSet<BookInStock> BookInStock { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Author { get; set; }
    }
}
