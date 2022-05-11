using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{
    internal class ListView : AbstractView
    {
        internal ListView(
            in Action<object> writeObject
            ) : base(writeObject)
        {
        }

        public override void displayResult(GetPowerDirInfo result, IPowerDirTheme theme)
        {
            _writeObject(theme.colorizeProperty(result, result.RelativeName));
        }
    }
}
