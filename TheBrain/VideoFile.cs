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
        private string[] extensions = new string[] { ".mp4", ".mkv", ".webm", ".avi" };
        private string SXXRegex = "[sS][0-9]+";
        private string SXXEYYRegex = "[sS][0-9]+[eE][0-9]+-*[eE]*[0-9]*";
        public string FullPath { get; set; }
        public string NewDirectory { get; internal set; }

        public List<string[]> CustomSeriesNames { get; set; }

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
                string ext = Path.GetExtension(FullPath).ToLower();
                return extensions.Contains(ext) && this.Season != null;
            }
        }

        public string GetOriginalSeriesName()
        {
            return GetSeriesName(false);
        }

        public string GetSeriesName(bool applyCustom = true)
        {
            string seriesName = String.Empty;
            if (this.IsVideoFile)
            {
                seriesName = this.GetSeriesNameFromFileName();
                if (applyCustom && CustomSeriesNames != null && CustomSeriesNames.Count > 0)
                    seriesName = this.ReplaceSeriesNameWithCustom(seriesName);
            }

            return seriesName;
        }

        private string GetSeriesNameFromFileName()
        {
            string seriesName = String.Empty;
            var match = Regex.Match(this.FileName, SXXEYYRegex);
            if (match.Success)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                seriesName = this.FileName.Substring(0, match.Index).Replace(".", " ").Trim();
                seriesName = textInfo.ToTitleCase(seriesName.ToLower());
            }

            return seriesName;
        }

        private string ReplaceSeriesNameWithCustom(string seriesName)
        {
            string[]? nameInConfig = CustomSeriesNames.FirstOrDefault(p => p[0] == seriesName);

            if (nameInConfig != null) 
                return nameInConfig[1];

            return seriesName;
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
            this.NewDirectory = Path.Combine(root, Manager.config.SortedDirectory, this.GetSeriesName(), this.SeasonDirectoryName);
        }

        public VideoFile()
        {
            FullPath = "";
            NewDirectory = "";
            CustomSeriesNames = new List<string[]>();
        }

        public VideoFile(string fullPath, List<string[]>? customSeriesNames = null)
        {
            FullPath = fullPath;
            NewDirectory = "";
            CustomSeriesNames = customSeriesNames ?? new List<string[]>();
        }
    }
}
