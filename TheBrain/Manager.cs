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
        public static Config config;
        public static readonly string configName = "MagicEpisodeSort.config";

        public static void MagicSort(string root)
        {
            config = new Config(root);
            ReadConfig(root);
            
            List<VideoFile> files = BuildVideoFiles(root, ReadFiles(root));
            UpdateConfig(root, files);

            MoveFiles(root, files);
        }

        public static void ReadConfig(string root)
        {
            string configPath = Path.Combine(root, configName);

            if (!File.Exists(configPath))
            {
                config = new Config(root);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config));
            }
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

        public static void UpdateConfig(string root, List<VideoFile> files)
        {
            foreach (VideoFile file in files)
            {
                string originalName = file.GetOriginalSeriesName();
                if (!config.CustomSeriesNames.Any(p => p[0] == originalName))
                    config.CustomSeriesNames.Add(new string[] { originalName, originalName });
            }

            UpdateConfig(root);
        }

        public static void UpdateConfig(string root)
        {
            File.WriteAllText(Path.Combine(root, configName), JsonConvert.SerializeObject(config));
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
