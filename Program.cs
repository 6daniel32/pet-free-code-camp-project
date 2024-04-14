using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FreeCodeCamp.Data;
using FreeCodeCamp.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(
    delegate(DbContextOptionsBuilder options) {
        options.UseSqlServer(connectionString);
    }
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(
    delegate(IdentityOptions options) {
        options.SignIn.RequireConfirmedAccount = false;
    }
).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IListingService, ListingService>();

builder.Services.AddScoped<IBidsService, BidsService>();

builder.Services.AddScoped<ICommentsService, CommentsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Listings}/{action=Index}/{id?}");
    
app.MapRazorPages();

app.Run();
