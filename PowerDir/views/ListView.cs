using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class ListView : AbstractView
    {
        internal ListView(
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme) : base(writeFunc, writeColorFunc, writeLineFunc, theme)
        {
        }

        public override void displayResult(GetPowerDirInfo result)
        {
            _writeColor(result.RelativeName, _theme.GetColor(result));
            _writeLine();
        }

        public override void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            foreach (var r in results)
                displayResult(r);
        }
    }
}
