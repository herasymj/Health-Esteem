using System;
using System.ComponentModel.DataAnnotations;


namespace eIDEAS.Models
{
    public class IdeaInteraction
    {
        [Key]
        public int ID { get; set; }

        public Guid UserId { get; set; }

        public int IdeaID {get; set; }

        public bool IsTracked { get; set; }

        public int Rating { get; set; }
    }
}
