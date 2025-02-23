using IndevLabs.Models.db;
using Microsoft.EntityFrameworkCore;

namespace IndevLabs.Models;

public partial class IndevLabsDbContext : DbContext
{
    public IndevLabsDbContext()
    {
    }
    
    public IndevLabsDbContext(DbContextOptions<IndevLabsDbContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<Wine> Wines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string pgConnectionEnv = Environment.GetEnvironmentVariable("PG_CONNECTION") ??
                                     throw new ApplicationException("Environment variable PG_CONNECTION is not set!");
            optionsBuilder.UseNpgsql(pgConnectionEnv!);
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IndevLabsDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}