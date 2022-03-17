using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public static class Manager
    {
        public static List<VideoFile> MagicSort(string root)
        {
            List<string> filePaths = new List<string>();

            return BuildVideoFiles(root, filePaths);
        }

        public static List<VideoFile> BuildVideoFiles(string root, List<string> filePaths)
        {
            List<VideoFile> videoFiles = new List<VideoFile>();
            foreach (string f in filePaths)
            {
                VideoFile videoFile = new VideoFile(f);
                if (videoFile.IsVideoFile)
                {
                    videoFile.BuildNewDirectory(root);
                    videoFiles.Add(videoFile);
                }
            }

            return videoFiles;
        }

        private static List<string> ReadFiles(string directory)
        {
            List<string> files = new List<string>();

            foreach (var dir in Directory.GetDirectories(directory))
            {
                ReadFiles(directory);
            }

            foreach (var file in Directory.GetFiles(directory))
            {
                files.Add(file);
            }

            return files;
        }
    }
}
