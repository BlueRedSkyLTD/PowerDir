using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDir.themes;

namespace PowerDir.Tests.helpers
{
    internal class Writers
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TestContext testContextInstance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private readonly StringBuilder? _sb;
       
        public void writeObject(object obj)
        {
            string msg = obj.ToString() ?? "";
            testContextInstance.WriteLine(msg);
            _sb?.Append(msg).AppendLine();
        }

        public Writers(TestContext testContext)
        {
            testContextInstance = testContext;
        }

        public Writers(TestContext testContext, StringBuilder sb)
        {
            testContextInstance = testContext;
            this._sb = sb;
        }
    }
}
