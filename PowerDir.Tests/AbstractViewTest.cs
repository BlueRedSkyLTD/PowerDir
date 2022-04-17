using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDir.views;
using System.IO;

namespace PowerDir.Tests
{
    [TestClass]
    public class AbstractViewTest
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private TestContext testContextInstance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
#pragma warning disable S2292 // Trivial properties should be auto-implemented
        public TestContext TestContext
#pragma warning restore S2292 // Trivial properties should be auto-implemented
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        


        class Writers
        {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public TestContext testContextInstance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

            public void write(string msg)
            {
                testContextInstance.Write(msg);
            }

            public void writeColor(string msg, PowerDirTheme.ColorThemeItem col)
            {
                testContextInstance.Write($"[COLOR: {col.Fg},{col.Bg}] {msg}");
            }

            public void writeLine(string msg)
            {
                testContextInstance.WriteLine(msg);
            }

            public Writers(TestContext testContext)
            {
                this.testContextInstance = testContext;
            }

        }

        class AbstractViewMock : AbstractView
        {
            internal AbstractViewMock(in Action<string> writeFunc, in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc, in Action<string> writeLineFunc, in PowerDirTheme theme) : base(writeFunc, writeColorFunc, writeLineFunc, theme)
            {
            }

            internal AbstractViewMock(int nameMaxLength, in Action<string> writeFunc, in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc, in Action<string> writeLineFunc, in PowerDirTheme theme) : base(nameMaxLength, writeFunc, writeColorFunc, writeLineFunc, theme)
            {
            }

            public AbstractViewMock(Writers writers) : this(writers.write, writers.writeColor, writers.writeLine, new PowerDirTheme())
            {
            }

            public AbstractViewMock(Writers writers, int max_length): this(max_length,writers.write, writers.writeColor, writers.writeLine, new PowerDirTheme())
            { }

            public string testNames(GetPowerDirInfo info)
            {
                return this.names(info);
            }
        }


        [DataTestMethod]
        [DataRow(50, "_power_dir_test.dir/_power_dir_test.file", "_power_dir_test.dir/_power_dir_test.file")]
        [DataRow(50, "_power_dir_test.dir.very_long_dir_name/_power_dir_test.file", "_power_dir_test.dir.very_long_dir_name/_power_d...")]
        [DataRow(50,"../_power_dir_test.file", "../_power_dir_test.file")]
        [DataRow(10,"../_power_dir_test.file", "../_pow...")]
        public void TestNames(int names_max_length, string input, string expResult)
        {
            Writers w = new(TestContext);
            AbstractViewMock avm = new(w, names_max_length);
            input = input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            expResult = expResult.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            Assert.IsTrue(avm.NameMaxLength == names_max_length);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), input);
            FileInfo finfo = new(filePath);
            GetPowerDirInfo info = new GetPowerDirInfo(finfo, Directory.GetCurrentDirectory());
            string result = avm.testNames(info);
            Assert.AreEqual(names_max_length, result.Length);
            Assert.AreEqual(expResult, result.Trim());
        }
    }
}
