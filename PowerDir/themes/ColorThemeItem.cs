using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    // avoid to use Generics
    internal class ColorThemeItem
    {
        public int Fg { get; }
        public int Bg { get; }

        public ColorThemeItem(int fg, int bg)
        {
            Fg = fg;
            Bg = bg;
        }
    }
}
