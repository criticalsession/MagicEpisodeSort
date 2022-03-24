using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public static class Manager
    {
        public static string SortedDirName // in case I want to add configs in the future
        {
            get
            {
                return "Magic-Sorted";
            }
        }

        public static void MagicSort(string root)
        {
            // todo: get config
            MoveFiles(root, BuildVideoFiles(root, ReadFiles(root)));
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
                files.AddRange(ReadFiles(dir));
            }

            files.AddRange(Directory.GetFiles(directory));

            return files;
        }

        private static void CreateDirectories(string root, List<VideoFile> videoFiles)
        {
            if (!Directory.Exists(Path.Combine(root, Manager.SortedDirName))) 
                Directory.CreateDirectory(Path.Combine(root, Manager.SortedDirName));

            foreach (var videoFile in videoFiles)
            {
                if (!Directory.Exists(GetSeriesDirectory(root, videoFile)))
                    Directory.CreateDirectory(GetSeriesDirectory(root, videoFile));

                if (!Directory.Exists(GetSeriesSeasonDirectory(root, videoFile)))
                    Directory.CreateDirectory(GetSeriesSeasonDirectory(root, videoFile));
            }
        }

        private static string GetSeriesSeasonDirectory(string root, VideoFile videoFile)
        {
            return Path.Combine(GetSeriesDirectory(root, videoFile), videoFile.SeasonDirectoryName);
        }

        private static string GetSeriesDirectory(string root, VideoFile videoFile)
        {
            return Path.Combine(root, Manager.SortedDirName, videoFile.GetSeriesName());
        }

        private static void MoveFiles(string root, List<VideoFile> videoFiles)
        {
            CreateDirectories(root, videoFiles);

            foreach (var videoFile in videoFiles)
            {
                File.Move(videoFile.FullPath, Path.Combine(videoFile.NewDirectory, videoFile.FileName));
            }
        }
    }
}
