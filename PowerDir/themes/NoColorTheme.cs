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

        public string colorizeRelativeName(GetPowerDirInfo info)
        {
            return info.RelativeName;
        }
    }
}
