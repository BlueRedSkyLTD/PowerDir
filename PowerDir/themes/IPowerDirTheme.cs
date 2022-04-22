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
    }
}
