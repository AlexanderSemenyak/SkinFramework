using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkinFramework.Painting
{
    public static class ColorPaintHelper
    {

        public static void DrawContent(Graphics g, Rectangle bounds, Color color)
        {
            g.DrawRectangle(new Pen(color), bounds);
        }

        public static void DrawFrame(Graphics g, Rectangle bounds, Padding frameWidths, Color color)
        {
            var targetBounds = bounds;
            targetBounds.X += frameWidths.Left;
            targetBounds.Y += frameWidths.Top;
            targetBounds.Width -= frameWidths.Horizontal;
            targetBounds.Height -= frameWidths.Vertical;

            g.DrawRectangle(new Pen(color), bounds);

            /*
            g.DrawLines(new Pen(color, (float)frameWidths.Left), new[]
            {
                new Point(0, 0),
                new Point(0, bounds.Width),
                new Point(bounds.Height, bounds.Width),
                new Point(bounds.Height, 0),
            });*/

            /*
            if (targetBounds.Height > 0)
            {
                if (frameWidths.Left > 0)
                {
                    g.DrawLine(new Pen(color, frameWidths.Left), bounds.X, targetBounds.Y, frameWidths.Left, targetBounds.Height);
                }

                if (frameWidths.Right > 0)
                {
                    g.DrawLine(new Pen(color, frameWidths.Right), bounds.Right - frameWidths.Right, targetBounds.Y, frameWidths.Right, targetBounds.Height);
                }
            }
            if (targetBounds.Width > 0)
            {
                if (frameWidths.Top > 0)
                {
                    g.DrawLine(new Pen(color, frameWidths.Top), targetBounds.X, bounds.Top, targetBounds.Width, frameWidths.Top);
                }

                if (frameWidths.Bottom > 0)
                {
                    g.DrawLine(new Pen(color, frameWidths.Bottom), targetBounds.X, bounds.Bottom - frameWidths.Bottom, targetBounds.Width, frameWidths.Bottom);
                }
            }*/
        }
    }
}
