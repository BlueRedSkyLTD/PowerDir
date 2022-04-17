using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerDir.Tests.helpers
{
    internal class Writers
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TestContext testContextInstance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private readonly StringBuilder? _sb;
        public void write(string msg)
        {
            testContextInstance.Write(msg);
            _sb?.Append(msg);
        }

        public void writeColor(string msg, PowerDirTheme.ColorThemeItem col)
        {
            testContextInstance.Write($"[COLOR: {col.Fg},{col.Bg}] {msg}");
            _sb?.Append(msg);
        }

        public void writeLine(string msg)
        {
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
