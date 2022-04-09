using Microsoft.PowerShell.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Linq;
using System.IO;
using System;

namespace PowerDir.Tests
{
    // https://stackoverflow.com/questions/10260597/invoking-powershell-script-with-arguments-from-c-sharp
    // https://docs.microsoft.com/en-us/powershell/scripting/developer/hosting/creating-an-initialsessionstate?view=powershell-7.2
    // https://docs.microsoft.com/en-us/powershell/scripting/developer/hosting/windows-powershell-host-quickstart?view=powershell-7.2

    [TestClass]
    public class BasicTest
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private TestContext testContextInstance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private readonly Type _type = typeof(GetPowerDir);
        private readonly string _filename = typeof(GetPowerDir).Module.ToString();
        private readonly string _parentDir = Path.GetRelativePath("../", Directory.GetCurrentDirectory());


        private PowerShell createCmdLet()
        {
            return PowerShell
                .Create()
                .AddCommand(new CmdletInfo("Get-PowerDir", _type))
                .AddParameter("Debug")
                .AddParameter("Verbose");
        }
        private void displayOutput(Collection<PSObject> res)
        {
            foreach (var item in res)
                TestContext.WriteLine(item.ToString());
        }
        private void displayOutput<T>(PSDataCollection<T> dataCollection, string prefix)
        {
            foreach(var data in dataCollection)
            {
                if (data == null) continue;
                TestContext.WriteLine(prefix + data.ToString());
            }
        }

        private void onDataAdding(object? sender, DataAddingEventArgs e)
        {
            string prefix = "";
            string? s;

            if (sender != null && (s = sender.ToString()) != null)
            {
                if (s.Contains("Debug"))
                    prefix = "[Debug] ";
                if (s.Contains("Verbose"))
                    prefix = "[Verbose] ";
            }

           TestContext.WriteLine(prefix + e.ItemAdded.ToString());
        }

        private Collection<PSObject> execute(PowerShell ps)
        {
            ps.Streams.Debug.DataAdding += onDataAdding;
            ps.Streams.Verbose.DataAdding += onDataAdding;
            var output = ps.Invoke();
            displayOutput(output);
            Assert.IsNotNull(output, "output is null");
            Assert.IsTrue(output.Count > 0, "output is empty");
            return output;
        }

        private void checkType(PSObject o, string type)
        {
            Assert.IsNotNull(o);
            StringBuilder sb = new StringBuilder();
            foreach(var t in o.TypeNames)
                sb.AppendLine(t);

            Assert.IsTrue(o.TypeNames.Contains(type), sb.ToString());
        }

        [TestMethod]
        public void TestDefaultInvoke()
        {
            var output = execute(createCmdLet());
            checkType(output[0], "PowerDir.GetPowerDirInfo");
            Assert.IsNotNull(output.Where(
                (dynamic o) => o.Name == _filename).First()
            );
        }

        [TestMethod]
        public void TestListInvoke()
        {
            var output = execute(createCmdLet().AddParameter("d", "l"));
            checkType(output[0], "System.String");
            Assert.IsNotNull(
                output.Where((dynamic o) => o == _filename).First()
            );
        }

        [TestMethod]
        public void TestListDetailsInvoke()
        {
            var output = execute(createCmdLet().AddParameter("d", "ld"));
            checkType(output[0], "System.String");
            Assert.IsNotNull(
                output.Where((dynamic o) => o.StartsWith("a------- " + _filename)).First()
            );
        }

        [TestMethod]
        public void TestWideInvoke()
        {
            // This won't run due to using Host.UI.WriteLine
            var output = execute(createCmdLet().AddParameter("d", "w"));
            checkType(output[0], "System.String");
            Assert.IsNotNull(
                output.Where((dynamic o) => o.Contains(_filename).First())
            );
        }

