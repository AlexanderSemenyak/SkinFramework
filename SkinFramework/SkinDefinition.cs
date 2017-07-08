using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkinFramework
{
    public class SkinDefinition
    {

        public Padding WindowBorderSize { get; set; }
        public Color WindowBorderColor { get; set; }

        public Color WindowBackgroundColor { get; set; }
        

        public ContentAlignment WindowCaptionAlignment { get; set; }
        public Padding WindowCaptionPadding { get; set; }
        public Color WindowCaptionForegroundColor { get; set; }
        public Color WindowCaptionBackgroundColor { get; set; }

        public HorizontalAlignment WindowCaptionControlAlignment { get; set; }
        public ContentAlignment WindowCaptionControlIconAlignment { get; set; }
        public Color WindowCaptionControlBackgroundColor { get; set; }
        public Color WindowCaptionControlForegroundColor { get; set; }
        public Size WindowCaptionControlSize { get; set; }
        public Margins WindowCaptionControlMargin { get; set; }

    }
}