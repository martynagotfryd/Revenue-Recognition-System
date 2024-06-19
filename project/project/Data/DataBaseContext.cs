using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data;

public class DataBaseContext : DbContext
{
    protected DataBaseContext()
    {
    }
    
    public DataBaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Software> Softwares { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Client>().HasData(new List<Client>
        // {
        //     new Client
        //     {
        //         
        //     }
        // });
    }
}