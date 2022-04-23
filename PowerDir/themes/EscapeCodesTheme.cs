using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    using KeyColorTheme = IPowerDirTheme.KeyColorTheme;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S927:Parameter names should match base declaration and other partial definitions", Justification = "<Pending>")]
    internal class EscapeCodesTheme : AbstractEscapeCodesTheme
    {
        /// <summary>
        /// convert Hex color format to RGB
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>(r,g,b)</returns>
        private (byte, byte, byte) hexToRgb(int hex)
        {
            return (
                (byte)((hex >> 16) & 0xFF),
                (byte)((hex >> 8) & 0xFF),
                (byte)((hex) & 0xFF)
            );
        }

        enum Color
        {
            Original = -1,
            Black = 0,
            DarkRed = 0x800000,
            DargGreen = 0x008000,
            DarkYellow = 0x808000,
            DarkBlue = 0x000080,
            DarkMagenta = 0x8000f0,
            DarkCyan = 0x008080,
            Gray = 0xf0f0f0,
            DarkGray = 0x808080,
            Red = 0xff0000,
            Green = 0x00ff00,
            Yellow = 0xffff00,
            Blue = 0x0000ff,
            Magenta = 0xff00ff,
            Cyan = 0x00ffff,
            White = 0xffffff,
        }

        static readonly Dictionary<KeyColorTheme, ColorThemeItem> _colorTheme = new()
        {
            { KeyColorTheme.DIRECTORY, new ColorThemeItem((int)Color.Blue, (int)Color.Original) },
            { KeyColorTheme.FILE, new ColorThemeItem((int)Color.Gray, (int)Color.Original) },
            { KeyColorTheme.EXE, new ColorThemeItem((int)Color.Green, (int)Color.Original) },
            { KeyColorTheme.LINK, new ColorThemeItem((int)Color.Cyan, (int)Color.Original) },
            { KeyColorTheme.HIDDEN_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkMagenta) },
            { KeyColorTheme.HIDDEN_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkMagenta) },
            { KeyColorTheme.SYSTEM_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkYellow) },
            { KeyColorTheme.SYSTEM_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkYellow) },
            { KeyColorTheme.READONLY_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkRed) },
            { KeyColorTheme.READONLY_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkRed) },
        };

        private int mixSingleColor(int c1, int c2)
        {
            if (c1 == -1)
                return c2;
            else if (c2 == -1)
                return c1;
            else
                return (c1 + c2) / 2;
        }
        private ColorThemeItem mixColors(ColorThemeItem c1, ColorThemeItem c2)
        {
            return new ColorThemeItem(
                 mixSingleColor(c1.Fg, c2.Fg), mixSingleColor(c1.Bg, c2.Bg),
                c1.Bold || c2.Bold,
                c1.Dim || c2.Dim,
                c1.Italic || c2.Italic,
                c1.Underline || c2.Underline,
                c1.Blink || c2.Blink,
                c1.Inverse || c2.Inverse
            );
        }

        public override string colorizeProperty(GetPowerDirInfo info, string str)
        {
            ColorThemeItem c = new ColorThemeItem((int)Color.Original, (int)Color.Original);

            if (info.Link)
            {
               c = mixColors(c, _colorTheme[KeyColorTheme.LINK]);
            }

            if (info.System)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.SYSTEM_DIR] :
                            _colorTheme[KeyColorTheme.SYSTEM_FILE]);
            }

            if (info.Hidden)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.HIDDEN_DIR] :
                            _colorTheme[KeyColorTheme.HIDDEN_FILE]);
            }
            
            if (info.ReadOnly)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.READONLY_DIR] :
                            _colorTheme[KeyColorTheme.READONLY_FILE]);
            }
            
            if (info.Directory)
            {
                c = mixColors(c, _colorTheme[KeyColorTheme.DIRECTORY]);
            }
            // FILES Only from here
            else if (_extensions.Any((x) =>
                x.Equals(info.Extension, StringComparison.OrdinalIgnoreCase)))
            {
                c = mixColors(c, _colorTheme[KeyColorTheme.EXE]);
            }
            else
            {
                // generic FILE
                c = mixColors(c, _colorTheme[KeyColorTheme.FILE]);
            }

            return colorize(c, str);
        }

        protected override string setColor(int fg, int bg)
        {
            string s = "";
            if (fg != -1)
            {
                var (r, g, b) = hexToRgb(fg);
                s += $"{ESC}[38;2;{r};{g};{b}m";
            }
            if (bg != -1)
            {
                var (r, g, b) = hexToRgb(bg);
                s+= $"{ESC}[48;2;{r};{g};{b}m";
            }

            return s;
        }
    }
}
