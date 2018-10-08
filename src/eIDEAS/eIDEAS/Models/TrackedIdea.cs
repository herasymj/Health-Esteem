using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class TrackedIdea
    {
        [Key]
        public int ID { get; set; }
        public int ActionID { get; set; }
    }
}
