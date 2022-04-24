using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    using KeyColorTheme = IPowerDirTheme.KeyColorTheme;
    using Color = Color256;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>")]
    internal class EscapeCodesTheme256 : AbstractEscapeCodesTheme
    {
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

        protected override string getEscapeCodeFg(int fg)
        {
            return fg == -1 ? "" : $"38;5;{fg}";

        }
        protected override string getEscapeCodeBg(int bg)
        {
            return bg == -1 ? "" : $"48;5;{bg}";

        }
    }
}
