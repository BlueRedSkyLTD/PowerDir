using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class ListView : AbstractView, IView
    {
        internal ListView(
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme) : base(writeFunc, writeColorFunc, writeLineFunc, theme)
        {
        }

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            foreach (var r in results)
            {
                _writeColor(r.RelativeName, _theme.GetColor(r));
                _writeLine();
                
            }
        }
    }
}
