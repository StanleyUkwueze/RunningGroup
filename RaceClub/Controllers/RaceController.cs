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
    }
}
