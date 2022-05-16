using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDir.themes;

namespace PowerDir
{
    static internal class GetPowerDirAbout
    {
        internal static readonly string line1 = "About GetPowerDir";
        internal static readonly string line2 = "Copyright " + new Rune(0x00A9).ToString() + " BlueRedSky Ltd.";
        internal static readonly string line3 = "";
        internal static readonly string line4 = "Theme Settings:";
        
        internal static string showTheme(IPowerDirTheme theme)
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.DIRECTORY, "Directory"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.FILE, "File"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.EXE, "Exe"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.LINK, "Link"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.HIDDEN_DIR, "Hidden Directory"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.HIDDEN_FILE, "Hidden File"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.SYSTEM_DIR, "System Directory"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.SYSTEM_FILE, "System File"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.READONLY_DIR, "Readonly Directory"))
                .AppendLine(theme.colorizeProperty(IPowerDirTheme.KeyColorTheme.READONLY_FILE, "Readonly File"));
            return sb.ToString();
        }
    }
}
