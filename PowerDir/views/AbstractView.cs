using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal class AbstractView
    {
        const int MAX_NAME_LENGTH = 50;
        public int NameMaxLength { get; } = MAX_NAME_LENGTH;
        protected readonly string _fmt_name = "{0," + -MAX_NAME_LENGTH + "}";

        protected delegate void Write(string msg);
        protected delegate void WriteLine(string msg = "");
        protected delegate void SetColor(PowerDirTheme.ColorThemeItem color);
        
        protected PowerDirTheme _theme;

        protected readonly Write _write;
        protected readonly WriteLine _writeLine;
        protected readonly SetColor _setColor;

        internal AbstractView(
            in Action<string> writeFunc,
            in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme
        ) {
            _write = new Write(writeFunc);
            _writeLine = new WriteLine(writeLineFunc);
            _setColor = new SetColor(setColorFunc);
            _theme = theme;
        }

        internal AbstractView(
            int nameMaxLength,
            in Action<string> writeFunc,
            in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme
        ) : this(writeFunc, writeLineFunc, setColorFunc, theme)
        {
            NameMaxLength = nameMaxLength;
            _fmt_name = "{0," + -NameMaxLength + "}";
        }

        protected void names(GetPowerDirInfo info, StringBuilder sb)
        {
            if (info.Name.Length > NameMaxLength)
            {
                sb.Append(info.Name.Substring(0, NameMaxLength - 3));
                sb.Append("...");
            }
            else
                sb.Append(String.Format(_fmt_name, info.Name));
        }

        protected string names(GetPowerDirInfo info)
        {
            StringBuilder sb = new StringBuilder();
            names(info, sb);
            return sb.ToString();
        }

    }
}
