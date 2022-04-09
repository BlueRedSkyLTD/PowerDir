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


        private System.Type _type = typeof(GetPowerDir);
        private string _filename = typeof(GetPowerDir).Module.ToString();
        private string _parentDir = Path.GetRelativePath("../", Directory.GetCurrentDirectory());


        private PowerShell createCmdLet()
        {
            return PowerShell
                .Create()
                .AddCommand(new CmdletInfo("Get-PowerDir", _type));
        }
        private void displayOutput(Collection<PSObject> res)
        {
            foreach (var item in res)
                TestContext.WriteLine(item.ToString());
        }
        private Collection<PSObject> execute(PowerShell ps)
        {
            var output = ps.Invoke();
            displayOutput(output);
            Assert.IsNotNull(output);
            Assert.IsTrue(output.Count > 0);
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
            var output = execute(createCmdLet().AddParameter("d", "w").AddParameter("Debug"));
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
            TestContext.WriteLine($"rootDir = ${rootDir}");
            if (System.OperatingSystem.IsWindows())
            { 
                rootDir = "Windows";
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
            
            Assert.IsNotNull(output.Where(
                (dynamic o) => o.Name == rootDir).First());
        }

        [DataTestMethod]
        [DataRow("$HOME")] // $HOME looks working for real, but not when used in testing
        [DataRow("$HOME/")]
        [DataRow("~")]
        [DataRow("~/")]
        //[DataRow("~/*")] // ~/*aaa 
        public void TestSpecialDirectories(string pathToTest)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            TestContext.WriteLine($"[DEBUG] home = {home}");
            string path = $"{home}/{_filename}";
            File.Copy(_filename, path, true);
            Assert.IsTrue(File.Exists(path));
            try
            {
                var output = execute(createCmdLet().AddParameter("Path", $"{pathToTest}/{_filename}"));
                Assert.IsNotNull(output.Where((dynamic o) => o.Name == _filename).First());
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
