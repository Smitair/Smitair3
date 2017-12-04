using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmitairDOTNET.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        //[Required]
        public string AvatarLink { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required]
        [NotMapped]
        [EmailAddress]
        [Compare("EmailAdress")]
        public string ConfirmEmailAdress { get; set; }
    }
}