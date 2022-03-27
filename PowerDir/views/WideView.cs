using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class WideView : AbstractView, IView
    {
        private int _width;
        private int _num_columns;
        private int _col_size;

        internal WideView(in int width, in int num_columns,
            in Action<string> writeFunc, in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme) : base(writeFunc, writeLineFunc, setColorFunc, theme)
        {
            _width = width;
            _num_columns = num_columns;
            _col_size = _width / _num_columns;
        }

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            // TODO: limit MAX_NAME=50 or column_size-1 otherwise it looks weird.
            int c = 0;
            foreach (var r in results)
            {
                int w = c * _col_size;
                int cc = w + _col_size;
                _setColor(_theme.getColor(r));
                // TODO: if c == 3 and r.Name > col_size, should start a new line.
                //Host.UI.Write(r.Name);
                _write(r.Name);
                w += r.Name.Length;
                _setColor(_theme.getOriginalColor());
                if (w < cc)
                    _write(new string(' ', (cc - w)));
                else
                {
                    // TODO if it wrote in over 2 columns?
                    int rest = w - cc;
                    while (rest > _col_size)
                    {
                        //Coordinates coord = Host.UI.RawUI.CursorPosition;
                        //coord.X += col_size;
                        //Host.UI.RawUI.CursorPosition = coord;

                        _write(new string(' ', _col_size));
                        rest -= _col_size;
                        c++;
                    }

                    // w>=cc => w-cc >= 0 (number of chars over column end)
                    _write(new string(' ', _col_size - rest));
                    // skip 1 column;
                    c++;
                }

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
