using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation;
using System.Collections.Generic;


namespace PowerDir.Tests
{
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

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void TestDefaultInvoke()
        {
            var ps = PowerShell.Create();
            ps.AddCommand(new CmdletInfo("Get-PowerDir", typeof(GetPowerDir)));
            var res = ps.Invoke(new List<string>() { "-Debug" });
            foreach (var item in res)
            {
                TestContext.WriteLine(item.ToString());
            }
        }
    }
    
}