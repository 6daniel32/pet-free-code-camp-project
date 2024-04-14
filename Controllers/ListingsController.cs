using System.Runtime.CompilerServices;
using System.Security.Claims;
using FreeCodeCamp.Data;
using FreeCodeCamp.Data.Services;
using FreeCodeCamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FreeCodeCamp.Controllers;

public class ListingsController : Controller {
    
    private readonly IListingService _listingService;
    private readonly IBidsService _bidsService;
    private readonly ICommentsService _commentsService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ListingsController(
        IListingService listingService,
        IWebHostEnvironment webHostEnvironment,
        IBidsService bidsService,
        ICommentsService commentsService
    ) {
        _listingService = listingService;
        _webHostEnvironment = webHostEnvironment;
        _bidsService = bidsService;
        _commentsService = commentsService;
    }

    // GET: Listings
    public async Task<IActionResult> Index(int? pageNumber, string searchString) 
    {
        IQueryable<Listing> listingsQueryable = _listingService.GetAll();
        int pageSize = 3;

        if(!searchString.IsNullOrEmpty()) {
            listingsQueryable = listingsQueryable.Where(l => l.Title.Contains(searchString));
        }

        return View(await PaginatedList<Listing>.CreateAsync(
            listingsQueryable.Where(l => l.IsSold == false).AsNoTracking(), 
            pageNumber ?? 1, 
            pageSize
        ));
    }

    public async Task<IActionResult> MyListings(int? pageNumber) 
    {
        IQueryable<Listing> listingsQueryable = _listingService.GetAll();
        int pageSize = 3;

        return View("Index", await PaginatedList<Listing>.CreateAsync(
            listingsQueryable.Where(
                l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).AsNoTracking(), 
            pageNumber ?? 1, 
            pageSize
        ));
    }

    public async Task<IActionResult> MyBids(int? pageNumber) 
    {
        IQueryable<Bid> bidsQueryable = _bidsService.GetAll();
        int pageSize = 3;

        return View("MyBids", await PaginatedList<Bid>.CreateAsync(
            bidsQueryable.Where(
                b => b.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).AsNoTracking(), 
            pageNumber ?? 1, 
            pageSize
        ));
    }

    // GET: Listings/Create
    public IActionResult Create() {
        return View();
    }

    // POST: Listings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ListingVM listing) {
        if (listing.Image != null) {
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string fileName = listing.Image.FileName;
            string filePath = Path.Combine(uploadDir, fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
                listing.Image.CopyTo(fileStream);
            }

            Listing listObj = new Listing {
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                IdentityUserId = listing.IdentityUserId,
                ImagePath = fileName,
            };

            await _listingService.Add(listObj);

            return RedirectToAction(nameof(Index));
        }

        return View(listing);
    }

    // GET: Listings/Details/{Id}
    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }

        Listing listing = await _listingService.GetById(id.Value);
        if (listing == null) {
            return NotFound();
        }

        return View(listing);
    }

    [HttpPost]
    public async Task<ActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid) {
        if(!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        await _bidsService.Add(bid);
        Listing listing = await _listingService.GetById(bid.ListingId.Value);
        listing.Price = bid.Price;
        await _listingService.SaveChanges();

        return View("Details", listing);
    }

    public async Task<ActionResult> CloseBidding(int id) {
        Listing listing = await _listingService.GetById(id);
        listing.IsSold = true;
        await _listingService.SaveChanges();
        return View("Details", listing);
    }

    [HttpPost]
    public async Task<ActionResult> AddComment(
        [Bind("Id, Content, IdentityUserId, ListingId")] Comment comment
    ) {
        if(!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        await _commentsService.Add(comment);
        var listing = await _listingService.GetById(comment.ListingId.Value);

        return View("Details", listing);
    }
}