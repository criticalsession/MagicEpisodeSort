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
        public static Config config = null;
        public static readonly string configName = "MagicEpisodeSort.config";

        public static void AutoMagicSort(string root)
        {
            ReadConfig(root);
            
            List<VideoFile> files = BuildVideoFiles(root);
            UpdateConfig(root, files);

            MoveFiles(root, files);
        }

        public static void ReadConfig(string root)
        {
            config = new Config(root);
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
            if (config == null) ReadConfig(root);

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

        public static List<VideoFile> BuildVideoFiles(string root)
        {
            return BuildVideoFiles(root, ReadFiles(root));
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

        public static List<string> GetNewSeriesNames(List<VideoFile> videoFiles)
        {
            List<string> seriesNames = new List<string>();
            foreach (var videoFile in videoFiles)
            {
                string originalSeriesName = videoFile.GetOriginalSeriesName();
                if (!seriesNames.Contains(originalSeriesName))
                {
                    if (!config.CustomSeriesNames.Any(p => p[0] == originalSeriesName))
                    {
                        seriesNames.Add(originalSeriesName);
                    }
                }
            }

            return seriesNames;
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
            List<string[]> seriesNames = new List<string[]>();
            foreach (VideoFile file in files)
            {
                string originalName = file.GetOriginalSeriesName();
                seriesNames.Add(new string[] { originalName, originalName });
            }

            UpdateConfig(root, seriesNames);
        }

        public static void UpdateConfig(string root, List<string[]> seriesNames)
        {
            foreach (string[] seriesName in seriesNames)
                if (!config.CustomSeriesNames.Any(p => p[0] == seriesName[0]))
                    config.CustomSeriesNames.Add(seriesName);

            UpdateConfig(root);
        }

        public static void UpdateConfig(string root)
        {
            File.WriteAllText(Path.Combine(root, configName), JsonConvert.SerializeObject(config));
        }

        public static void MoveFiles(string root, List<VideoFile> videoFiles)
        {
            CreateDirectories(root, videoFiles);

            foreach (var videoFile in videoFiles)
            {
                File.Move(videoFile.FullPath, Path.Combine(videoFile.NewDirectory, videoFile.FileName));
            }
        }
    }
}
