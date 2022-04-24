using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PowerDir.themes;

namespace PowerDir.Tests
{
    [TestClass]
    public class EscapeCodesThemesTest
    {
        private const string testDir = "_power_dir_test.dir";
        private const string testFile = "_power_dir_test.file";

        [ClassInitialize()]
        public static void TestFixtureSetup(TestContext context)
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), testDir));
        }

        [ClassCleanup()]
        public static void TestFixtureTearDown()
        {
            Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), testDir), true);
        }

        private GetPowerDirInfo getPowerDirInfo(string input, FileAttributes fa)
        {
            var filePath = Path.Combine
                (Directory.GetCurrentDirectory(),
                input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            );
            FileInfo finfo = new(filePath);
            try
            {
                finfo.Create().Close();
                //finfo.Attributes = FileAttributes.ReadOnly;
                //finfo.Attributes = FileAttributes.Hidden;
                //finfo.Attributes = FileAttributes.Directory;
                //finfo.Attributes = FileAttributes.System;
                finfo.Attributes = fa;

                return new GetPowerDirInfo(finfo, Directory.GetCurrentDirectory());
            }
            finally
            {
                //File.Delete(filePath);
                finfo.Attributes = FileAttributes.Normal;
                finfo.Delete();
            }
        }

        private void checkGetPowerDirInfo(GetPowerDirInfo info)
        {
            Assert.IsNotNull(info);
            Assert.IsTrue(info.RelativeName.StartsWith("\x1B["));
            Assert.IsTrue(info.RelativeName.EndsWith("\x1B[0m"));
        }

        [DataTestMethod]
        [DataRow(FileAttributes.Normal)]
        [DataRow(FileAttributes.Directory)]
        //[DataRow(FileAttributes.EXE)] // to be done EXE
        //[DataRow(FileAttributes.Link)] // to be done Link
        [DataRow(FileAttributes.Hidden)] // File
        [DataRow(FileAttributes.Hidden | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.System)] // File
        [DataRow(FileAttributes.System | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.ReadOnly)] // File
        [DataRow(FileAttributes.ReadOnly | FileAttributes.Directory)] // Dir
        public void TestDisplayResultsColor16(FileAttributes fa)
        {
            GetPowerDirInfo info = getPowerDirInfo($"{testDir}/{testFile}", fa);
            EscapeCodesTheme16 esc = new();
            checkGetPowerDirInfo(esc.colorize(info));
        }

        [DataTestMethod]
        [DataRow(FileAttributes.Normal)]
        [DataRow(FileAttributes.Directory)]
        //[DataRow(FileAttributes.EXE)] // to be done EXE
        //[DataRow(FileAttributes.Link)] // to be done Link
        [DataRow(FileAttributes.Hidden)] // File
        [DataRow(FileAttributes.Hidden | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.System)] // File
        [DataRow(FileAttributes.System | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.ReadOnly)] // File
        [DataRow(FileAttributes.ReadOnly | FileAttributes.Directory)] // Dir
        public void TestDisplayResultsColor256(FileAttributes fa)
        {
            // TODO check the color too?
            GetPowerDirInfo info = getPowerDirInfo($"{testDir}/{testFile}", fa);
            EscapeCodesTheme256 esc = new();
            checkGetPowerDirInfo(esc.colorize(info));
        }

        [DataTestMethod]
        [DataRow(FileAttributes.Normal)]
        [DataRow(FileAttributes.Directory)]
        //[DataRow(FileAttributes.EXE)] // to be done EXE
        //[DataRow(FileAttributes.Link)] // to be done Link
        [DataRow(FileAttributes.Hidden)] // File
        [DataRow(FileAttributes.Hidden | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.System)] // File
        [DataRow(FileAttributes.System | FileAttributes.Directory)] // Dir
        [DataRow(FileAttributes.ReadOnly)] // File
        [DataRow(FileAttributes.ReadOnly | FileAttributes.Directory)] // Dir
        public void TestDisplayResultsColorRgb(FileAttributes fa)
        {
            // TODO check the color too?
            GetPowerDirInfo info = getPowerDirInfo($"{testDir}/{testFile}", fa);
            EscapeCodesThemeRGB esc = new();
            checkGetPowerDirInfo(esc.colorize(info));
        }
    }
}
