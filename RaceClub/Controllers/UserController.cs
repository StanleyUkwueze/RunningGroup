using Microsoft.AspNetCore.Mvc;
using RaceClub.Repository.Interfaces;
using RaceClub.ViewModel;

namespace RaceClub.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users =  await _userRepo.GetAllUsers();
            if(users == null) return NotFound();

            var ListOfUserVM  = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userVm = new UserViewModel
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    State = user.State,
                    City = user.City,
                    ProfileImageUrl = user.ProfileImageUrl,
                };
                ListOfUserVM.Add(userVm);
            }
            return View(ListOfUserVM);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepo.GetUserById(id);

            var userDetailVM = new UserDetailViewModel
            {
                UserName=user.UserName,
                Pace=user.Pace,
                Id = user.Id,
                Mileage=user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,

            };

            return View(userDetailVM);
        }
    }
}
