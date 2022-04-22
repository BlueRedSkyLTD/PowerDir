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
            in Action<string, PowerDirThemeClassic.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirThemeClassic theme) : base(writeFunc, writeColorFunc, writeLineFunc, theme)
        {
        }

        public override void displayResult(GetPowerDirInfo result)
        {
            _writeColor(result.RelativeName, _theme.GetColor(result));
            _writeLine();
        }
    }
}
