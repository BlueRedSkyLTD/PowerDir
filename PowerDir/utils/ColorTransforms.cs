using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.utils
{
    internal static class ColorTransforms
    {
        /// <summary>
        /// convert Hex color format to RGB
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>(r,g,b)</returns>
        static internal (byte, byte, byte) hexToRgb(int hex)
        {
            return (
                (byte)((hex >> 16) & 0xFF),
                (byte)((hex >> 8) & 0xFF),
                (byte)((hex) & 0xFF)
            );
        }

        /// <summary>
        /// convert RGB to HEX color value
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static internal int rgbToHex(byte r, byte g, byte b)
        {
            return (r << 16) + (g << 8) + b;
        }

        /// <summary>
        /// Convert RGB to HSV color space
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static internal (float, float, float) RgbToHsv(byte r, byte g, byte b)
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
            else if (g == max)
                h = 2f + (b - r) / delta;
            else
                h = 4f + (r - g) / delta;

            h *= 60f;
            if (h < 0f)
                h += 360f;

            return (h, s, v);
        }

        /// <summary>
        /// Convert HSV to RGB color space
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static internal (byte, byte, byte) HsvToRgb(float h, float s, float v)
        {
            float i;
            byte r, g, b;
            float f, p, q, t;
            float _r, _g, _b;

            if (s == 0)
            {
                byte _v = (byte)(v * byte.MaxValue);
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
    }
}
