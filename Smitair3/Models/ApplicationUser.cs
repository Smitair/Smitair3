using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Smitair3.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string AvatarLink { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}
