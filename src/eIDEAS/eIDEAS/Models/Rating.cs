using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }
        public int ActionID { get; set; }
        public float Value { get; set; }
    }
}
