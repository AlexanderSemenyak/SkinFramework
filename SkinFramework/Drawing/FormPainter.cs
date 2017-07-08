using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkinFramework.Utils;

namespace SkinFramework.Drawing
{
    public class FormPainter
    {
        protected SkinWindowPaintData Data { get; }

        protected SkinDefinition Skin { get; }

        protected Graphics Graphics => Data.Graphics;


        private Rectangle _captionBounds = Rectangle.Empty;
        protected Rectangle CaptionBounds
        {
            get
            {
                if (_captionBounds != Rectangle.Empty) return _captionBounds;

                var b = Data.Bounds;
                b.Location = Point.Empty;

                var bs = Skin.WindowBorderSize;

                b.X += bs.Left;
                b.Width -= bs.Horizontal;

                b.Y += bs.Top;
                b.Height = 30;
                _captionBounds = b;

                return _captionBounds;
            }
        }


        public FormPainter(SkinWindowPaintData data, SkinDefinition skin)
        {
            Data = data;
            Skin = skin;
        }

        public void Paint()
        {
            Graphics.Clear(Skin.WindowBackgroundColor);

            PaintCaption();
            PaintBorders();
        }
        
        protected void PaintCaption()
        {
            Graphics.FillRectangle(Skin.WindowCaptionBackgroundColor.ToBrush(), CaptionBounds);

            var flags = TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping |
                        Skin.WindowCaptionAlignment.ToTextFormatFlags();

            var bounds = CaptionBounds;
            bounds.X += Skin.WindowCaptionPadding.Left;
            bounds.Y += Skin.WindowCaptionPadding.Top;
            bounds.Width -= Skin.WindowCaptionPadding.Horizontal;
            bounds.Height -= Skin.WindowCaptionPadding.Vertical;

            TextRenderer.DrawText(Graphics, Data.Window.MainForm.Text, SystemFonts.CaptionFont, bounds, Skin.WindowCaptionForegroundColor, flags);
        }

        protected void PaintBorders()
        {
            var r = Graphics.Clip;

            var bounds = Data.Bounds;
            bounds.Location = Point.Empty;

            bounds.X = Skin.WindowBorderSize.Left;
            bounds.Y = Skin.WindowBorderSize.Top;
            bounds.Width -= Skin.WindowBorderSize.Horizontal;
            bounds.Height -= Skin.WindowBorderSize.Vertical;

            r.Exclude(bounds);
            Graphics.Clip = r;

            Graphics.FillRectangle(Skin.WindowBorderColor.ToBrush(), Data.Bounds);
            Graphics.ResetClip();
        }
    }
}
