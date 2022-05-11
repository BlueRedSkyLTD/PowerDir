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

        protected delegate void WriteObject(object msg);
        protected readonly WriteObject _writeObject;
        protected AbstractView(in Action<object> writeObject)
        {
            _writeObject = new WriteObject(writeObject);
        }

        protected AbstractView(
            int nameMaxLength,
            in Action<object> writeObject
        ) : this(writeObject)
        {
            NameMaxLength = nameMaxLength;
        }

        public virtual void endDisplay() { }
        public abstract void displayResult(GetPowerDirInfo result, IPowerDirTheme theme);

        internal string names(string relativeName)
        {
            //if (NameMaxLength == -1)
            //    return relativeName;
            if (relativeName.Length > NameMaxLength)
                return relativeName.Substring(0, NameMaxLength - 3) + "...";

            return String.Format("{0," + -NameMaxLength + "}", relativeName);
        }
    }
}
