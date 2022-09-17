using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceClub.Data;

namespace RaceClub.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Clubs.ToListAsync();
            return View(clubs);
        }
    }
}
