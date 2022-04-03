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
            in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme) : base(writeFunc, writeLineFunc, setColorFunc, theme)
        {
        }

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            foreach (var r in results)
            {
                _setColor(_theme.GetColor(r));
                _write(r.Name);
                _setColor(_theme.GetOriginalColor());
                _writeLine();
                
            }
        }
    }
}
