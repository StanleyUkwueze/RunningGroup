using Microsoft.AspNetCore.Mvc;
using RaceClub.Repository.Interfaces;
using RaceClub.ViewModel;

namespace RaceClub.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepo _dashboard;

        public DashboardController( IDashboardRepo dashboard)
        {
            _dashboard = dashboard;
        }
        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboard.GetAllUserClubs();
            var userRaces = await _dashboard.GetAllUserRaces();

            var dashboardVM = new DashboardViewModel
            {
                UserClubs = userClubs,
                UserRaces = userRaces
            };
            return View(dashboardVM);
        }
    }
}
