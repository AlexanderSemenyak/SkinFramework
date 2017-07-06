using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinFramework.DefaultSkins.VS2017.VSTheme
{
    public class Palette
    {

        public PaletteColor Base { get; set; }
        public PaletteColor Border { get; set; }
        public PaletteColor Text { get; set; }
        public PaletteColor Glyph { get; set; }

    }

    public class PaletteColor
    {
        public Color Color { get; set; }

        public PaletteColor(Color color)
        {
            Color = color;
        }

        public static implicit operator PaletteColor(int val)
        {
            return new PaletteColor(Color.FromArgb(val));
        }

        public static implicit operator PaletteColor(uint val)
        {
            return new PaletteColor(Color.FromArgb((int)val));
        }

        public static implicit operator Color(PaletteColor val)
        {
            return val.Color;
        }

    }
}
