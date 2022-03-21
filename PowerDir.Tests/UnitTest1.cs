using Microsoft.PowerShell.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowerDir.Tests
{
    // https://stackoverflow.com/questions/10260597/invoking-powershell-script-with-arguments-from-c-sharp
    // https://docs.microsoft.com/en-us/powershell/scripting/developer/hosting/creating-an-initialsessionstate?view=powershell-7.2
    // https://docs.microsoft.com/en-us/powershell/scripting/developer/hosting/windows-powershell-host-quickstart?view=powershell-7.2

    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private PowerShell createCmdLet()
        {
            return PowerShell
                .Create()
                .AddCommand(new CmdletInfo("Get-PowerDir", typeof(GetPowerDir)));
        }
        private void displayOutput(Collection<PSObject> res)
        {
            foreach (var item in res)
            {
                TestContext.WriteLine(item.ToString());
            }

        }
        private void execute(PowerShell ps)
        {
            displayOutput(ps.Invoke());
        }

        private void execute(Pipeline pipeline)
        {
            displayOutput(pipeline.Invoke());
        }

        [TestMethod]
        public void TestDefaultInvoke()
        {
            execute(createCmdLet());
        }

        [TestMethod]
        public void TestListDetailsInvoke()
        {
            execute(createCmdLet().AddParameter("d", "ld"));
        }
        
        [TestMethod]
        public void TestListInvoke()
        {
            execute(createCmdLet().AddParameter("d", "l"));
        }

        [TestMethod]
        public void TestWideInvoke()
        {
            // This won't run due to using Host.UI.WriteLine
            execute(createCmdLet().AddParameter("d", "w"));
        }
    }

}