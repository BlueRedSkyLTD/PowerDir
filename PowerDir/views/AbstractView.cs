using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{ 
    /// <summary>
    /// TODO instead of delegate functions, could use events
    /// TODO Review the delegate functions later on when doing with escape codes
    /// at the moment WriteObject is outside the view
    /// </summary>
    internal abstract class AbstractView : IView
    {
        public int NameMaxLength { get; } = -1;

        protected delegate void Write(string msg);
        protected delegate void WriteLine(string msg = "");
        protected readonly Write _write;
        protected readonly WriteLine _writeLine;

        protected AbstractView() {}

        protected AbstractView(
            in Action<string> writeFunc,
            in Action<string> writeLineFunc
        ) {
            _write = new Write(writeFunc);
            _writeLine = new WriteLine(writeLineFunc);
        }

        protected AbstractView(
            int nameMaxLength,
            in Action<string> writeFunc,
            in Action<string> writeLineFunc
        ) : this(writeFunc, writeLineFunc)
        {
            NameMaxLength = nameMaxLength;
        }

        //protected void names(GetPowerDirInfo info, StringBuilder sb)
        //{
        //    if (info.Name.Length > NameMaxLength)
        //    {
        //        sb.Append(info.Name.Substring(0, NameMaxLength - 3));
        //        sb.Append("...");
        //    }
        //    else
        //        sb.Append(String.Format(_fmt_name, info.Name));
        //}
     
        //protected string names(GetPowerDirInfo info)
        //{
        //    if (info.RelativeName.Length > NameMaxLength)
        //    {
        //        return info.RelativeName.Substring(0, NameMaxLength - 3) + "...";
        //    }
        //    else
        //        return String.Format(_fmt_name, info.RelativeName);
        //}
        public virtual void endDisplay() { }
        public abstract void displayResult(GetPowerDirInfo result);
        //public abstract void displayResult(GetPowerDirInfo result, IPowerDirTheme theme);
    }
}
