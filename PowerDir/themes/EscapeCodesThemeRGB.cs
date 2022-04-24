using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.themes
{
    using KeyColorTheme = IPowerDirTheme.KeyColorTheme;
    using Color = ColorRGB;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S927:Parameter names should match base declaration and other partial definitions", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    internal class EscapeCodesThemeRGB : AbstractEscapeCodesTheme
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

        private int rgbToHex(byte r, byte g, byte b)
        {
            return (r << 16) + (g << 8) + b;
        }

        static readonly Dictionary<KeyColorTheme, ColorThemeItem> _colorTheme = new()
        {
            { KeyColorTheme.DIRECTORY, new ColorThemeItem((int)Color.Blue, (int)Color.Original) },
            { KeyColorTheme.FILE, new ColorThemeItem((int)Color.Gray, (int)Color.Original) },
            { KeyColorTheme.EXE, new ColorThemeItem((int)Color.Green, (int)Color.Original) },
            { KeyColorTheme.LINK, new ColorThemeItem((int)Color.Cyan, (int)Color.Original) },
            { KeyColorTheme.HIDDEN_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkMagenta) },
            { KeyColorTheme.HIDDEN_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkMagenta) },
            { KeyColorTheme.SYSTEM_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkYellow) },
            { KeyColorTheme.SYSTEM_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkYellow) },
            { KeyColorTheme.READONLY_DIR, new ColorThemeItem((int)Color.White, (int)Color.DarkRed) },
            { KeyColorTheme.READONLY_FILE, new ColorThemeItem((int)Color.Gray, (int)Color.DarkRed) },
        };

        private (float, float, float) RgbToHsv(byte r, byte g ,byte b)
        {
            // r,g,b values are from 0 to 1
            // h = [0,360], s = [0,1], v = [0,1]
            //  if s == 0, then h = -1 (undefined)
            float h, s, v;

            float _r = (float)r / (float)byte.MaxValue;
            float _g = (float)g / (float)byte.MaxValue;
            float _b = (float)b / (float)byte.MaxValue;

            float min = Math.Min(Math.Min(_r, _g), _b);
            float max = Math.Max(Math.Max(_r, _g), _b);
            v = max;

            float delta = max - min;
            if (max != 0f)
                s = delta / max;
            else
            {
                s = 0f;
                h = -1f;
                return (h, s, v);
            }

            if (r == max)
                h = (g - b) / delta;
            else if(g == max)
                h = 2f + (b - r) / delta;
            else
                h = 4f + (r - g) / delta;

            h *= 60f;
            if (h < 0f)
                h += 360f;

            return (h, s, v);
        }

        private (byte, byte, byte) HsvToRgb(float h, float s, float v)
        {
            float i;
            byte r, g, b;
            float f, p, q, t;
            float _r, _g, _b;

            if (s == 0)
            {
                byte _v = (byte) (v * byte.MaxValue);
                r = g = b = _v;
                return (r, g, b);
            }

            h /= 60f;
            i = (float)Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));
            switch (i)
            {
                case 0:
                    _r = v;
                    _g = t;
                    _b = p;
                    break;
                case 1:
                    _r = q;
                    _g = v;
                    _b = p;
                    break;
                case 2:
                    _r = p;
                    _g = v;
                    _b = t;
                    break;
                case 3:
                    _r = p;
                    _g = q;
                    _b = v;
                    break;
                case 4:
                    _r = t;
                    _g = p;
                    _b = v;
                    break;
                default:        // case 5:
                    _r = v;
                    _g = p;
                    _b = q;
                    break;
            }

            r = (byte)(_r * byte.MaxValue);
            g = (byte)(_g * byte.MaxValue);
            b = (byte)(_b * byte.MaxValue);

            return (r, g, b);
        }

        private int mixSingleColor(int c1, int c2)
        {
            if (c1 == -1)
                return c2;
            else if (c2 == -1)
                return c1;
            else
            {
                //return (c1 & c2);

                var (r1, g1, b1) = hexToRgb(c1);
                var (r2, g2, b2) = hexToRgb(c2);
                //return rgbToHex((byte)(r1 + r2 / 2), (byte)(g1 + g2 / 2), (byte)(b1 + b2 / 2));
               
                //return rgbToHex((byte)(r1 & r2), (byte)(g1 & g2), (byte)(b1 & b2));

                // TODO try with HSL, HSV transforms

                var (h1,s1, v1) = RgbToHsv(r1,g1,b1);
                var (h2,s2, v2) = RgbToHsv(r2,g2,b2);

                var (r,g,b) = HsvToRgb((h1 + h2), (s1 + s2) / 2f, (v1 + v2) / 2f);
                return rgbToHex(r, g, b);
            }
        }

        private ColorThemeItem mixColors(ColorThemeItem c1, ColorThemeItem c2)
        {
            return new ColorThemeItem(
                 mixSingleColor(c1.Fg, c2.Fg), mixSingleColor(c1.Bg, c2.Bg),
                c1.Bold ^ c2.Bold,
                c1.Dim ^ c2.Dim,
                c1.Italic ^ c2.Italic,
                c1.Underline ^ c2.Underline,
                c1.Blink ^ c2.Blink,
                c1.Inverse ^ c2.Inverse
            );
        }

        public override string colorizeProperty(GetPowerDirInfo info, string str)
        {
            ColorThemeItem c = new ColorThemeItem((int)Color.Original, (int)Color.Original);

            if (info.Link)
            {
               c = mixColors(c, _colorTheme[KeyColorTheme.LINK]);
            }

            if (info.System)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.SYSTEM_DIR] :
                            _colorTheme[KeyColorTheme.SYSTEM_FILE]);
            }

            if (info.Hidden)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.HIDDEN_DIR] :
                            _colorTheme[KeyColorTheme.HIDDEN_FILE]);
            }
            
            if (info.ReadOnly)
            {
                c = mixColors(c, info.Directory ?
                            _colorTheme[KeyColorTheme.READONLY_DIR] :
                            _colorTheme[KeyColorTheme.READONLY_FILE]);
            }
            
            if (info.Directory)
            {
                c = mixColors(c, _colorTheme[KeyColorTheme.DIRECTORY]);
            }
            // FILES Only from here
            else if (_extensions.Any((x) =>
                x.Equals(info.Extension, StringComparison.OrdinalIgnoreCase)))
            {
                c = mixColors(c, _colorTheme[KeyColorTheme.EXE]);
            }
            else
            {
                // generic FILE
                c = mixColors(c, _colorTheme[KeyColorTheme.FILE]);
            }

            return colorize(c, str);
        }

        protected override string getEscapeCodeFg(int fg)
        {
            if (fg == -1) return "";

            var (r, g, b) = hexToRgb(fg);
            return $"38;2;{r};{g};{b}";

        }
        protected override string getEscapeCodeBg(int bg)
        {
            if (bg == -1) return "";

            var (r, g, b) = hexToRgb(bg);
            return $"48;2;{r};{g};{b}";
        }
    }
}
