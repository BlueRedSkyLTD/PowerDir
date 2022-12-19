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
            var curDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine
                (curDir,
                input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            );

            FileSystemInfo? finfo = null;
            try
            {
                if (fa.HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo di;
                    di = new(filePath);
                    di.Create();
                    finfo = di;
                }
                else
                {
                    FileInfo fi;
                    fi = new(filePath);
                    fi.Create().Close();
                    finfo = fi;
                }

                finfo.Attributes = fa;

                return new GetPowerDirInfo(finfo, Directory.GetCurrentDirectory());
            }
            finally
            {
                if (finfo != null)
                {
                    finfo.Attributes = FileAttributes.Normal;
                    finfo.Delete();
                }
            }
        }

        private void checkGetPowerDirInfo(GetPowerDirInfo info, string? contains = null)
        {
            Assert.IsNotNull(info);
            Assert.IsTrue(info.RelativeName.StartsWith("\x1B["));
            Assert.IsTrue(info.RelativeName.EndsWith("\x1B[0m"));
            if(contains != null)
                Assert.IsTrue(info.RelativeName.Contains(contains));
        }

        [DataTestMethod]
        [DataRow(FileAttributes.Normal)]  // this should not return any escape code ?
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
        //[DataRow(FileAttributes.Directory)]
        //[DataRow(FileAttributes.Hidden)] // File
        //[DataRow(FileAttributes.Hidden | FileAttributes.Directory)] // Dir
        //[DataRow(FileAttributes.System)] // File
        //[DataRow(FileAttributes.System | FileAttributes.Directory)] // Dir
        //[DataRow(FileAttributes.ReadOnly)] // File
        //[DataRow(FileAttributes.ReadOnly | FileAttributes.Directory)] // Dir
        public void TestDisplayResultsColor16FileExe(FileAttributes fa)
        {
            GetPowerDirInfo info = getPowerDirInfo($"{testDir}/{testFile}.exe", fa);
            EscapeCodesTheme16 esc = new();
            checkGetPowerDirInfo(esc.colorize(info));
        }

        //[DataTestMethod]
        //[DataRow(FileAttributes.Normal)]
        //[DataRow(FileAttributes.Directory)]
        //[TestMethod]

        // TODO: this won't create the symlink due to windows must be admin policy to create symlink
        // @link https://security.stackexchange.com/questions/10194/why-do-you-have-to-be-an-admin-to-create-a-symlink-in-windows
        public void TestDisplayResultsColor16FileLink()
        {
            //GetPowerDirInfo info = getPowerDirInfo($"{testDir}/{testFile}", fa);
            string input = $"{testDir}/{testFile}";
            FileAttributes fa = FileAttributes.Normal;
            var curDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine
                (curDir,
                input.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            );
            var linkTarget = filePath + ".link";

            FileInfo finfo = new(filePath);
            FileInfo fi = new(linkTarget);
            try
            {
                finfo.Create().Close();
                Assert.IsTrue(finfo.Exists);
                finfo.Attributes = fa;

                fi.CreateAsSymbolicLink(filePath);
                Assert.IsTrue(fi.Exists);

                fi.Attributes = fa;
                Assert.IsNotNull(finfo.LinkTarget);
#pragma warning disable CS8604 // Possible null reference argument.
                fi = new(finfo.LinkTarget);
#pragma warning restore CS8604 // Possible null reference argument.

                var info = new GetPowerDirInfo(fi, Directory.GetCurrentDirectory());
                Assert.IsTrue(info.Link);
                EscapeCodesTheme16 esc = new();
                checkGetPowerDirInfo(esc.colorize(info));
            }
            finally
            {
                finfo.Attributes = FileAttributes.Normal;
                finfo.Delete();
                fi?.Delete();
            }
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
            checkGetPowerDirInfo(esc.colorize(info), "38;5");
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
            checkGetPowerDirInfo(esc.colorize(info), "38;2");
        }
    }
}
