using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    internal interface IView
    {
        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results);
        public void displayResult(GetPowerDirInfo result);
        public void endDisplay();
    }
}
