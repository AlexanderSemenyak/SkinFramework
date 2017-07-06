// This file is part of CoderLine SkinFramework.
//
// CoderLine SkinFramework is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// CoderLine SkinFramework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with CoderLine SkinFramework.  If not, see <http://www.gnu.org/licenses/>.
//
// (C) 2010 Daniel Kuschny, (http://www.coderline.net)

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;
using System.Xml;

namespace SkinFramework.Painting
{
    /// <summary>
    ///     This class handles the loading of resources and drawing of all controls.
    /// </summary>
    internal class ControlPaintHelper
    {
        #region Fields

        private readonly PaintHelperData _data;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ControlPaintHelper" /> class.
        /// </summary>
        /// <param name="data">The data container for this ControlPaintHelper.</param>
        public ControlPaintHelper(PaintHelperData data)
        {
            _data = data;

            Images = new ImageStrip { Image = _data.Image, ImageSize = _data.ImageSize };


            if (!_data.ContentBounds.IsEmpty) return;

            var rect = new Rectangle(Point.Empty, _data.ImageSize);
            rect.X += _data.ImagePadding.Left;
            rect.Y += _data.ImagePadding.Top;
            rect.Width -= _data.ImagePadding.Horizontal;
            rect.Height -= _data.ImagePadding.Vertical;
            _data.ContentBounds = rect;
        }

        #endregion

        #region Properties

        public ImageStrip Images { get; }

        #endregion

        #region Paint Methods

        /// <summary>
        ///     Draws the control.
        /// </summary>
        /// <param name="g">The graphics to draw into.</param>
        /// <param name="rect">The target bounds.</param>
        /// <param name="index">The image strip index.</param>
        public void Draw(Graphics g, Rectangle rect, int index)
        {
            // calculate Bounds for contents
            var contentRect = rect;
            contentRect.X += _data.ImagePadding.Left;
            contentRect.Y += _data.ImagePadding.Top;
            contentRect.Width -= _data.ImagePadding.Horizontal;
            contentRect.Height -= _data.ImagePadding.Vertical;

            DrawContent(g, contentRect, index);
            DrawFrame(g, rect, index);
        }

        /// <summary>
        ///     Draws the contents .
        /// </summary>
        /// <param name="g">The graphics to draw into.</param>
        /// <param name="rectContent">The content bounds.</param>
        /// <param name="index">The index.</param>
        private void DrawContent(Graphics g, Rectangle rectContent, int index)
        {
            if (_data.ContentBounds.IsEmpty)
                return;

            var rectImage = _data.ContentBounds;
            Images.Draw(g, index, rectContent, rectImage, DrawingAlign.Stretch, DrawingAlign.Stretch);
        }


        /// <summary>
        ///     Draws the a frame with 4 corners and 4 sides . (3x3 matrix) using the specified data.
        /// </summary>
        /// <param name="g">The graphics to draw into.</param>
        /// <param name="bounds">The bounds to draw into.</param>
        /// <param name="index">The index of the image to use.</param>
        public void DrawFrame(Graphics g, Rectangle bounds, int index)
        {
            // The bounds of the image to use
            var rectImage = new Rectangle(Point.Empty, _data.ImageSize);

            // The Bounds to draw into
            var targetBounds = bounds;

            var padding = _data.ImagePadding;

            targetBounds.X += padding.Left;
            targetBounds.Y += padding.Top;
            targetBounds.Width -= padding.Horizontal;
            targetBounds.Height -= padding.Vertical;

            // Need drawing?
            if (targetBounds.Width > 0 || targetBounds.Height > 0)
            {
                DrawSides(g, index, bounds, rectImage, targetBounds, padding, padding);

                DrawCorners(g, index, bounds, rectImage, padding);
            }
        }

