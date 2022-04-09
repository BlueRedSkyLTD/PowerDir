using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    // TODO instead of delegate functions, could use events
    internal class AbstractView
    {
        const int MAX_NAME_LENGTH = 50;
        public int NameMaxLength { get; } = MAX_NAME_LENGTH;
        protected readonly string _fmt_name = "{0," + -MAX_NAME_LENGTH + "}";

        protected delegate void Write(string msg);
        protected delegate void WriteColor(string msg, PowerDirTheme.ColorThemeItem color);
        protected delegate void WriteLine(string msg = "");

        protected readonly Write _write;
        protected  WriteColor _writeColor;
        protected readonly WriteLine _writeLine;

        protected PowerDirTheme _theme;
       
        internal AbstractView(
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme
        ) {
            _write = new Write(writeFunc);
            _writeColor = new WriteColor(writeColorFunc);
            _writeLine = new WriteLine(writeLineFunc);
            _theme = theme;
        }

        internal AbstractView(
            int nameMaxLength,
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme
        ) : this(writeFunc, writeColorFunc, writeLineFunc, theme)
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
            //StringBuilder sb = new StringBuilder();
            //names(info, sb);
            //return sb.ToString();

            if (info.Name.Length > NameMaxLength)
            {
                return info.Name.Substring(0, NameMaxLength - 3) + "...";
            }
            else
                return String.Format(_fmt_name, info.Name);
        }

    }
}
