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
        enum KeyColorTheme
        {
            DIRECTORY = 0,
            FILE,
            EXE,
            LINK,
            HIDDEN_DIR,
            HIDDEN_FILE,
            SYSTEM_DIR,
            SYSTEM_FILE,
            READONLY_DIR,
            READONLY_FILE,
            ORIGINAL,
        }

        public struct ColorThemeItem
        {
            public ConsoleColor Fg { get; }
            public ConsoleColor Bg { get; }
            internal ColorThemeItem(ConsoleColor fg, ConsoleColor bg)
            {
                this.Fg = fg;
                this.Bg = bg;
            }
        }

        Dictionary<KeyColorTheme, ColorThemeItem> colorTheme = new Dictionary<KeyColorTheme, ColorThemeItem>()
        {
            {KeyColorTheme.DIRECTORY, new ColorThemeItem(ConsoleColor.Blue, ConsoleColor.Black)},
            {KeyColorTheme.FILE, new ColorThemeItem(ConsoleColor.Gray, ConsoleColor.Black)},
            {KeyColorTheme.EXE, new ColorThemeItem(ConsoleColor.Green, ConsoleColor.Black)},
            {KeyColorTheme.LINK, new ColorThemeItem(ConsoleColor.Cyan, ConsoleColor.Black)},
            {KeyColorTheme.HIDDEN_DIR, new ColorThemeItem(ConsoleColor.White, ConsoleColor.DarkMagenta)},
            {KeyColorTheme.HIDDEN_FILE, new ColorThemeItem(ConsoleColor.Gray, ConsoleColor.DarkMagenta)},
            {KeyColorTheme.SYSTEM_DIR, new ColorThemeItem(ConsoleColor.White, ConsoleColor.DarkYellow)},
            {KeyColorTheme.SYSTEM_FILE, new ColorThemeItem(ConsoleColor.Gray, ConsoleColor.DarkYellow)},
            {KeyColorTheme.READONLY_DIR, new ColorThemeItem(ConsoleColor.White, ConsoleColor.DarkRed)},
            {KeyColorTheme.READONLY_FILE, new ColorThemeItem(ConsoleColor.Gray, ConsoleColor.DarkRed)},
        };

        public PowerDirTheme(ColorThemeItem original_color)
        {
            colorTheme.Add(KeyColorTheme.ORIGINAL, original_color);
        }

        public PowerDirTheme(ConsoleColor original_fg, ConsoleColor original_bg)
        {
            colorTheme.Add(KeyColorTheme.ORIGINAL, new ColorThemeItem(original_fg, original_bg));
        }

        public ColorThemeItem GetOriginalColor()
        {
            return colorTheme[KeyColorTheme.ORIGINAL];
        }
        public ColorThemeItem GetColor(GetPowerDirInfo info)
        {
            // TODO review the colors as those are not mutual exclusive
            // eg a file can be system, hidden and readonly.
            if (info.Link)
            {
                return colorTheme[KeyColorTheme.LINK];
            }
            else if (info.System)
            {
                return info.Directory ?
                    colorTheme[KeyColorTheme.SYSTEM_DIR] :
                    colorTheme[KeyColorTheme.SYSTEM_FILE];
            }
            else if (info.Hidden)
            {
                return info.Directory ?
                    colorTheme[KeyColorTheme.HIDDEN_DIR] :
                    colorTheme[KeyColorTheme.HIDDEN_FILE];
            }
            else if (info.ReadOnly)
            {
                return info.Directory ?
                    colorTheme[KeyColorTheme.READONLY_DIR] :
                    colorTheme[KeyColorTheme.READONLY_FILE];
            }
            else if (info.Directory)
            {
                return colorTheme[KeyColorTheme.DIRECTORY];
            }
            // FILES Only from here
            else if (info.Extension.ToUpper().EndsWith(".EXE"))
            {
                return colorTheme[KeyColorTheme.EXE];
            }
            else
            {
                // generic FILE
                return colorTheme[KeyColorTheme.FILE];
            }
        }
    }
}
