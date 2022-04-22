﻿using System;
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

        internal WideView(in int width, in int num_columns,
            in Action<string> writeFunc,
            in Action<string> writeLineFunc
            ) : base((width / num_columns) - 1, writeFunc, writeLineFunc)
        {
            _num_columns = num_columns;
        }

        public override void displayResult(GetPowerDirInfo result)
        {
            _write(result.RelativeName);
            _write(" ");

            current_column++;
            if (current_column >= _num_columns)
            {
                _writeLine();
                current_column = 0;
            }
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
