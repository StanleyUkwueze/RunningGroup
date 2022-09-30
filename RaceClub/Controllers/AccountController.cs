using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaceClub.Data;
using RaceClub.Models;
using RaceClub.ViewModel;

namespace RaceClub.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
                if(passwordCheck)
                {
                    var response = await _signInManager.PasswordSignInAsync(user, model.Password,false,false);
                    if (response.Succeeded)
                    {
                        return RedirectToAction("Index", "Club");
                    }
                }
                TempData["Error"] = "Wrong Credentials, Please try again!";
                return View(model);

            }
            TempData["Error"] = "Wrong Credentials, Please try again!";
            return View(model);

        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email is already tied to a user!";
            }

            var newUser = new AppUser
            {
                Email = model.EmailAddress,
                UserName = model.EmailAddress
            };
            var response = await _userManager.CreateAsync(newUser, model.Password);
            if (response.Succeeded)
            
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Home");


        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
