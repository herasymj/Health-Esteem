using eIDEAS.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Message
    {
        [Key]
        public int ID { get; set; }
        public Guid AuthorID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public MessageEnum MessageType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
