using eIDEAS.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Models
{
    public class Action
    {
        [Key]
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public int IdeaID { get; set; }
        public int ActionTypeID { get; set; }
        public ActionTypeEnum Type { get; set; }
    }
}
