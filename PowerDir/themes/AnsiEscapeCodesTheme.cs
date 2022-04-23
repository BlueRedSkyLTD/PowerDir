using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    using KeyColorTheme = IPowerDirTheme.KeyColorTheme;
    //using ColorThemeItem = ColorThemeItem<Color>;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>")]
    internal class AnsiEscapeCodesTheme : AbstractEscapeCodesTheme
    {
        ///  This instead of enum to avoid explicit casting or using Generics
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
        //static class Color
        //{
        //    public const int Black = 0;
        //    public const int DarkRed = 1;
        //    public const int DargGreen = 2;
        //    public const int DarkYellow = 3;
        //    public const int DarkBlue = 4;
        //    public const int DarkMagenta = 5;
        //    public const int DarkCyan = 6;
        //    public const int Gray = 7;
        //    public const int DarkGray = 8;
        //    public const int Red = 9;
        //    public const int Green = 10;
        //    public const int Yellow = 11;
        //    public const int Blue = 12;
        //    public const int Magenta = 13;
        //    public const int Cyan = 14;
        //    public const int White = 15;
        //}

        enum Color {
            Original = -1,
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

        static readonly Dictionary<KeyColorTheme, ColorThemeItem> _colorTheme = new()
        {
            { KeyColorTheme.DIRECTORY,     new ColorThemeItem((int) Color.Blue,  (int) Color.Original) },
            { KeyColorTheme.FILE,          new ColorThemeItem((int) Color.Gray,  (int) Color.Original) },
            { KeyColorTheme.EXE,           new ColorThemeItem((int) Color.Green, (int) Color.Original, bold:true) },
            { KeyColorTheme.LINK,          new ColorThemeItem((int) Color.Cyan,  (int) Color.Original) },
            { KeyColorTheme.HIDDEN_DIR,    new ColorThemeItem((int) Color.White, (int) Color.DarkMagenta) },
            { KeyColorTheme.HIDDEN_FILE,   new ColorThemeItem((int) Color.Gray,  (int) Color.DarkMagenta) },
            { KeyColorTheme.SYSTEM_DIR,    new ColorThemeItem((int) Color.White, (int) Color.DarkYellow) },
            { KeyColorTheme.SYSTEM_FILE,   new ColorThemeItem((int) Color.Gray,  (int) Color.DarkYellow) },
            { KeyColorTheme.READONLY_DIR,  new ColorThemeItem((int) Color.White, (int) Color.DarkRed) },
            { KeyColorTheme.READONLY_FILE, new ColorThemeItem((int) Color.Gray,  (int) Color.DarkRed) },
        };

        public override GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            
            info.RelativeName = colorizeProperty(info, info.RelativeName);
            // TODO Size colorized?
            return info;
        }

        public override string colorizeProperty(GetPowerDirInfo info, string str)
        {
            if (info.Link)
            {
                return colorize(_colorTheme[KeyColorTheme.LINK], str);
            }
            else if (info.System)
            {
                return colorize(info.Directory ?
                            _colorTheme[KeyColorTheme.SYSTEM_DIR] :
                            _colorTheme[KeyColorTheme.SYSTEM_FILE],
                        str);
            }
            else if (info.Hidden)
            {
                return colorize(info.Directory ?
                            _colorTheme[KeyColorTheme.HIDDEN_DIR] :
                            _colorTheme[KeyColorTheme.HIDDEN_FILE],
                        str);
            }
            else if (info.ReadOnly)
            {
                return colorize(info.Directory ?
                            _colorTheme[KeyColorTheme.READONLY_DIR] :
                            _colorTheme[KeyColorTheme.READONLY_FILE],
                        str);
            }
            else if (info.Directory)
            {
                return colorize(_colorTheme[KeyColorTheme.DIRECTORY], str);
            }
            // FILES Only from here
            else if (_extensions.Any((x) =>
                x.Equals(info.Extension, StringComparison.OrdinalIgnoreCase)))
            {
                return colorize(_colorTheme[KeyColorTheme.EXE], str);
            }
            else
            {
                // generic FILE
                return colorize(_colorTheme[KeyColorTheme.FILE], str);
            }
        }

        protected override string setBold(bool bold)
        {
            return bold ? $"{ESC}[1m" : "";
        }
        protected override string setDim(bool dim)
        {
            return dim ? $"{ESC}[2m" : "";
        }

        protected override string setItalic(bool italic)
        {
            return italic ? $"{ESC}[3m" : "";
        }

        protected override string setUnderline(bool underline)
        {
            return underline ? $"{ESC}[4m" : "";
        }

        protected override string setBlink(bool blink)
        {
            return blink ? $"{ESC}[5m" : "";
        }
        
        protected override string setInverse(bool inverse)
        {
            return inverse ? $"{ESC}[7m" : "";
        }
        
        protected override string setColor(int fg, int bg)
        {
            string s = "";
            if (fg != -1)
                s += $"{ESC}[38;5;{fg}m";
            if (bg != -1)
                s += $"{ESC}[48;5;{bg}m";

            return s;
        }
    }
}
