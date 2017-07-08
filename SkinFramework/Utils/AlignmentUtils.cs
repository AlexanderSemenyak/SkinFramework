using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkinFramework.Utils
{
    public static class AlignmentUtils
    {

        private static ContentAlignment[] Left =
            {ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft};
        private static ContentAlignment[] Center =
            {ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter};
        private static ContentAlignment[] Right =
            {ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight};

        private static ContentAlignment[] Top =
            {ContentAlignment.TopLeft, ContentAlignment.TopCenter, ContentAlignment.TopRight};
        private static ContentAlignment[] Middle =
            {ContentAlignment.MiddleLeft, ContentAlignment.MiddleCenter, ContentAlignment.MiddleRight};
        private static ContentAlignment[] Bottom =
            {ContentAlignment.BottomLeft, ContentAlignment.BottomCenter, ContentAlignment.BottomRight};


        public static TextFormatFlags ToTextFormatFlags(this ContentAlignment alignment)
        {
            TextFormatFlags flags = TextFormatFlags.Default;

            if(Left.Contains(alignment))
                flags |= TextFormatFlags.Left;
            else if(Center.Contains(alignment))
                flags |= TextFormatFlags.HorizontalCenter;
            else if(Right.Contains(alignment))
                flags |= TextFormatFlags.Right;

            if(Top.Contains(alignment))
                flags |= TextFormatFlags.Top;
            else if(Middle.Contains(alignment))
                flags |= TextFormatFlags.VerticalCenter;
            else if(Bottom.Contains(alignment))
                flags |= TextFormatFlags.Bottom;

            return flags;
        }

    }
}
