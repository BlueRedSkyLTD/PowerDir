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

        abstract public GetPowerDirInfo colorize(GetPowerDirInfo info);
        abstract public string colorizeProperty(GetPowerDirInfo info, string str);
        abstract protected string setColor(int fg, int bg);
        abstract protected string setBold(bool bold);
        //abstract protected string setDim(bool dim);
        abstract protected string setItalic(bool italic);
        //abstract protected string setUnderline(bool underline);
        abstract protected string setBlink(bool blink);


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
            string s = setBold(col.Bold) + setItalic(col.Italic) + setBlink(col.Blink) + setColor(col) + str + RESET;
            return s;
        }
    }
}
