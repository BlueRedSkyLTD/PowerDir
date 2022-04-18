using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{ 
    /// <summary>
    /// TODO instead of delegate functions, could use events
    /// TODO Review the delegate functions later on when doing with escape codes
    /// at the moment WriteObject is outside the view
    /// </summary>
    public abstract class AbstractView : IView
    {
        const int MAX_NAME_LENGTH = 50;
        /// <summary>
        /// 
        /// </summary>
        public int NameMaxLength { get; } = MAX_NAME_LENGTH;
        /// <summary>
        /// 
        /// </summary>
        protected readonly string _fmt_name = "{0," + -MAX_NAME_LENGTH + "}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected delegate void Write(string msg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        protected delegate void WriteColor(string msg, PowerDirTheme.ColorThemeItem color);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected delegate void WriteLine(string msg = "");
        /// <summary>
        /// 
        /// </summary>
        protected readonly Write _write;
        /// <summary>
        /// 
        /// </summary>
        protected  WriteColor _writeColor;
        /// <summary>
        /// 
        /// </summary>
        protected readonly WriteLine _writeLine;

        // TODO remove from the constructor, also make it optional? like "no theme"
        //      also it might requires a setter to set a theme.
        // TODO also try to do a zero arg constructor
        /// <summary>
        /// 
        /// </summary>
        protected PowerDirTheme _theme;
        /// <summary>
        /// 
        /// </summary>
        protected AbstractView() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writeFunc"></param>
        /// <param name="writeColorFunc"></param>
        /// <param name="writeLineFunc"></param>
        /// <param name="theme"></param>
        protected AbstractView(
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameMaxLength"></param>
        /// <param name="writeFunc"></param>
        /// <param name="writeColorFunc"></param>
        /// <param name="writeLineFunc"></param>
        /// <param name="theme"></param>
        protected AbstractView(
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sb"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        public virtual void endDisplay() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        public abstract void displayResults(IReadOnlyCollection<GetPowerDirInfo> results);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public abstract void displayResult(GetPowerDirInfo result);
    }
}
