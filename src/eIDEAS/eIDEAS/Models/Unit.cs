using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{

    public class Unit
    {
        [Key]
        public int ID { get; set; }
        public int DivisionID { get; set; }
        public string Name { get; set; }
    }
}
