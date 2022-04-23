using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal abstract class AbstractEscapeCodesTheme : IPowerDirTheme
    {
        protected const char ESC = '\x1B';
        protected const string RESET = "\x1B[0m";

        protected readonly string[] _default_extensions = { ".EXE", ".COM", ".BAT", ".CMD", ".PS1" };
        protected readonly HashSet<string> _extensions;

        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            info.RelativeName = colorizeProperty(info, info.RelativeName);
            // TODO Size colorized?
            return info;
        }
      
        abstract public string colorizeProperty(GetPowerDirInfo info, string str);
        abstract protected string setColor(int fg, int bg);

        protected string setBold(bool bold)
        {
            return bold ? $"{ESC}[1m" : "";
        }
        protected string setDim(bool dim)
        {
            return dim ? $"{ESC}[2m" : "";
        }

        protected string setItalic(bool italic)
        {
            return italic ? $"{ESC}[3m" : "";
        }

        protected string setUnderline(bool underline)
        {
            return underline ? $"{ESC}[4m" : "";
        }

        protected string setBlink(bool blink)
        {
            return blink ? $"{ESC}[5m" : "";
        }

        protected string setInverse(bool inverse)
        {
            return inverse ? $"{ESC}[7m" : "";
        }

        protected AbstractEscapeCodesTheme()
        {
            _extensions = new HashSet<string>(_default_extensions.ToList());
        }

        public static string QueryDevice()
        {
            return $"{ESC}[0c";
        }

        public static string ResponseDevice()
        {
            return $"{ESC}[?1;0c";
        }

        protected string setColor(ColorThemeItem col)
        {
            return setColor(col.Fg, col.Bg);
        }

        protected string colorize(ColorThemeItem col, string str)
        {
            return setBold(col.Bold) + setDim(col.Dim) + setItalic(col.Italic)
                + setUnderline(col.Underline) + setBlink(col.Blink) + setInverse(col.Inverse)
                + setColor(col) + str + RESET;
        }
    }
}
