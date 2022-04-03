using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class WideView : AbstractView, IView
    {
        //private readonly int _width;
        private readonly int _num_columns;

        internal WideView(in int width, in int num_columns,
            in Action<string> writeFunc, in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme) : base((width / num_columns) - 1, writeFunc, writeLineFunc, setColorFunc, theme)
        {
            //_width = width;
            _num_columns = num_columns;
        }

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            int c = 0;
            foreach (var r in results)
            {
                _setColor(_theme.GetColor(r));
                _write(names(r));
                _setColor(_theme.GetOriginalColor());
                _write(" ");

                c++;
                if (c >= _num_columns)
                {
                    _writeLine();
                    c = 0;
                }
            }
        }
    }
}
