using FreeCodeCamp.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeCodeCamp.Data.Services;

public class BidsService : IBidsService {
    private readonly ApplicationDbContext _context;

    public BidsService(ApplicationDbContext context) {
        _context = context;
    }

    public IQueryable<Bid> GetAll() {
        return _context.Bids
            .Include(b => b.Listing)
                .ThenInclude(l => l.User);
    }

    public async Task Add(Bid bid) {
        _context.Bids.Add(bid);
        await _context.SaveChangesAsync();
    }
}