        private void DrawSides(Graphics g, int index, Rectangle bounds, Rectangle rectImage,
            Rectangle targetBounds, Padding padSides, Padding padCorners)
        {
            // left and right side needed?
            if (targetBounds.Height > 0)
            {
                // the subimage for the right side.
                // Horizontal: Left
                // Vertical: stretch
                if (padSides.Left > 0)
                    Images.Draw(g, index,
                        new Rectangle(bounds.X, targetBounds.Y, padSides.Left, targetBounds.Height),
                        new Rectangle(rectImage.Left, rectImage.Top + padCorners.Top,
                            padSides.Left, rectImage.Height - padCorners.Bottom - padCorners.Top),
                        DrawingAlign.Near, DrawingAlign.Stretch);

                // the subimage for the right side.
                // Horizontal: Right
                // Vertical: stretch
                if (padSides.Right > 0)
                    Images.Draw(g, index,
                        new Rectangle(bounds.Right - padSides.Right, targetBounds.Y, padSides.Right,
                            targetBounds.Height),
                        new Rectangle(rectImage.Right - padSides.Right, rectImage.Y + padCorners.Top,
                            padSides.Right, rectImage.Height - padCorners.Bottom - padCorners.Top),
                        DrawingAlign.Far, DrawingAlign.Stretch);
            }

            // top and bottom side needed?
            if (targetBounds.Width > 0)
            {
                // the subimage for the top side.
                // Horizontal: stretch
                // Vertical: top
                if (padSides.Top > 0)
                    Images.Draw(g, index,
                        new Rectangle(targetBounds.X, bounds.Top, targetBounds.Width, padSides.Top),
                        new Rectangle(rectImage.X + padCorners.Left, rectImage.Top,
                            rectImage.Width - padCorners.Right - padCorners.Left, padSides.Top),
                        DrawingAlign.Stretch, DrawingAlign.Near);

                // the subimage for the bottom side.
                // Horizontal: stretch
                // Vertical: bottom
                if (padSides.Bottom > 0)
                    Images.Draw(g, index,
                        new Rectangle(targetBounds.X, bounds.Bottom - padSides.Bottom, targetBounds.Width,
                            padSides.Bottom),
                        new Rectangle(rectImage.Left + padCorners.Left, rectImage.Bottom - padSides.Bottom,
                            rectImage.Width - padCorners.Right - padCorners.Left, padSides.Bottom),
                        DrawingAlign.Stretch, DrawingAlign.Far);
            }
        }

        private void DrawCorners(Graphics g, int index, Rectangle bounds, Rectangle rectImage, Padding padCorners)
        {
            // Top Left
            if (padCorners.Left > 0 && padCorners.Top > 0)
                Images.Draw(g, index, bounds,
                    new Rectangle(rectImage.Left, rectImage.Top, padCorners.Left, padCorners.Top),
                    DrawingAlign.Near, DrawingAlign.Near);

            // Top Right
            if (padCorners.Right > 0 && padCorners.Top > 0)
                Images.Draw(g, index, bounds,
                    new Rectangle(rectImage.Right - padCorners.Right, rectImage.Top, padCorners.Right, padCorners.Top),
                    DrawingAlign.Far, DrawingAlign.Near);

            // Bottom Left
            if (padCorners.Left > 0 && padCorners.Bottom > 0)
                Images.Draw(g, index, bounds,
                    new Rectangle(rectImage.Left, rectImage.Bottom - padCorners.Bottom, padCorners.Left,
                        padCorners.Bottom),
                    DrawingAlign.Near, DrawingAlign.Far);

            // Bottom Right
            if (padCorners.Right > 0 && padCorners.Bottom > 0)
                Images.Draw(g, index, bounds,
                    new Rectangle(rectImage.Right - padCorners.Right, rectImage.Bottom - padCorners.Bottom,
                        padCorners.Right, padCorners.Bottom),
                    DrawingAlign.Far, DrawingAlign.Far);
        }

        #endregion
    }

    /// <summary>
    ///     This wrapper contains all information for painting a component
    /// </summary>
    public class PaintHelperData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaintHelperData" /> class.
        /// </summary>
        public PaintHelperData()
        {
            ImagePadding = Padding.Empty;
        }

        /// <summary>
        ///     Gets or sets the image strip image used for painting.
        /// </summary>
        /// <value>The image strip image.</value>
        public Bitmap Image { get; set; }

