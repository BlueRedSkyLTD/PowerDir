using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    /// <summary>
    /// Concatenate multiple objects into 1 line
    /// TODO: color a single object in a line looks like is not possible.
    /// </summary>
    internal class PowerDirInfoWide
    {
        int Width { get; }
        private int _current_width = 0;

        delegate void Write(ConsoleColor fg, ConsoleColor bg, String msg);
        private Write _write;
        PowerDirInfoWide(in int width, in Action<ConsoleColor, ConsoleColor, String> writeFunc)
        {
            Width = width;
            _write = new Write(writeFunc);
        }

        void addResult(GetPowerDirInfo item)
        {

        }


    }
}
