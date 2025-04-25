using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Place> Places { get; set; }
    
    public DbSet<Review> Reviews { get; set; }

    public DbSet<ReviewLike> ReviewLikes { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Photo> Photos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.PlaceId });

        modelBuilder.Entity<ReviewLike>()
        .HasKey(rl => new { rl.UserId, rl.ReviewId });
    }
}