using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RaceClub.ViewModel
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [DisplayName("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
