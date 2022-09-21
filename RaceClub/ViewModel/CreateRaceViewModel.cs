using RaceClub.Data.Enum;
using RaceClub.Models;

namespace RaceClub.ViewModel
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile Image { get; set; }
        public Address? Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
