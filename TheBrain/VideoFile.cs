using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public class VideoFile
    {
        public string FullPath { get; set; }
        public int Season { get; set; }
        public string SeriesName 
        { 
            get
            {
                return "";
            }
        }

        public bool IsVideoFile {
            get
            {
                string ext = Path.GetExtension(FullPath);
                return ext.ToLower() == ".mp4" || ext.ToLower() == ".mkv";
            }
        }

        public VideoFile()
        {
            FullPath = "";
            Season = 0;
        }

        public VideoFile(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
