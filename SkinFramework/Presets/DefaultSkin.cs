using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkinFramework.Presets
{
    public class DefaultSkin : Skin
    {
        public override SkinDefinition Load()
        {
            return new SkinDefinition()
            {
                WindowBackgroundColor = Color.FromArgb(179, 228, 251),
                WindowBorderColor = Color.FromArgb(2, 136, 208),
                WindowBorderSize = new Padding(2),

                WindowCaptionAlignment = ContentAlignment.MiddleLeft,
                WindowCaptionPadding = new Padding(3, 3, 3, 3),
                WindowCaptionForegroundColor = Color.FromArgb(254, 254, 254),
                WindowCaptionBackgroundColor = Color.FromArgb(3, 168, 243),

                WindowCaptionControlAlignment = HorizontalAlignment.Right,
                WindowCaptionControlIconAlignment = ContentAlignment.MiddleLeft,
                WindowCaptionControlBackgroundColor = Color.FromArgb(125, 33, 33, 33),
                WindowCaptionControlForegroundColor = Color.FromArgb(179, 228, 251),
                WindowCaptionControlSize = new Size(20, 20),
                WindowCaptionControlMargin = new Margins(0, 5, 5, 0)
            };
        }
    }
}
