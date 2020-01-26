using System.ComponentModel.DataAnnotations;

namespace Udemy.API.DTOs
{
    public class UserForRegisterDTO
    {

        [Required]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}