using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{
    internal class ListView : AbstractView
    {
        internal ListView(
            in Action<string> writeFunc,
            in Action<string> writeLineFunc
            ) : base(writeFunc, writeLineFunc)
        {
        }

        public override void displayResult(GetPowerDirInfo result)
        {
            _writeLine(result.RelativeName);
        }
    }
}
