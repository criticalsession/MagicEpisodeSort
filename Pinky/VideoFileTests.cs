using NUnit.Framework;
using TheBrain;
using System.IO;

namespace Pinky
{
    public class VideoFileTests
    {
        private System.Collections.Generic.List<VideoFileTestingObject> testFiles = new System.Collections.Generic.List<VideoFileTestingObject>();
        private string root = @"C:\Downloads";

        [SetUp]
        public void Setup()
        {
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Sample.Series.S07E06.mp4"), "A Sample Series", 7, Path.Combine(root, Manager.config.SortedDirectory, "A Sample Series", "Season 07", "A.Sample.Series.S07E06.mp4")));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Different.Series.s06e09.WEBRip", "A.Different.Series.s06e09.WEBRip.mkv"), "A Different Series", 6, Path.Combine(root, Manager.config.SortedDirectory, "A Different Series", "Season 06", "A.Different.Series.s06e09.WEBRip.mkv")));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Different.Series.s06e10.hdtv", "A.Different.Series.s06e10.hdtv.mkv"), "A Different Series", 6, Path.Combine(root, Manager.config.SortedDirectory, "A Different Series", "Season 06", "A.Different.Series.s06e10.hdtv.mkv")));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Third.Show.S12E0910.WEB.x264", "A.Third.Show.S12E0910.episode-name.WEB.x264.mp4"), "A Third Show", 12, Path.Combine(root, Manager.config.SortedDirectory, "A Third Show", "Season 12", "A.Third.Show.S12E0910.episode-name.WEB.x264.mp4")));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "a.third.show.s12e11-12.WEB.x264", "a.third.show.s12e11-12.episode-name.WEB.x264.mp4"), "A Third Show", 12, Path.Combine(root, Manager.config.SortedDirectory, "A Third Show", "Season 12", "a.third.show.s12e11-12.episode-name.WEB.x264.mp4")));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Sample.Series.S07E06-E07.mp4"), "A Sample Series", 7, Path.Combine(root, Manager.config.SortedDirectory, "A Sample Series", "Season 07", "A.Sample.Series.S07E06-E07.mp4")));
        }

        [Test]
        public void isVideoFile_fromCurrentPath_returnsTrueIfValidVideoFile()
        {
            VideoFile toTest = new VideoFile(@"C:\Downloads\A.Different.Series.s06e09.WEBRIP\you wouldnt download a car.txt");
            Assert.IsFalse(toTest.IsVideoFile);

            toTest = new VideoFile(@"C:\Downloads\A.Different.Series.s06e09.WEBRIP\subtitles.srt");
            Assert.IsFalse(toTest.IsVideoFile);

            toTest = new VideoFile(@"C:\Downloads\A.Different.Series.s06e09.WEBRIP\A.Different.Series.s06e09.WEBRIP.mp4");
            Assert.IsTrue(toTest.IsVideoFile);

            toTest = new VideoFile(@"C:\Downloads\A.Different.Series.s06e09.WEBRIP\A.Different.Series.s06e09.WEBRIP.mkv");
            Assert.IsTrue(toTest.IsVideoFile);
        }

        [Test]
        public void getSeriesName_fromCurrentPath()
        {
            foreach (var tester in this.testFiles)
            {
                VideoFile toTest = new VideoFile(tester.FullPath);
                Assert.AreEqual(tester.SeriesName, toTest.GetSeriesName());
            }
        }

        [Test]
        public void getCustomSeriesName_givenConfig_fromCurrentPath()
        {
            Config config = new Config();
            config.CustomSeriesNames.Add(new string[] { "My First Series", "My 1st Series" });
            config.CustomSeriesNames.Add(new string[] { "My Specialoneword Title", "My Special One-Word Title" });
            config.CustomSeriesNames.Add(new string[] { "Critical Sessions Apostrophe", "Critical Session's Apostrophe" });

            VideoFile toTest = new VideoFile(Path.Combine(root, "my.first.series.s01e01.mp4"), config.CustomSeriesNames);
            Assert.AreEqual(toTest.GetSeriesName(), "My 1st Series");

            toTest = new VideoFile(Path.Combine(root, "my.second.series.s01e01.mp4"), config.CustomSeriesNames);
            Assert.AreEqual(toTest.GetSeriesName(), "My Second Series");

            toTest = new VideoFile(Path.Combine(root, "series directory", "my.SPECIALONEWORD.title.s05e0506.mp4"), config.CustomSeriesNames);
            Assert.AreEqual(toTest.GetSeriesName(), "My Special One-Word Title");

            toTest = new VideoFile(Path.Combine(root, "critical.sessions.apostrophe.s02e02.avi"), config.CustomSeriesNames);
            Assert.AreEqual(toTest.GetSeriesName(), "Critical Session's Apostrophe");
        }

        [Test]
        public void getSeason_fromCurrentPath()
        {
            foreach (var tester in this.testFiles)
            {
                VideoFile toTest = new VideoFile(tester.FullPath);
                Assert.AreEqual(tester.Season, toTest.Season);
            }
        }

        [Test]
        public void buildNewPath_fromCurrentPath_rootToSeriesToSeason()
        {
            foreach (var tester in this.testFiles)
            {
                VideoFile toTest = new VideoFile(tester.FullPath);
                toTest.BuildNewDirectory(root);

                Assert.AreEqual(tester.NewPath, Path.Combine(toTest.NewDirectory, toTest.FileName));
            }
        }

        internal class VideoFileTestingObject
        {
            public string FullPath { get; set; }
            public string SeriesName { get; set; }
            public int Season { get; set; }
            public string NewPath { get; set; }

            public VideoFileTestingObject(string fullPath, string seriesName, int season, string newPath)
            {
                this.FullPath = fullPath;
                this.SeriesName = seriesName;
                this.Season = season;
                this.NewPath = newPath;
            }
        }
    }
}