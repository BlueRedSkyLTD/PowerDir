using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    // TODO instead of delegate functions, could use events
    internal abstract class AbstractView : IView
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

        // TODO remove from the constructor, also make it optional? like "no theme"
        //      also it might requires a setter to set a theme.
        // TODO also try to do a zero arg constructor
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

            if (info.RelativeName.Length > NameMaxLength)
            {
                return info.RelativeName.Substring(0, NameMaxLength - 3) + "...";
            }
            else
                return String.Format(_fmt_name, info.RelativeName);
        }

        public virtual void endDisplay() { }
        public abstract void displayResults(IReadOnlyCollection<GetPowerDirInfo> results);
        public abstract void displayResult(GetPowerDirInfo result);
    }
}
