using System.ComponentModel.DataAnnotations;

namespace MiniFacebook.Models.users
{
    public class LoginModel
    { 
          [Required]
         public string Username { get; set; }
        [Required]
        public string Password { get; set; } 
}
}
