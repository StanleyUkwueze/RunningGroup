using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Repository.Interfaces;
using RaceClub.Models;
using RaceClub.ViewModel;
using RaceClub.Services;
using RaceClub.Helper;

namespace RaceClub.Controllers
{
    public class ClubController : Controller
    {

        private readonly IClubRepo _clubRepo;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContext;

        public ClubController(IClubRepo clubRepo, IPhotoService photoService, IHttpContextAccessor httpContext)
        {

            _clubRepo = clubRepo;
            _photoService = photoService;
            _httpContext = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepo.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepo.GetByIdAsync(id);
            return View(club);
        }

        public async Task<IActionResult> GetClubByCity(string city)
        {
            var res = await _clubRepo.GetClubByCity(city);
            return View(res);
        }

        public async Task<IActionResult> Create()
        {
            var currentUser = _httpContext.HttpContext?.User.GetUserId();
            var createClubVM = new CreateClubViewModel
            {
                AppUserId = currentUser
            };
            return View(createClubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Image = result.Url.ToString(),
                    Description = clubVM.Description,
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        State = clubVM.Address.State,
                        City = clubVM.Address.City
                    }

                };
                _clubRepo.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload has failed!!!");
            }

            return View(clubVM);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var clubToEdit = await _clubRepo.GetByIdAsync(id);
            if (clubToEdit == null)
                return View("Error");

            var clubVm = new EditClubViewModel
            {
                Title = clubToEdit.Title,
                Description = clubToEdit.Description,
                Address = clubToEdit.Address,
                AddressId = clubToEdit.AddressId,
                Url = clubToEdit.Image,
                ClubCategory = clubToEdit.ClubCategory,
            };

            return View(clubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Error");
            }

            var userClub = await _clubRepo.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");
                    return View(clubVM);
                }
                var photo = await _photoService.AddPhotoAsync(clubVM.Image);



                var club = new Club
                {
                    Id = clubVM.Id,
                    Title = clubVM.Title,                 
                    Address = clubVM.Address,
                    AddressId = clubVM.AddressId,
                    ClubCategory = clubVM.ClubCategory,
                    Image = photo.Url.ToString(),
                    Description = clubVM.Description
                };

                _clubRepo.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubToDelete = await _clubRepo.GetByIdAsync(id);
            if(clubToDelete == null) return View("Error");
            else
            {
                return View(clubToDelete);
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _clubRepo.GetByIdAsync(id);
            if(club == null) return View("Error");

            _clubRepo.Delete(club);

            return RedirectToAction("Index");
        }
    }
}
