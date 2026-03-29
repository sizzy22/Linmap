using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linmap
{
    public class Color
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;



        private Color(byte r, byte g, byte b) : this(255,r,g,b)
        {}
        private Color(byte a, byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Color FromArgb(byte a, byte r, byte g, byte b) => new Color(a, r, g, b);

        public static Color FromArgb(byte r, byte g, byte b) =>  new Color(r, g, b);

        public static string ToHex(Color color) => $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";

        public static Color FromHex(string hex)
        {
            if (hex.Length != 7 && !hex.StartsWith('#'))
            {
                throw new FormatException("The parameter you entered is not recognized as a hex color.");
            }
            return new Color(byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber),
                byte.Parse(hex.Substring(3, 4), NumberStyles.HexNumber),
                byte.Parse(hex.Substring(5, 6), NumberStyles.HexNumber));
        }



        public static Color Red = new Color(255, 0, 0);
        public static Color Lime = new Color(0, 255, 0);
        public static Color Green = new Color(0, 128, 0);
        public static Color Blue = new Color(0, 0, 255);
        public static Color Sea = new Color(0, 255, 255);
        public static Color White = new Color(255, 255, 255);
        public static Color Black = new Color(0, 0, 0);
        public static Color Gray = new Color(128, 128, 128);
        public static Color Yellow = new Color(255, 255, 0);
        public static Color Alpha = new Color(0, 0, 0,0);

    }
}
