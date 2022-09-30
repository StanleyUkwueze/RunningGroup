using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Helper;
using RaceClub.Models;
using RaceClub.Repository.Implementation;
using RaceClub.Repository.Interfaces;
using RaceClub.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClubRepo, ClubRepo>();
builder.Services.AddScoped<IRaceRepo, RaceRepo>();
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();


var app = builder.Build();

//if (args.Length == 1 && args[0].ToLower() == "seeddata")
//{
   await Seed.SeedUsersAndRolesAsync(app);
//    Seed.SeedData(app);
//}

// Configure the HTTP request pipeline.

//await Seed.SeedUsersAndRolesAsync(app);
Seed.SeedData(app);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
