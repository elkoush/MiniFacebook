using System.ComponentModel.DataAnnotations;

namespace MiniFacebook.Models.Otp
{
    public class VerificationModel
    {
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string code { get; set; }
    }
}
