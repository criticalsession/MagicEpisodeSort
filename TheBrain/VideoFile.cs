using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheBrain
{
    public class VideoFile
    {
        private string SXXRegex = "[sS][0-9]+";
        private string SXXEYYRegex = "[sS][0-9]+[eE][0-9]+-*[eE]*[0-9]*";
        public string FullPath { get; set; }
        public string NewDirectory { get; internal set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(this.FullPath);
            }
        }

        public bool IsVideoFile {
            get
            {
                string ext = Path.GetExtension(FullPath);
                return (ext.ToLower() == ".mp4" || ext.ToLower() == ".mkv") && this.Season != null;
            }
        }

        public string SeriesName
        {
            get
            {
                if (this.IsVideoFile)
                {
                    var match = Regex.Match(this.FileName, SXXEYYRegex);
                    if (match.Success)
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                        string seriesName = this.FileName.Substring(0, match.Index).Replace(".", " ").Trim();
                        return textInfo.ToTitleCase(seriesName);
                    }
                }

                return String.Empty;
            }
        }

        public int? Season {
            get
            {
                // get SXXEYY first, then search for season inside it
                // to avoid weird cases where the title
                // of the series contains SXX

                var match = Regex.Match(this.FileName, SXXEYYRegex);
                if (match.Success)
                {
                    var seasonMatch = Regex.Match(match.Value, SXXRegex);
                    return int.Parse(seasonMatch.Value.ToLower().Replace("s", ""));
                }

                return null;
            }
        }

        public string SeasonDirectoryName
        {
            get
            {
                return "Season " + this.Season.GetValueOrDefault(0).ToString().PadLeft(2, '0');
            }
        }

        public void BuildNewDirectory(string root)
        {
            this.NewDirectory = Path.Combine(root, "MagicSorted", this.SeriesName, this.SeasonDirectoryName);
        }

        public VideoFile()
        {
            FullPath = "";
            NewDirectory = "";
        }

        public VideoFile(string fullPath)
        {
            FullPath = fullPath;
            NewDirectory = "";
        }
    }
}
