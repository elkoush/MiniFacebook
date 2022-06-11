using System.ComponentModel.DataAnnotations;

namespace MiniFacebook.Models.users
{
    public class RegisterModel
    { 
          [Required]
         public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Mobile { get; set; } 
    }
}
