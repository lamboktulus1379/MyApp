using Microsoft.EntityFrameworkCore;
using Wallet.Core.Models;

namespace Wallet.Infrastructure.Data
{
    public class WalletContext : DbContext
    {
        public WalletContext()
        {

        }
        public WalletContext(DbContextOptions options) : base(options) { }
        public DbSet<UserBalance> UserBalance { get; set; }
        public DbSet<UserBalanceLog> UseBalanceLog { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        }
    }
}