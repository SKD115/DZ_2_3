using Microsoft.EntityFrameworkCore;
using Dota2StatsApi.Models;

namespace Dota2StatsApi.Data;

public class DotaStatsDbContext : DbContext
{
    public DotaStatsDbContext(DbContextOptions<DotaStatsDbContext> options) : base(options) { }

    public DbSet<Side> Sides { get; set; }
    public DbSet<Hero> Heroes { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Hero)
            .WithMany(h => h.Matches)
            .HasForeignKey(m => m.HeroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Side)
            .WithMany(s => s.Matches)
            .HasForeignKey(m => m.SideId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Side>().HasData(
            new Side { Id = 1, Name = "Radiant", WinRate = 0.5 },
            new Side { Id = 2, Name = "Dire", WinRate = 0.5 }
        );

        modelBuilder.Entity<Hero>().HasData(
            new Hero { Id = 1, Name = "Pudge", Attribute = "Strength", WinRate = 0.52 },
            new Hero { Id = 2, Name = "Phantom Assassin", Attribute = "Agility", WinRate = 0.48 },
            new Hero { Id = 3, Name = "Invoker", Attribute = "Intelligence", WinRate = 0.55 },
            new Hero { Id = 4, Name = "Snapfire", Attribute = "Universal", WinRate = 0.50 }
        );
    }
}