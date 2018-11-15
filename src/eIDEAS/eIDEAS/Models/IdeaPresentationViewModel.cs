using System.Collections.Generic;

namespace eIDEAS.Models
{
    public class IdeaPresentationViewModel
    {
        public Idea Overview { get; set; }
        public double AverageRating { get; set; }
        public int UserRating { get; set; }
        public bool IsTracked { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string UnitName { get; set; }
        public List<AmendmentPresentationViewModel> Amendments { get; set; }
    }
}
