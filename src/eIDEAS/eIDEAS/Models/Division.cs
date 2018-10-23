using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Division
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
