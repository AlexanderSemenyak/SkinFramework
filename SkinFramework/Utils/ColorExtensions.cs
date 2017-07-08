using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinFramework.Utils
{
    public static class ColorExtensions
    {

        public static Pen ToPen(this Color color, float width = 1.0f)
        {
            return new Pen(color, width);
        }

        public static Brush ToBrush(this Color color)
        {
            return new SolidBrush(color);
        }

    }
}
