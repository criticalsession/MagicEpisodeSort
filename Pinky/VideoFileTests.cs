using NUnit.Framework;
using TheBrain;
using System.IO;

namespace Pinky
{
    public class VideoFileTests
    {
        private System.Collections.Generic.List<VideoFileTestingObject> testFiles = new System.Collections.Generic.List<VideoFileTestingObject>();

        [SetUp]
        public void Setup()
        {
            string root = @"C:\Downloads";
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Sample.Series.S07E06.mp4"), "A Sample Series", 7));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Different.Series.s06e09.WEBRip", "A.Different.Series.s06e09.WEBRip.mkv"), "A Different Series", 6));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Different.Series.s06e10.hdtv", "A.Different.Series.s06e10.hdtv.mkv"), "A Different Series", 6));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Third.Show.S12E0910.WEB.x264", "A.Third.Show.S12E0910.episode-name.WEB.x264.mp4"), "A Third Show", 12));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "a.third.show.s12e11-12.WEB.x264", "a.third.show.s12e11-12.episode-name.WEB.x264.mp4"), "A Third Show", 12));
            testFiles.Add(new VideoFileTestingObject(Path.Combine(root, "A.Sample.Series.S07E06-E07.mp4"), "A Sample Series", 7));
        }

        [Test]
        public void isVideoFile_fromFullPath_returnsTrueIfValidVideoFile()
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
        public void getSeriesName_fromFullPath()
        {
            foreach (var tester in this.testFiles)
            {
                VideoFile toTest = new VideoFile(tester.FullPath);
                if (toTest.IsVideoFile)
                {
                    Assert.AreEqual(tester.SeriesName, toTest.SeriesName);
                }
            }
        }

        internal class VideoFileTestingObject
        {
            public string FullPath { get; set; }
            public string SeriesName { get; set; }
            public int Season { get; set; }

            public VideoFileTestingObject(string fullPath, string seriesName, int season)
            {
                this.FullPath = fullPath;
                this.SeriesName = seriesName;
                this.Season = season;
            }
        }
    }
}