        [DataTestMethod]
        [DataRow("..")]
        [DataRow("../")]
        [DataRow("../*")]
        [DataRow("../.")]
        [DataRow(".././")]
        [DataRow(".././*")]
        [DataRow("./../")]
        public void TestParentInvoke(string path)
        {
            var output = execute(createCmdLet().AddParameter("Path", path));
            checkType(output[0], "PowerDir.GetPowerDirInfo");
            Assert.IsNotNull(output.Where(
                (dynamic o) => o.Name == _parentDir).First()
            );
        }

        [DataTestMethod]
        [DataRow("../*.0")]
        [DataRow("../????.0")]
        [DataRow(".././*.0")]
        [DataRow("./../*.0")]
        public void TestParentInvokeWildCard(string path)
        {
            var output = execute(createCmdLet().AddParameter("Path", path));
            checkType(output[0], "PowerDir.GetPowerDirInfo");
            Assert.IsNotNull(output.Where(
                (dynamic o) => o.Name == _parentDir).First()
            );

        }

        [DataTestMethod]
        [DataRow("/")]
        public void TestRootInvoke(string path)
        {
            var output = execute(createCmdLet().AddParameter("Path", path));
            checkType(output[0], "PowerDir.GetPowerDirInfo");
            string rootDir = "";
            if (System.OperatingSystem.IsWindows())
            {
                string? drive = Path.GetPathRoot(Directory.GetCurrentDirectory());
                // CI fix: run in D:
                if (Path.GetPathRoot(Environment.SystemDirectory) == drive)
                {
                    // if in the same drive
                    rootDir = "Windows"; // eg. only if in C: drive
                } else if(drive != null)
                {
                    string[] dirs = Directory.GetDirectories(drive);
                    // root dir won't contain "Windows".
                    // skipping special dirs
                    rootDir = dirs[0]; // always existing
                }
            } else if(System.OperatingSystem.IsLinux())
            {
                rootDir = "root";
            } else if(System.OperatingSystem.IsMacOS())
            {
                rootDir = "root";
            } else
            {
                throw new PSNotImplementedException(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            }
            TestContext.WriteLine($"rootDir = {rootDir}");

            Assert.IsNotNull(output);
            Assert.IsTrue(output.Count >= 1);

            if (rootDir.Length > 0)
            {
                Assert.IsNotNull(output.Where(
                    (dynamic o) => o.Name == rootDir).First(), "output contains" + output.Select((dynamic o) => o.Name).ToString());
            }
        }

        [DataTestMethod]
        [DataRow("$HOME")] // $HOME looks working for real, but not when used in testing
        [DataRow("$HOME/")]
        [DataRow("~")]
        [DataRow("~/")]
        //[DataRow("~/*")] // ~/*aaa 
        public void TestHomeDirectory(string pathToTest)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            TestContext.WriteLine($"[DEBUG] home = {home}");
            string path = $"{home}/{_filename}";
            File.Copy(_filename, path, true);
            Assert.IsTrue(File.Exists(path));
            try
            {
                var output = execute(createCmdLet().AddParameter("Path", $"{pathToTest}/{_filename}"));
                Assert.IsNotNull(output.Where((dynamic o) => o.Name == _filename).First(), $"{pathToTest}/{_filename} not found");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void TestInHomeDirectory()
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            TestContext.WriteLine($"[DEBUG] home = {home}");
            const string dirName = "___power_dir_test___";
            string dirPath = Path.Combine(home, dirName);
            const string filename = "_power_dir_.test";
            string filePath = Path.Combine(dirPath, filename);
            Directory.CreateDirectory(dirPath);
            File.Create(filePath).Close();
            try
            {
                var output = execute(createCmdLet().AddParameter("Path", $"~/{dirName}"));
                Assert.IsNotNull(output.Where((dynamic o) => o.Name == filename).First());
            }
            finally
            {
                File.Delete(filePath);
                Directory.Delete(dirPath);
            }
        }
    }
}
