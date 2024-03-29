using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ParksApi.Models
{
  public class ParksApiContext : DbContext
  {
    public DbSet<Park> Parks { get; set; }

    public ParksApiContext(DbContextOptions<ParksApiContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Park>()
        .HasData(
          new Park { ParkId = 1, Name = "Zion", State = "Utah", Features = "the Narrows", Rating = 8 },
          new Park { ParkId = 2, Name = "Glacier", State = "Montana", Features = "The Sun Road", Rating = 9 },
          new Park { ParkId = 3, Name = "Yosemite", State = "California", Features = "Half Dome", Rating = 7 },
          new Park { ParkId = 4, Name = "The Grand Tetons", State = "Wyoming", Features = "Jenny Lake", Rating = 7 }
        );
    }
  }
}