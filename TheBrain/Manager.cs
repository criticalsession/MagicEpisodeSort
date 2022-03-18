using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public static class Manager
    {
        public static void MagicSort(string root)
        {
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
            if (!Directory.Exists(Path.Combine(root, "MagicSorted"))) 
                Directory.CreateDirectory(Path.Combine(root, "MagicSorted"));

            foreach (var videoFile in videoFiles)
            {
                if (!Directory.Exists(Path.Combine(root, "MagicSorted", videoFile.SeriesName)))
                    Directory.CreateDirectory(Path.Combine(root, "MagicSorted", videoFile.SeriesName));

                if (!Directory.Exists(Path.Combine(root, "MagicSorted", videoFile.SeriesName, videoFile.SeasonDirectoryName)))
                    Directory.CreateDirectory(Path.Combine(root, "MagicSorted", videoFile.SeriesName, videoFile.SeasonDirectoryName));
            }
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
