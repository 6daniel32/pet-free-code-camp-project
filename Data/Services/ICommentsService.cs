using FreeCodeCamp.Models;

namespace FreeCodeCamp.Data.Services;

public interface ICommentsService {
    Task Add(Comment comment);
}