using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal class EscapeCodesTheme : IPowerDirTheme
    {
        const char ESC = '\x1B';
        const string RESET = "\x1B[0m";

        private readonly string[] _default_extensions = { ".EXE", ".COM", ".BAT", ".CMD", ".PS1" };
        private readonly HashSet<string> _extensions;

        public EscapeCodesTheme()
        {
            _extensions = new HashSet<string>(_default_extensions.ToList());
        }

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

        private string setColor(int fg_col, int bg_col)
        {
            var (fr, fg, fb) = hexToRgb(fg_col);
            var (br, bg, bb) = hexToRgb(bg_col);
            return $"{ESC}[38;2{fr};{fg};{fb}m{ESC}[48;2;{br};{bg};{bb}m";
        }

        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            return info;
        }

        public string colorizeProperty(GetPowerDirInfo info, string str)
        {
            int fg_col = 0;
            int bg_col = 0;
            if (info.Link)
            {
                fg_col = 0xF0F0FF;
                bg_col = 0;
            }
            else if (info.System)
            {
                if (info.Directory)
                {
                    //colorTheme[KeyColorTheme.SYSTEM_DIR] :
                    fg_col = 0xFFFFFF;
                    bg_col = 0x000000;
                }
                else
                {
                    //colorTheme[KeyColorTheme.SYSTEM_FILE];
                    fg_col += 0x000000;
                    bg_col += 0x000000;
                }

            }
            else if (info.Hidden)
            {
                if (info.Directory)
                {
                    //colorTheme[KeyColorTheme.HIDDEN_DIR] :
                    fg_col += 0x000000;
                    bg_col += 0x000000;
                }
                else
                {
                    //colorTheme[KeyColorTheme.HIDDEN_FILE];
                    fg_col += 0x000000;
                    bg_col += 0x000000;
                }
            }
            else if (info.ReadOnly)
            {
                if (info.Directory)
                {
                    //colorTheme[KeyColorTheme.READONLY_DIR] :
                    fg_col += 0x000000;
                    bg_col += 0x000000;
                }
                else
                {
                    //colorTheme[KeyColorTheme.READONLY_FILE];
                    fg_col += 0x000000;
                    bg_col += 0x000000;
                }
            }
            else if (info.Directory)
            {
                //return colorTheme[KeyColorTheme.DIRECTORY];
                fg_col += 0x000000;
                bg_col += 0x000000;
            }
            // FILES Only from here
            else if (
                _extensions.Any((x) => x == info.Extension.ToUpper())
                )
            {
                //return colorTheme[KeyColorTheme.EXE];
                fg_col += 0x000000;
                bg_col += 0x000000;
            }
            else
            {
                // generic FILE
                //return colorTheme[KeyColorTheme.FILE];
                fg_col += 0x000000;
                bg_col += 0x000000;
            }

            return setColor(fg_col,bg_col) + str + RESET;
        }
    }
}
