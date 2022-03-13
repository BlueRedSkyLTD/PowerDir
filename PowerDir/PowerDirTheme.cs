using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    internal class PowerDirTheme
    {
        /// <summary>
        /// Mapping colors
        /// </summary>
        enum ColorTheme
        {
            DIRECTORY = ConsoleColor.Blue,
            FILE = ConsoleColor.Gray,
            EXE = ConsoleColor.Green,
            LINK = ConsoleColor.Cyan,
            HIDDEN_DIR = ConsoleColor.DarkBlue,
            HIDDEN_FILE = ConsoleColor.DarkGray,
            SYSTEM_DIR = ConsoleColor.DarkRed,
            SYSTEM_FILE = ConsoleColor.Red
        }

        public static ConsoleColor getFgColor(GetPowerDirInfo info)
        {
            if (info.Link)
            {
                return (ConsoleColor) ColorTheme.LINK;
            }
            else if (info.Hidden)
            {
                return (ConsoleColor) (info.Directory ? ColorTheme.HIDDEN_DIR : ColorTheme.HIDDEN_FILE);
            }
            else if (info.System)
            {
                return (ConsoleColor) (info.Directory ? ColorTheme.SYSTEM_DIR : ColorTheme.SYSTEM_FILE);
            }
            else if (info.Directory)
            {
                return (ConsoleColor) (ColorTheme.DIRECTORY);
            }
            else if (info.Extension.ToUpper().EndsWith(".EXE"))
            {
                return (ConsoleColor) (ColorTheme.EXE);
            }
            else
            {
                // generic FILE
                return (ConsoleColor) (ColorTheme.FILE);
            }
        }
    }
}
