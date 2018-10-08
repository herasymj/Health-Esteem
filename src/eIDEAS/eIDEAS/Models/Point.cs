using eIDEAS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class Point
    {
        [Key]
        public int ID { get; set; }
        public int ActionID { get; set; }
        public int Value { get; set; }
        public PointTypeEnum Type { get; set; }
    }
}
