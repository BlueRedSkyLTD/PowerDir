using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDir.views;
using System.IO;
using PowerDir.Tests.helpers;
using PowerDir.themes;

namespace PowerDir.Tests
{
#if DEBUG
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>")]
    public class AbstractViewTest
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

        class AbstractViewMock : AbstractView
        {
            public AbstractViewMock(in Writers w, int names_max_length): base(names_max_length, w.writeObject)
            {
            }

            public string testNames(string relativeNames)
            {
                return this.names(relativeNames);
            }

            public override void displayResult(GetPowerDirInfo result, IPowerDirTheme theme)
            {
                throw new NotImplementedException();
            }
        }

        // TODO this should be moved in GetPowerDirInfoTest
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
            string result = avm.testNames(info.RelativeName);
            Assert.AreEqual(names_max_length, result.Length);
            Assert.AreEqual(expResult, result.Trim());
        }
    }
#endif
}
