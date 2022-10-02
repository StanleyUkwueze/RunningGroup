using Microsoft.AspNetCore.Mvc;
using RaceClub.Helper;
using RaceClub.Repository.Interfaces;
using RaceClub.ViewModel;

namespace RaceClub.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepo _dashboard;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepo dashboard, IHttpContextAccessor httpContextAccessor)
        {
            _dashboard = dashboard;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<IActionResult> EditUserProfile()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();

            var user = await _dashboard.GetUserById(currUserId);
            if (user == null) return View("Error");

            var userEditProfileVM = new EditProfileViewModel
            {
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,

            };
            return View(userEditProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Failed To edit profile");
                return View("EditUserProfile", model);
            }

            var user = await _dashboard.GetUserByIdNoTracking(model.Id);

            if (user == null) return View("Error");

            user.Pace = model.Pace;
            user.Mileage = model.Mileage;
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.City = model.City;
            user.State = model.State;

            return View("Index");

        }
    }
}
