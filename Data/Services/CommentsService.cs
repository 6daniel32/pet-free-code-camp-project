using FreeCodeCamp.Models;

namespace FreeCodeCamp.Data.Services;

public class CommentsService : ICommentsService {
    private readonly ApplicationDbContext _context;

    public CommentsService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task Add(Comment comment) {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }
}