using FreeCodeCamp.Models;

namespace FreeCodeCamp.Data.Services;

public interface IListingService {
    IQueryable<Listing> GetAll();
    Task Add(Listing listing);
    Task<Listing> GetById(int id);
    Task SaveChanges();
}