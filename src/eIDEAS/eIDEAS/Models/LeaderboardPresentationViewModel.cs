using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Models
{
    public class LeaderboardPresentationViewModel
    {
        public string name;
        public int ideaPoints;
        public int participationPoints;

        public LeaderboardPresentationViewModel()
        {
        }

        public LeaderboardPresentationViewModel(ApplicationUser userModel)
        {
            this.name = userModel.FirstName + " " + userModel.LastName;
            this.ideaPoints = userModel.IdeaPoints;
            this.participationPoints = userModel.ParticipationPoints;
        }
    }
}
