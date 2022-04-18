using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class WideView : AbstractView
    {
        //private readonly int _width;
        private readonly int _num_columns;

        private int current_column = 0;

        internal WideView(in int width, in int num_columns,
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme) : base((width / num_columns) - 1, writeFunc, writeColorFunc, writeLineFunc, theme)
        {
            //_width = width;
            _num_columns = num_columns;
        }

        public override void displayResult(GetPowerDirInfo result)
        {
            _writeColor(names(result), _theme.GetColor(result));
            _write(" ");

            current_column++;
            if (current_column >= _num_columns)
            {
                _writeLine();
                current_column = 0;
            }
        }


        public override void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            int c = 0;
            foreach (var r in results)
            {
                displayResult(r);
            }

            // todo move at the end processing
            if (c!= 0)
                _writeLine();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void endDisplay()
        {
            if (current_column != 0)
                _writeLine();
        }
    }
}
