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
        /// <summary>
        /// @deprecated
        /// </summary>
        /// <param name="result"></param>
        [Obsolete("this displayResult is deprecated")]
        void displayResult(GetPowerDirInfo result);

        //internal void displayResult(GetPowerDirInfo result, IPowerDirTheme theme);
        void endDisplay();

        int NameMaxLength { get; }
    }
}
