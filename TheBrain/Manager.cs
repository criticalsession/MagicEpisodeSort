using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrain
{
    public static class Manager
    {
        public static Config config = new Config();

        public static void MagicSort(string root)
        {
            ReadConfig(root);
            MoveFiles(root, BuildVideoFiles(root, ReadFiles(root)));
        }

        private static void ReadConfig(string root)
        {
            string configPath = Path.Combine(root, "MagicEpisodeSort.config");

            if (!File.Exists(configPath)) File.WriteAllText(configPath, JsonConvert.SerializeObject(config));
            else
            {
                var deserializedConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
                if (deserializedConfig != null) config = deserializedConfig;
            }
        }

        public static List<VideoFile> BuildVideoFiles(string root, List<string> filePaths)
        {
            List<VideoFile> videoFiles = new List<VideoFile>();
            foreach (string f in filePaths)
            {
                VideoFile videoFile = new VideoFile(f, config.CustomSeriesNames);
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
            if (!Directory.Exists(Path.Combine(root, config.SortedDirectory))) 
                Directory.CreateDirectory(Path.Combine(root, config.SortedDirectory));

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
            return Path.Combine(root, config.SortedDirectory, videoFile.GetSeriesName());
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
