using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerDir.themes
{
    // TODOS:
    // - echo $env:PATHEXT
    // .COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC;.CPL
    // check if PATHEXT is a powershell env variable for executable file
    // in case integrate for the extension to highlight
    /// <summary>
    /// @deprecated
    /// </summary>
    public class PowerDirThemeClassic
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

        const string PATHEXT = "PATHEXT";
        /// <summary>
        /// 
        /// </summary>
        public struct ColorThemeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public ConsoleColor Fg { get; }
            /// <summary>
            /// 
            /// </summary>
            public ConsoleColor Bg { get; }
            internal ColorThemeItem(ConsoleColor fg, ConsoleColor bg)
            {
                this.Fg = fg;
                this.Bg = bg;
            }
        }

        readonly Dictionary<KeyColorTheme, ColorThemeItem> colorTheme = new Dictionary<KeyColorTheme, ColorThemeItem>()
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

        internal readonly HashSet<string> _extensions;
        private readonly string[] _default_extensions = { ".EXE", ".COM", ".BAT", ".CMD", ".PS1" };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original_color"></param>
        public PowerDirThemeClassic(ColorThemeItem original_color)
        {
            colorTheme.Add(KeyColorTheme.ORIGINAL, original_color);
            var pathExt = Environment.GetEnvironmentVariable(PATHEXT, EnvironmentVariableTarget.Process);
            var s = pathExt != null ? pathExt.Split(';') : Array.Empty<string>();
            var l = s.ToList();
            l.AddRange(_default_extensions);
            _extensions = new HashSet<string>(l);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original_fg"></param>
        /// <param name="original_bg"></param>
        public PowerDirThemeClassic(ConsoleColor original_fg, ConsoleColor original_bg) :
            this(new ColorThemeItem(original_fg, original_bg))
        {
        }

        /// <summary>
        /// If not supporting color
        /// </summary>
        public PowerDirThemeClassic()
        {
            colorTheme.Add(KeyColorTheme.ORIGINAL, new ColorThemeItem(ConsoleColor.Gray, ConsoleColor.Black));
            _extensions = new HashSet<string>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ColorThemeItem GetOriginalColor()
        {
            return colorTheme[KeyColorTheme.ORIGINAL];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
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
            else if (
                _extensions.Any((x) => x == info.Extension.ToUpper())
                )
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
