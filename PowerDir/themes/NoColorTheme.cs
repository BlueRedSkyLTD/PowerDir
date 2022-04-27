using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal class NoColorTheme : IPowerDirTheme
    {
        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            return info;
        }

        public string colorizeProperty(GetPowerDirInfo info, string str)
        {
            return str;
        }

        public string colorizeProperty(IPowerDirTheme.KeyColorTheme keyColorTheme, string str)
        {
            return str;
        }
    }
}
