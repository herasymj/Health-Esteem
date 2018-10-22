using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class ApplicationUserPresentationViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string Division { get; set; }

        public string Unit { get; set; }

        [Display(Name = "Idea Points")]
        public int IdeaPoints { get; set; }

        [Display(Name = "Participation Points")]
        public int ParticpationPoints { get; set; }
    }
}
