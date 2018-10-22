using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int DivisionID { get; set; }

        [Required]
        public int UnitID { get; set; }

        public int IdeaPoints { get; set; }

        public int ParticipationPoints { get; set; }

        public int Permissions { get; set; }

        public string ProfilePic { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
