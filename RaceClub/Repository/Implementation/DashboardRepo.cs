using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Helper;
using RaceClub.Models;
using RaceClub.Repository.Interfaces;

namespace RaceClub.Repository.Implementation
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DashboardRepo(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUSer =  _httpContext.HttpContext?.User;
            var userClubs =  _context.Clubs.Where(c => c.AppUser.Id == currentUSer.GetUserId());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUSer = _httpContext.HttpContext?.User;
            var userRaces = _context.Races.Where(c => c.AppUser.Id == currentUSer.GetUserId());
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
           return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
