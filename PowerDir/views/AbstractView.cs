using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class AbstractView
    {
        protected delegate void Write(string msg);
        protected delegate void WriteLine(string msg = "");
        protected delegate void SetColor(PowerDirTheme.ColorThemeItem color);
        
        protected PowerDirTheme _theme;

        protected readonly Write _write;
        protected readonly WriteLine _writeLine;
        protected readonly SetColor _setColor;

        internal AbstractView(
            in Action<string> writeFunc,
            in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme
        ) {
            _write = new Write(writeFunc);
            _writeLine = new WriteLine(writeLineFunc);
            _setColor = new SetColor(setColorFunc);
            _theme = theme;
        }
    }
}
