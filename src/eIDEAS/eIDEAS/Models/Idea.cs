using System;
using eIDEAS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Idea
    {
        [Key]
        public int ID { get; set; }

        public Guid UserID { get; set; }

        [Display(Name = "Unit")]
        [Required]
        public int UnitID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Solution Plan")]
        [Required]
        public string SolutionPlan { get; set; }

        public StatusEnum Status { get; set; }

        public bool IsDraft { get; set; }

        public string ClosingRemarks { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateEdited { get; set; }

    }

}
