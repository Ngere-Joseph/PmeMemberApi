using Microsoft.AspNetCore.Identity;
using PmeMemberApi.Core.Model;

namespace PmeMemberApi
{
    public class AppUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
    }
}
