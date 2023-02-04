using System.ComponentModel.DataAnnotations;
using PmeMemberApi.Core.Model;

namespace PmeMemberApi.SecureAuth
{
    public class UserRegistration
    {

        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? Dob { get; set; }

    }
}
