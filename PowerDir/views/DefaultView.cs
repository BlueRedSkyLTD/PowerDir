using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultView : AbstractView
    {
        public DefaultView(in Action<object> writeObject) : base(writeObject)
        {}

        public override void displayResult(GetPowerDirInfo result, IPowerDirTheme theme)
        {
            _writeObject(theme.colorize(result));
        }
    }
}
