using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir.views
{
    internal interface IView
    {
        void displayResult(GetPowerDirInfo result, IPowerDirTheme theme);
        void endDisplay();

        int NameMaxLength { get; }
    }
}
