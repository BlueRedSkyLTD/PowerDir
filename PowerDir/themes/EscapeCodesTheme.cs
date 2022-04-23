using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S927:Parameter names should match base declaration and other partial definitions", Justification = "<Pending>")]
    internal class EscapeCodesTheme : AbstractEscapeCodesTheme
    {
        /// <summary>
        /// convert Hex color format to RGB
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>(r,g,b)</returns>
        private (byte, byte, byte) hexToRgb(int hex)
        {
            return (
                (byte)((hex >> 16) & 0xFF),
                (byte)((hex >> 8) & 0xFF),
                (byte)((hex) & 0xFF)
            );
        }

        public override GetPowerDirInfo colorize(GetPowerDirInfo info)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override string colorizeProperty(GetPowerDirInfo info, string str)
        {
            // TODO
            throw new NotImplementedException();
        }

        protected override string setColor(int fg_col, int bg_col)
        {
            // TODO
            throw new NotImplementedException();
            
            //var (fr, fg, fb) = hexToRgb(fg_col);
            //var (br, bg, bb) = hexToRgb(bg_col);
            //return $"{ESC}[38;2{fr};{fg};{fb}m{ESC}[48;2;{br};{bg};{bb}m";
        }

        protected override string setBold(bool bold)
        {
            throw new NotImplementedException();
        }

        protected override string setDim(bool dim)
        {
            throw new NotImplementedException();
        }

        protected override string setItalic(bool italic)
        {
            throw new NotImplementedException();
        }

        protected override string setUnderline(bool underline)
        {
            throw new NotImplementedException();
        }

        protected override string setBlink(bool blink)
        {
            throw new NotImplementedException();
        }

        protected override string setInverse(bool inverse)
        {
            throw new NotImplementedException();
        }
    }
}
