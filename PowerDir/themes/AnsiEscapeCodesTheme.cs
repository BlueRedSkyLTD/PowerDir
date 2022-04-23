using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal class AnsiEscapeCodesTheme : IPowerDirTheme
    {
        const char ESC = '\x1B';
        const string RESET = "\x1B[0m";

        private readonly string[] _default_extensions = { ".EXE", ".COM", ".BAT", ".CMD", ".PS1" };
        private readonly HashSet<string> _extensions;


        // TODO refactor / restrucutre later on
        public static string QueryDevice()
        {
            return $"{ESC}[0c";
        }

        public static string ResponseDevice()
        {
            return $"{ESC}[?1;0c";
        }

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

        enum Color
        {
            Black = 0,
            DarkRed = 1,
            DargGreen = 2,
            DarkYellow = 3,
            DarkBlue = 4,
            DarkMagenta = 5,
            DarkCyan = 6,
            Gray = 7,
            DarkGray = 8,
            Red = 9,
            Green = 10,
            Yellow = 11,
            Blue = 12,
            Magenta = 13,
            Cyan = 14,
            White = 15,
        }
        
        struct ColorThemeItem
        {
            public Color Fg { get; }
            public Color Bg { get; }


            public ColorThemeItem(Color fg, Color bg)
            {
                Fg = fg;
                Bg = bg;
            }
        }


        readonly Dictionary<KeyColorTheme, ColorThemeItem> _colorTheme = new()
        {
            { KeyColorTheme.DIRECTORY, new ColorThemeItem(Color.Blue, Color.Black) },
            { KeyColorTheme.FILE, new ColorThemeItem(Color.Gray, Color.Black) },
            { KeyColorTheme.EXE, new ColorThemeItem(Color.Green, Color.Black) },
            { KeyColorTheme.LINK, new ColorThemeItem(Color.Cyan, Color.Black) },
            { KeyColorTheme.HIDDEN_DIR, new ColorThemeItem(Color.White, Color.DarkMagenta) },
            { KeyColorTheme.HIDDEN_FILE, new ColorThemeItem(Color.Gray, Color.DarkMagenta) },
            { KeyColorTheme.SYSTEM_DIR, new ColorThemeItem(Color.White, Color.DarkYellow) },
            { KeyColorTheme.SYSTEM_FILE, new ColorThemeItem(Color.Gray, Color.DarkYellow) },
            { KeyColorTheme.READONLY_DIR, new ColorThemeItem(Color.White, Color.DarkRed) },
            { KeyColorTheme.READONLY_FILE, new ColorThemeItem(Color.Gray, Color.DarkRed) },
        };


        public AnsiEscapeCodesTheme()
        {
            _extensions = new HashSet<string>(_default_extensions.ToList());
        }

        private string setColor(Color fg, Color bg)
        {
            return $"{ESC}[38;5;{(int)fg}m{ESC}[48;5;{(int)bg}m";
        }

        private string setColor(ColorThemeItem col)
        {
            return setColor(col.Fg, col.Bg);
        }

        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            // TOOD should overwrite RelativeName here
            info.setRelativeName(colorizeProperty(info, info.RelativeName));
            return info;
        }

        public string colorizeProperty(GetPowerDirInfo info, string str)
        {
            ColorThemeItem c;

            if (info.Link)
            {
                c = _colorTheme[KeyColorTheme.LINK];
            }
            else if (info.System)
            {
                if (info.Directory)
                    c = _colorTheme[KeyColorTheme.SYSTEM_DIR];
                else
                    c = _colorTheme[KeyColorTheme.SYSTEM_FILE];

            }
            else if (info.Hidden)
            {
                if (info.Directory)
                    c = _colorTheme[KeyColorTheme.HIDDEN_DIR];
                else
                    c = _colorTheme[KeyColorTheme.HIDDEN_FILE];
            }
            else if (info.ReadOnly)
            {
                if (info.Directory)
                    c = _colorTheme[KeyColorTheme.READONLY_DIR];
                else
                    c = _colorTheme[KeyColorTheme.READONLY_FILE];
            }
            else if (info.Directory)
            {
                c = _colorTheme[KeyColorTheme.DIRECTORY];
            }
            // FILES Only from here
            else if (
                _extensions.Any((x) => x == info.Extension.ToUpper())
                )
            {
                c = _colorTheme[KeyColorTheme.EXE];
            }
            else
            {
                // generic FILE
                c = _colorTheme[KeyColorTheme.FILE];
            }

            return setColor(c) + str + RESET;
        }
    }
}
