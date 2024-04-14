using FreeCodeCamp.Models;

namespace FreeCodeCamp.Data.Services;

public interface IBidsService {
    IQueryable<Bid> GetAll();
    Task Add(Bid bid);
}