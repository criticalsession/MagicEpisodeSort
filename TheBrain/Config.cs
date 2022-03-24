using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public class Config
    {
        public string SortedDirectory { get; set; }
        public List<string[]> CustomSeriesNames { get; set; }

        public Config()
        {
            SortedDirectory = "Magic-Sorted";
            CustomSeriesNames = new List<string[]>();
        }
    }
}
