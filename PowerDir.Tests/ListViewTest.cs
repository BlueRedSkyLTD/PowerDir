using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PowerDir.views;
using PowerDir.Tests.helpers;
using PowerDir.themes;

namespace PowerDir.Tests
{
#if DEBUG
    [TestClass]
    public class ListViewTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TestContext TestContext
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            get;
            set;
        }

        [DataTestMethod]
        [DataRow("_power_dir_test.dir/_power_dir_test.file")]
        [DataRow("_power_dir_test.dir.very_long_dir_name/_power_dir_test.file")]
        [DataRow("../_power_dir_test.file")]
        [DataRow("../_power_dir_test.file")]
        public void TestDisplayResults(string input)
        {
            // TODO check the color too?
            StringBuilder sb = new StringBuilder();
            Writers w = new(TestContext, sb);
            ListView lv = new ListView(w.writeObject);
            input = input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), input);
            FileInfo finfo = new(filePath);
            GetPowerDirInfo info = new GetPowerDirInfo(finfo, Directory.GetCurrentDirectory());
            lv.displayResult(info, new NoColorTheme());
            Assert.IsTrue(sb.Length > 0);
            Assert.AreEqual(sb.ToString(), string.Concat(input, Environment.NewLine));
        }

        [DataTestMethod]
        [DataRow("_power_dir_test.dir/_power_dir_test.file")]
        [DataRow("_power_dir_test.dir.very_long_dir_name/_power_dir_test.file")]
        [DataRow("../_power_dir_test.file")]
        [DataRow("../_power_dir_test.file")]
        public void TestDisplayResultsAnsiColor(string input)
        {
            // TODO check the color too?
            StringBuilder sb = new StringBuilder();
            Writers w = new(TestContext, sb);
            ListView lv = new ListView(w.writeObject);
            input = input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), input);
            FileInfo finfo = new(filePath);
            GetPowerDirInfo info = new GetPowerDirInfo(finfo, Directory.GetCurrentDirectory());
            lv.displayResult(info, new themes.EscapeCodesTheme256());
            Assert.IsTrue(sb.Length > 0);

            Assert.IsTrue(sb.ToString().Contains(input));
            Assert.IsTrue(sb.ToString().EndsWith("\x1B[0m" + Environment.NewLine)); // RESET ESCape Code
            Assert.IsTrue(sb.ToString().StartsWith("\x1B[")); // a generic ESCape Code
        }
    }
#endif
}
