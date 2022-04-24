using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>")]
    internal abstract class AbstractEscapeCodesTheme : IPowerDirTheme
    {
        protected const char ESC = '\x1B';
        protected const string RESET = "\x1B[0m";

        protected readonly string[] _default_extensions = { ".EXE", ".COM", ".BAT", ".CMD", ".PS1" };
        protected readonly HashSet<string> _extensions;

        private readonly StringBuilder _sb = new StringBuilder();
        //private bool _carry = false;
        
        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            info.RelativeName = colorizeProperty(info, info.RelativeName);
            // TODO Size colorized?
            return info;
        }
      
        abstract public string colorizeProperty(GetPowerDirInfo info, string str);

        abstract protected string getEscapeCodeFg(int fg);
        abstract protected string getEscapeCodeBg(int bg);

        //private void doCarry()
        //{
        //    //if (_carry)
        //    //{
        //        _sb.Append(';');
        //        //_carry = false;
        //    //}
        //}

        protected void setBold(bool bold)
        {
            if (!bold)
                return;
            //doCarry();
           _sb.Append('1');
            //_carry = true;
        }
        protected void setDim(bool dim)
        {
            if (!dim)
                return;
            //doCarry();
            _sb.Append(";2");
            //_carry = true;
        }

        protected void setItalic(bool italic)
        {
            if (!italic)
                return;
            //doCarry();
            _sb.Append(";3");
        }

        protected void setUnderline(bool underline)
        {
            if (!underline)
                return;
            //doCarry();
            _sb.Append(";4");
        }

        protected void setBlink(bool blink)
        {
            if (!blink)
                return;
            //doCarry();
            _sb.Append(";5");
        }

        protected void setInverse(bool inverse)
        {
            if (!inverse)
                return;
            //doCarry();
            _sb.Append(";7");
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

        private void setColor(ColorThemeItem col)
        {
            setColor(col.Fg, col.Bg);
        }

        private void setColor(int fg, int bg)
        {
            //doCarry();
            _sb.Append(';')
                .Append(getEscapeCodeFg(fg))
                .Append(';')
                .Append(getEscapeCodeBg(bg));
        }

        protected string colorize(ColorThemeItem col, string str)
        {
            //_carry = false;
            _sb.Clear();
            _sb.Append($"{ESC}[");
            setBold(col.Bold);
            setDim(col.Dim);
            setItalic(col.Italic);
            setUnderline(col.Underline);
            setBlink(col.Blink);
            setInverse(col.Inverse);
            setColor(col);
            _sb.Append('m')
               .Append(str)
               .Append(RESET);

            return _sb.ToString();

            //return $"{ESC}[" + setBold(col.Bold) + setDim(col.Dim) + setItalic(col.Italic)
            //    + setUnderline(col.Underline) + setBlink(col.Blink) + setInverse(col.Inverse)
            //    + setColor(col) + str + RESET;
        }
    }
}
