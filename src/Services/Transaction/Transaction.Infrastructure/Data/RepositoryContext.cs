using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Transaction.Infrastructure.Data;

public class RepositoryContext : DbContext
{
    public RepositoryContext() : base()
    { }
    // Define your DbSet properties here

    // Override the OnConfiguring method to configure the database provider and connection string
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // Get the configuration from the dependency injection (DI) container
    //     var configuration = this.GetService<IConfiguration>();

    //     // Use SQL Server as the database provider and get the connection string from appsettings.json
    //     optionsBuilder.UseSqlServer(configuration.GetConnectionString("Connection"));
    // }
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
    public DbSet<Transaction.Core.TransactionAggregate.Transaction> Transactions { get; set; }
}