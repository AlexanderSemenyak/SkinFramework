using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinFramework.Drawing
{
    public class SkinWindowPaintData
    {

        public SkinWindow Window { get; }
        public Graphics Graphics { get; }
        public Rectangle Bounds { get; }

        public SkinWindowPaintData(SkinWindow window, Graphics graphics, Rectangle bounds)
        {
            Window = window;
            Graphics = graphics;
            Bounds = bounds;
        }

    }
}
