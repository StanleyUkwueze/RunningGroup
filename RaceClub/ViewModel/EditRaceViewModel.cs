using RaceClub.Data.Enum;
using RaceClub.Models;

namespace RaceClub.ViewModel
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public int? AddressId { get; set; }
        public IFormFile Image { get; set; }
        public Address? Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
