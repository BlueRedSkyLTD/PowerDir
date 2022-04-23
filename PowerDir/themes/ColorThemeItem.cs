using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    /// <summary>
    /// Some Escape Codes are only supported with Windows Terminal
    /// </summary>
    internal class ColorThemeItem
    {
        public int Fg { get; }
        public int Bg { get; }

        public bool Bold { get; }
        public bool Dim { get; }
        public bool Italic { get; }
        public bool Underline { get; }
        public bool Blink { get; }
        public bool Inverse { get; }

        //public ColorThemeItem(int fg, int bg)
        //{
        //    Fg = fg;
        //    Bg = bg;
        //    Bold = Italic = Underline = Inverse = false;
        //}

        public ColorThemeItem(int fg, int bg, bool bold = false, bool dim = false, bool italic = false, bool underline = false, bool blink = false, bool inverse = false)
        {
            Fg = fg;
            Bg = bg;
            Bold = bold;
            Dim = dim;
            Italic = italic;
            Underline = underline;
            Blink = blink;
            Inverse = inverse;
        }
    }
}
