using FreeCodeCamp.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeCodeCamp.Data.Services;

public class ListingService : IListingService {
    private readonly ApplicationDbContext _context;

    public ListingService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task Add(Listing listing) {
        _context.Listings.Add(listing);
        await _context.SaveChangesAsync();
    }

    public IQueryable<Listing> GetAll() {
        return _context.Listings.Include(l => l.User);
    }

    public async Task<Listing> GetById(int id) {
        return await _context.Listings
            .Include(listing => listing.User)
            .Include(listing => listing.Bids)
            .Include(listing => listing.Comments)
                .ThenInclude(comment => comment.User)
            .FirstOrDefaultAsync(listing => listing.Id == id);
    }

    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }
}