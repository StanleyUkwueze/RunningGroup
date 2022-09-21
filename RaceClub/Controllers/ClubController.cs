using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceClub.Data;
using RaceClub.Repository.Interfaces;
using RaceClub.Models;
using RaceClub.ViewModel;
using RaceClub.Services;

namespace RaceClub.Controllers
{
    public class ClubController : Controller
    {
       
        private readonly IClubRepo _clubRepo;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepo clubRepo, IPhotoService photoService)
        {
            
            _clubRepo = clubRepo;
            _photoService = photoService;
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
           
            return View();
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
            if(clubToEdit == null) 
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
    }
}
