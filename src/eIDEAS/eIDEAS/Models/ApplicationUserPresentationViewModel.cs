using eIDEAS.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class ApplicationUserPresentationViewModel
    {
        public Guid ID { get; set; }

        [Display (Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string Division { get; set; }

        public string Unit { get; set; }

        [Display(Name = "Idea Points")]
        public int IdeaPoints { get; set; }

        [Display(Name = "Participation Points")]
        public int ParticipationPoints { get; set; }

        [Display(Name =  "User's Roles")]
        public List<RoleEnum> UserRoles { get; set; }

        public int Permissions { get; set; }
    }
}
