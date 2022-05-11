using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    internal interface IPowerDirTheme
    {
        /// <summary>
        /// Mapping colors
        /// </summary>
        public enum KeyColorTheme
        {
            DIRECTORY = 0,
            FILE,
            EXE,
            LINK,
            HIDDEN_DIR,
            HIDDEN_FILE,
            SYSTEM_DIR,
            SYSTEM_FILE,
            READONLY_DIR,
            READONLY_FILE,
            ORIGINAL,
        }

        const string PATHEXT = "PATHEXT";

        /// <summary>
        /// Process all fields and return a colorized GetPowerDirInfo object
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public GetPowerDirInfo colorize(GetPowerDirInfo info);

        /// <summary>
        /// process RelativeName and return it colorized
        /// </summary>
        /// <param name="info"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public string colorizeProperty(GetPowerDirInfo info, string str);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyColorTheme"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public string colorizeProperty(KeyColorTheme keyColorTheme, string str);
    }
}
