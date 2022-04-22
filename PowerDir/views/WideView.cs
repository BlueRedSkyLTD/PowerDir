using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{
    internal class WideView : AbstractView
    {
        private readonly int _num_columns;

        private int current_column = 0;

        private readonly StringBuilder _sb;

        internal WideView(in int width, in int num_columns,
            in Action<object> writeObject
            ) : base((width / num_columns) - 1, writeObject)
        {
            _num_columns = num_columns;
            _sb = new StringBuilder();
        }

        public override void displayResult(GetPowerDirInfo result, IPowerDirTheme theme)
        {
            // TODO the names is processing ESCAPE CODES too....
            _sb.Append(names(theme.colorizeRelativeName(result)))
                .Append(" ");

            current_column++;
            if (current_column >= _num_columns)
            {
                _writeObject(_sb.ToString());
                current_column = 0;
            }
        }

        public override void endDisplay()
        {
            if (current_column != 0)
                _writeObject(_sb.ToString());
        }
    }
}
