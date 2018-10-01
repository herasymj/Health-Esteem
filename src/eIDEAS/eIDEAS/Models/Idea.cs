using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Models
{
    public class Idea
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int UnitID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SolutionPlan { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
    }
}
