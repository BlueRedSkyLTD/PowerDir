using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal class EscapeCodesTheme : IPowerDirTheme
    {
        public GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            return info;
        }

        public string colorizeRelativeName(GetPowerDirInfo info)
        {
            //if(info.Link)
            //{
            //    return colorTheme[KeyColorTheme.LINK];
            //}
            //else if (info.System)
            //{
            //    return info.Directory ?
            //        colorTheme[KeyColorTheme.SYSTEM_DIR] :
            //        colorTheme[KeyColorTheme.SYSTEM_FILE];
            //}
            //else if (info.Hidden)
            //{
            //    return info.Directory ?
            //        colorTheme[KeyColorTheme.HIDDEN_DIR] :
            //        colorTheme[KeyColorTheme.HIDDEN_FILE];
            //}
            //else if (info.ReadOnly)
            //{
            //    return info.Directory ?
            //        colorTheme[KeyColorTheme.READONLY_DIR] :
            //        colorTheme[KeyColorTheme.READONLY_FILE];
            //}
            //else if (info.Directory)
            //{
            //    return colorTheme[KeyColorTheme.DIRECTORY];
            //}
            //// FILES Only from here
            //else if (
            //    _extensions.Any((x) => x == info.Extension.ToUpper())
            //    )
            //{
            //    return colorTheme[KeyColorTheme.EXE];
            //}
            //else
            //{
            //    // generic FILE
            //    return colorTheme[KeyColorTheme.FILE];
            //}
            
            return info.RelativeName;
        }
    }
}
