using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
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
            var coreUSer =  _httpContext.HttpContext?.User;
            var userClubs =  _context.Clubs.Where(c => c.AppUser.Id == coreUSer.ToString());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var coreUSer = _httpContext.HttpContext?.User;
            var userRaces = _context.Races.Where(c => c.AppUser.Id == coreUSer.ToString());
            return userRaces.ToList();
        }
    }
}