        /// <summary>
        ///     Gets or sets the size of one single image in the image strip.
        /// </summary>
        /// <value>The size of the image.</value>
        public Size ImageSize { get; set; }

        /// <summary>
        ///     Gets or sets the padding indicating the border size.
        /// </summary>
        /// <value>The sides.</value>
        public Padding ImagePadding { get; set; }

        /// <summary>
        ///     Gets or sets the bounds of the content section.
        /// </summary>
        /// <remarks>
        ///     This placeholder is used later to calculate the content bounds.
        /// </remarks>
        /// <value>The content bounds.</value>
        public Rectangle ContentBounds { get; set; }

        public static PaintHelperData Read(XmlNode node, ResourceManager resManager)
        {
            var data = new PaintHelperData();

            var colors = new Color[0];

            var child = node.FirstChild;
            while (child != null)
            {
                switch (child.Name)
                {
                    case "ImageSize":
                        data.ImageSize = StringToSize(child.InnerText);
                        break;
                    case "ImagePadding":
                        data.ImagePadding = StringToPadding(child.InnerText);
                        break;
                    case "ImageResource":
                        data.Image = (Bitmap)resManager.GetObject(child.InnerText);
                        break;
                    case "ImageColors":
                        colors = ReadColorsArray(child);
                        break;
                }
                child = child.NextSibling;
            }

            if (colors.Length > 0)
            {
                data.Image = CreateImageStrip(data.ImageSize, colors);
            }

            return data;
        }

        public static PaintHelperData Read(XmlNode node, ResourceManager resManager, string imageName)
        {
            PaintHelperData data = new PaintHelperData { Image = (Bitmap)resManager.GetObject(imageName) };

            XmlNode child = node.FirstChild;
            while (child != null)
            {
                switch (child.Name)
                {
                    case "ImageSize":
                        data.ImageSize = StringToSize(child.InnerText);
                        break;
                    case "ImagePadding":
                        data.ImagePadding = StringToPadding(child.InnerText);
                        break;
                }
                child = child.NextSibling;
            }
            return data;
        }

        private static Bitmap CreateImageStrip(Size size, params Color[] colors)
        {
            var bitmap = new Bitmap(size.Width, size.Height * colors.Length);
            bitmap.MakeTransparent();

            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                var i = 0;
                foreach (var c in colors)
                {
                    using (var p = new Pen(c))
                    {
                        g.DrawRectangle(p, 0, size.Height * i, size.Width, size.Height);
                    }
                    i++;
                }
                g.Flush(FlushIntention.Flush);
            }

            return bitmap;
        }

        private static Color[] ReadColorsArray(XmlNode node)
        {
            if (!node.HasChildNodes) return new Color[0];

            var colors = new Color[node.ChildNodes.Count];

            var i = 0;
            var child = node.FirstChild;
            while (child != null)
            {
                switch (child.Name)
                {
                    case "Color":
                        colors[i] = StringToColor(child.Attributes.GetNamedItem("Value").Value);
                        break;
                    default:
                        colors[i] = Color.GreenYellow;
                        break;
                }
                i++;
                child = child.NextSibling;
            }

            return colors;
        }

        public static Size StringToSize(string value)
        {
            var split = value.Split(',');
            if (split.Length != 2)
                throw new ApplicationException("Invalid Value for Size");

            return new Size(int.Parse(split[0]), int.Parse(split[1]));
        }

        private static Padding StringToPadding(string value)
        {
            var split = value.Split(',');
            if (split.Length != 4)
                throw new ApplicationException("Invalid Value for Padding");

            return new Padding(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
        }

        public static Color StringToColor(string value)
        {
            var split = value.Split(',');
            if (split.Length != 3)
            {
                if (value.Length == 6 || value.Length == 8)
                {
                    // Try Hex
                    int argb = (int)Convert.ToUInt32("0x" + value, 16);
                    return Color.FromArgb(argb);
                }

                throw new ApplicationException("Invalid Value for Color");
            }
            return Color.FromArgb(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        public static bool StringToBool(string value)
        {
            return bool.Parse(value);
        }
    }
}