using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheBrain;

namespace Pinky
{
    public class ManagerTests
    {
        string root = @"C:\Downloads";
        List<string> filePaths =new List<string>();

        [SetUp]
        public void Setup()
        {
            Manager.config = new Config(root);
            filePaths.Add(Path.Combine(root, "A.Sample.Series.S07E06.mp4"));
            filePaths.Add(Path.Combine(root, "A.Different.Series.s06e09.WEBRip", "A.Different.Series.s06e09.WEBRip.mkv"));
            filePaths.Add(Path.Combine(root, "A.Different.Series.s06e10.hdtv", "readme.txt"));
            filePaths.Add(Path.Combine(root, "A.Different.Series.s06e10.hdtv", "A.Different.Series.s06e10.hdtv.mkv"));
            filePaths.Add(Path.Combine(root, "A.Third.Show.S12E0910.WEB.x264", "A.Third.Show.S12E0910.episode-name.WEB.x264.mp4"));
            filePaths.Add(Path.Combine(root, "A.Third.Show.S12E0910.WEB.x264", "A.Third.Show.S12E0910.episode-name.WEB.x264.srt"));
            filePaths.Add(Path.Combine(root, "a.third.show.s12e11-12.WEB.x264", "a.third.show.s12e11-12.episode-name.WEB.x264.mp4"));
            filePaths.Add(Path.Combine(root, "A.Sample.Series.S07E06-E07.mp4"));
            filePaths.Add(Path.Combine(root, "A.Different.Series.s07e01.WEBRip", "A.Different.Series.s07e01.WEBRip.mkv"));
        }

        [Test]
        public void buildVideoFiles_fromRootDirectory()
        {
            var toTest = Manager.BuildVideoFiles(root, filePaths);
            Assert.IsTrue(toTest.Count == 7);
            Assert.AreEqual(Path.Combine(toTest[0].NewDirectory, toTest[0].FileName), Path.Combine(root, Manager.config.SortedDirectory, "A Sample Series", "Season 07", "A.Sample.Series.S07E06.mp4"));
            Assert.AreEqual(Path.Combine(toTest[3].NewDirectory, toTest[3].FileName), Path.Combine(root, Manager.config.SortedDirectory, "A Third Show", "Season 12", "A.Third.Show.S12E0910.episode-name.WEB.x264.mp4"));
            Assert.AreEqual(Path.Combine(toTest[6].NewDirectory, toTest[6].FileName), Path.Combine(root, Manager.config.SortedDirectory, "A Different Series", "Season 07", "A.Different.Series.s07e01.WEBRip.mkv"));
        }

        [Test]
        public void getNewSeriesNames_givenConfigAndVideoFiles()
        {
            var videoFiles = Manager.BuildVideoFiles(root, filePaths);
            var config = new Config(root);
            config.CustomSeriesNames.Add(new string[] { "A Sample Series", "A Sample Series" });
            Manager.config = config;

            List<string> newSeries = Manager.GetNewSeriesNames(videoFiles);
            Assert.AreEqual(2, newSeries.Count());
            Assert.IsTrue(newSeries.Any(p => p == "A Different Series"));
            Assert.IsTrue(newSeries.Any(p => p == "A Third Show"));
            Assert.IsFalse(newSeries.Any(p => p == "A Sample Series"));
        }
    }
}