using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Models;
using RaceClub.Repository.Implementation;
using RaceClub.Repository.Interfaces;
using RaceClub.Services;
using RaceClub.ViewModel;

namespace RaceClub.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepo _raceRepo;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepo raceRepo, IPhotoService photoService)
        {
            _raceRepo = raceRepo;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepo.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var race = await _raceRepo.GetByIdAsync(id);
            return View(race);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _raceRepo.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Upload Failed");
            }

            return View(raceVM);
          
        }

        public async Task<IActionResult> Edit(int id)
        {
            var clubToEdit = await _raceRepo.GetByIdAsync(id);
            if (clubToEdit == null)
                return View("Error");

            var clubVm = new EditRaceViewModel
            {
                Title = clubToEdit.Title,
                Description = clubToEdit.Description,
                Address = clubToEdit.Address,
                AddressId = clubToEdit.AddressId,
                Url = clubToEdit.Image,
                RaceCategory = clubToEdit.RaceCategory,
            };

            return View(clubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Error");
            }

            var userClub = await _raceRepo.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");
                    return View(raceVM);
                }
                var photo = await _photoService.AddPhotoAsync(raceVM.Image);



                var club = new Race
                {
                    Id = raceVM.Id,
                    Title = raceVM.Title,
                    Address = raceVM.Address,
                    AddressId = raceVM.AddressId,
                    RaceCategory = raceVM.RaceCategory,
                    Image = photo.Url.ToString(),
                    Description = raceVM.Description
                };

                _raceRepo.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }
        }
    }
}
