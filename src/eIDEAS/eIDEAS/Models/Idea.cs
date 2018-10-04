using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Idea
    {
        [Key]
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public int UnitID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SolutionPlan { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
    }

}
