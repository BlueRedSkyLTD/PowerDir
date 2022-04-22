using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal class NoColorTheme : IPowerDirTheme
    {
        public string colorize(string str)
        {
            return str;
        }
    }
}
