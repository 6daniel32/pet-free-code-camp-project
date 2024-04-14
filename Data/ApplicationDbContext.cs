using FreeCodeCamp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeCodeCamp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Listing)
            .WithMany(l => l.Bids)
            .HasForeignKey(b => b.ListingId)
            .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete for the Bid-Listing relationship

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Listing)
            .WithMany(l => l.Comments)
            .HasForeignKey(c => c.ListingId)
            .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete for the Comment-Listing relationship
    }
}
