using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Amendment
    {
        [Key]
        public int ID { get; set; }
        public int ActionID { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }
}
