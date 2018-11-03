using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Models
{
    public class StatisticPresentationModel
    {
        public string Name { get; set; }
        public string Value {get; set;}

        public StatisticPresentationModel(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
