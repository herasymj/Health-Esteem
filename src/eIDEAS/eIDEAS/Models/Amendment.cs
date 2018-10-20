using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Models
{
    public class Amendment
    {
        [Key]
        public int ID { get; set; }

        public Guid UserID { get; set; }

        public int IdeaID { get; set; }

        public string Comment { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
