using eIDEAS.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Action
    {
        [Key]
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public int IdeaID { get; set; }
        public ActionTypeEnum Type { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }
}
