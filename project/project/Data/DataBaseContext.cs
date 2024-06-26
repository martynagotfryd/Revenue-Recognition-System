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
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareVersion?> SoftwareVersions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Client>().HasData(new List<Client>
        {
            new Client
            {
               Id = 1,
               Name = "Dom",
               Address = "adr1",
               Mail = "@",
               Phone = 123,
               KRS = 12345,
            },
            new Client
            {
                Id = 2,
                Name = "Ala",
                Address = "adr2",
                Mail = "@@",
                Phone = 234,
                PESEL = 1234567,
                LastName = "Smith"
            }
        });
        
        modelBuilder.Entity<Software>().HasData(new List<Software>
        {
            new Software()
            {
                Id = 1,
                Name = "Windows",
                Description = "abc",
                Category = "1",
                Cost = 200
            }
        });
        
        modelBuilder.Entity<SoftwareVersion>().HasData(new List<SoftwareVersion>
        {
            new SoftwareVersion()
            {
                Id = 1,
                Version = "10a",
                IdSoftware = 1
            }
        });

        modelBuilder.Entity<Discount>().HasData(new List<Discount>
        {
            new Discount()
            {
                Id = 1,
                Name = "this",
                Value = 200,
                Start = DateTime.Now,
                End = DateTime.Now,
                IdSoftware = 1
            }
        });
        
        modelBuilder.Entity<Contract>().HasData(new List<Contract>
        {
            new Contract()
            {
                Id = 1,
                Start = DateTime.Now,
                End = DateTime.Now,
                UpgradesEnd = DateTime.Now.AddYears(1),
                Price = 200,
                Signed = true,
                IdClient = 1,
                IdSoftwareVersion = 1
            }
        });
        
        modelBuilder.Entity<Payment>().HasData(new List<Payment>
        {
            new Payment()
            {
                Id = 1,
                Value = 200,
                IdContract = 1
            }
        });
    }
}