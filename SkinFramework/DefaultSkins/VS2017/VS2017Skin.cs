using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SkinFramework.DefaultSkins.VS2017.Shadows;
using SkinFramework.Painting;

namespace SkinFramework.DefaultSkins.VS2017
{
    public enum ButtonState
    {
        Active = 0,
        Down = 1,
        HoverActive = 2,
        HoverInactive = 3,
        Inactive = 4
    }

    public class VS2017Skin : SkinBase
    {
        public override Padding NCPadding => _ncPadding;
        public override uint CaptionHeight => 34;

        #region Fields

        private ControlPaintHelper _formCaption;
        private ControlPaintHelper _formBorder;

        private ControlPaintHelper _formCaptionButton;

        private ImageStrip _formCloseIcon;
        private ImageStrip _formRestoreIcon;
        private ImageStrip _formMaximizeIcon;
        private ImageStrip _formMinimizeIcon;

        private Color _formActiveTitleColor;
        private Color _formInactiveTitleColor;
        private bool _formIsTextCentered;

        protected ResourceManager _currentManager;
        private Padding _ncPadding;

        #endregion

        public VS2017Skin()
        {

        }

        /// <summary>
        ///     Called when the skin is loaded.
        /// </summary>
        public override void OnLoad()
        {
            try
            {
                LoadResourceManager();
                var skinDef = new XmlDocument();
                skinDef.LoadXml(_currentManager.GetString("SkinDefinition"));


                var elm = skinDef.DocumentElement;
                XmlNode form = elm["Form"];
                XmlNode captionNode = form["Caption"];
                XmlNode normalButton = captionNode["NormalButton"];
                XmlNode smallButton = captionNode["SmallButton"];

                // Background
                _formBorder =
                    new ControlPaintHelper(PaintHelperData.Read(form["Border"], _currentManager, "FormBorder"));
                _formCaption =
                    new ControlPaintHelper(PaintHelperData.Read(captionNode["Background"], _currentManager,
                        "FormCaption"));
                //  calculate NC
                _ncPadding = new Padding(8, 0, 8, 8);


                // Big Buttons
                var imageSize = PaintHelperData.StringToSize(normalButton["IconSize"].InnerText);

                _formCloseIcon = new ImageStrip(true, imageSize, (Bitmap)_currentManager.GetObject("CloseIcon"));
                _formRestoreIcon = new ImageStrip(true, imageSize, (Bitmap)_currentManager.GetObject("RestoreIcon"));
                _formMaximizeIcon = new ImageStrip(true, imageSize, (Bitmap)_currentManager.GetObject("MaximizeIcon"));
                _formMinimizeIcon = new ImageStrip(true, imageSize, (Bitmap)_currentManager.GetObject("MinimizeIcon"));
                _formCaptionButton =
                    new ControlPaintHelper(PaintHelperData.Read(normalButton, _currentManager, "FormCaptionButton"));

                // General Infos
                _formActiveTitleColor = PaintHelperData.StringToColor(form["ActiveCaption"].InnerText);
                _formInactiveTitleColor = PaintHelperData.StringToColor(form["InactiveCaption"].InnerText);
                _formIsTextCentered = PaintHelperData.StringToBool(form["CenterCaption"].InnerText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Invalid SkinDefinition XML", e);
            }
        }

        /// <summary>
        ///     Loads the resource manager assigned to the current <see cref="OfficeStyle" />.
        /// </summary>
        protected virtual void LoadResourceManager()
        {
            _currentManager = VS2017Dark.ResourceManager;
        }

        public override void OnSetRegion(Form form, Size size)
        {
            if (form == null)
                return;

            // Create a rounded rectangle using Gdi
            //var cornerSize = new Size(9, 9);
            var hRegion = Win32Api.CreateRectRgn(0, 0, size.Width + 1, size.Height + 1);
            //var hRegion = Win32Api.CreateRoundRectRgn(0, 0, size.Width + 1, size.Height + 1, cornerSize.Width,
            //    cornerSize.Height);
            var region = Region.FromHrgn(hRegion);
            form.Region = region;

            region.ReleaseHrgn(hRegion);
        }

        /// <summary>
        ///     Gets the button paint indices.
        /// </summary>
        /// <param name="button">The button paint info.</param>
        /// <param name="active">A value indicating whether the button is active.</param>
        /// <param name="buttonIndex">Index of the button icon image strip.</param>
        /// <param name="rendererIndex">Index of the button background image strip.</param>
        private static void GetButtonData(CaptionButtonPaintData button, bool active, out int buttonIndex,
            out int rendererIndex)
        {
            if (!button.Enabled)
            {
                buttonIndex = (int)ButtonState.Inactive;
                rendererIndex = (int)ButtonState.Inactive;
            }
            else if (button.Pressed)
            {
                //buttonIndex = active ? (int)ButtonState.Down : (int)ButtonState.HoverInactive;
                buttonIndex = (int)ButtonState.Down;
                rendererIndex = (int)ButtonState.Down;
            }
            else if (button.Hovered)
            {
                //buttonIndex = active ? (int)ButtonState.HoverActive : (int)ButtonState.HoverInactive;
                buttonIndex = (int)ButtonState.HoverActive;
                //rendererIndex = active ? (int)ButtonState.HoverActive : (int)ButtonState.HoverInactive;
                rendererIndex = (int)ButtonState.HoverActive;
            }
            else
            {
                //buttonIndex = active ? (int)ButtonState.Active : (int)ButtonState.Inactive;
                buttonIndex = (int)ButtonState.Active;
                rendererIndex = (int)ButtonState.Active;
            }
        }

        /// <summary>
        ///     Called when the non client area of the form needs to be painted.
        /// </summary>
        /// <param name="form">The form which gets drawn.</param>
        /// <param name="paintData">The paint data to use for drawing.</param>
        /// <returns><code>true</code> if the original painting should be suppressed, otherwise <code>false</code></returns>
        public override bool OnNcPaint(Form form, SkinningFormPaintData paintData)
        {
            if (form == null) return false;
            form.BackColor = Color.FromArgb(255, 45, 45, 48);
            //paintData.Graphics.CompositingMode = CompositingMode.SourceOver;

            var isMaximized = form.WindowState == FormWindowState.Maximized;
            var isMinimized = form.WindowState == FormWindowState.Minimized;

            // prepare bounds
            var windowBounds = paintData.Bounds;
            windowBounds.Location = Point.Empty;

            var captionBounds = windowBounds;
            var borderSize = paintData.Borders;
            //var borderSize = new Rectangle(2, 2, 2, 2);
            captionBounds.X += borderSize.Width;
            captionBounds.Width -= borderSize.Width * 2;
            captionBounds.Y += borderSize.Height;
            //captionBounds.Height -= borderSize.Width * 2;
            captionBounds.Height = /*borderSize.Height +*/ paintData.CaptionHeight;

            var textBounds = captionBounds;
            var iconBounds = captionBounds;
            iconBounds.Inflate(-borderSize.Width, 0);
            iconBounds.Y += borderSize.Height;
            iconBounds.Height -= borderSize.Height;

            // Draw Caption
            //PaintAsNearestNeighbour(paintData.Graphics, g =>
            //{
            var active = paintData.Active;
            _formCaption.Draw(paintData.Graphics, captionBounds, active ? 0 : 1);
            //});

            // Paint Icon
            if (paintData.HasMenu && form.Icon != null)
            {
                iconBounds.Size = paintData.IconSize;
                var tmpIcon = new Icon(form.Icon, paintData.IconSize);
                iconBounds.Y = captionBounds.Y + (captionBounds.Height - iconBounds.Height) / 2;
                paintData.Graphics.DrawIcon(tmpIcon, iconBounds);
                textBounds.X = iconBounds.Right;
                iconBounds.Width -= iconBounds.Right;
            }

            // Paint Icons
            foreach (var data in paintData.CaptionButtons)
            {
                var painter = _formCaptionButton;

                // Get Indices for imagestrip
                int iconIndex;
                int backgroundIndex;
                GetButtonData(data, paintData.Active, out iconIndex, out backgroundIndex);

                // get imageStrip for button icon
                ImageStrip iconStrip;
                switch (data.HitTest)
                {
                    case HitTest.HTCLOSE:
                        iconStrip = _formCloseIcon;
                        break;
                    case HitTest.HTMAXBUTTON:
                        if (isMaximized)
                            iconStrip = _formRestoreIcon;
                        else
                            iconStrip = _formMaximizeIcon;
                        break;
                    case HitTest.HTMINBUTTON:
                        if (isMinimized)
                            iconStrip = _formRestoreIcon;
                        else
                            iconStrip = _formMinimizeIcon;
                        break;
                    default:
                        continue;
                }

                // draw background
                if (backgroundIndex >= 0)
                    painter.Draw(paintData.Graphics, data.Bounds, backgroundIndex);

                // draw Icon 
                var b = data.Bounds;
                b.Y += 1;
                if (iconIndex >= 0)
                    iconStrip.Draw(paintData.Graphics, iconIndex, b, Rectangle.Empty,
                        DrawingAlign.Center, DrawingAlign.Center);
                // Ensure textbounds
                textBounds.Width -= data.Bounds.Width;
            }

            // draw text
            if (!string.IsNullOrEmpty(paintData.Text) && !textBounds.IsEmpty)
            {
                var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                            TextFormatFlags.PreserveGraphicsClipping;
                if (_formIsTextCentered)
                    flags = flags | TextFormatFlags.HorizontalCenter;
                var font = paintData.IsSmallCaption ? SystemFonts.SmallCaptionFont : SystemFonts.CaptionFont;
                TextRenderer.DrawText(paintData.Graphics, paintData.Text, font, textBounds,
                    paintData.Active ? _formActiveTitleColor : _formInactiveTitleColor, flags);
            }

            // exclude caption area from painting
            var region = paintData.Graphics.Clip;
            region.Exclude(captionBounds);
            paintData.Graphics.Clip = region;

            var bounds = windowBounds;
            //bounds.X -= 5;
            //bounds.Y -= 32;
            //bounds.Width += 10;
            //bounds.Height += 36;

            // Paint borders and corners
            // PaintAsNearestNeighbour(paintData.Graphics, g =>
            //{
            _formBorder.Draw(paintData.Graphics, bounds, paintData.Active ? 0 : 1);
            //});

            paintData.Graphics.ResetClip();
            return true;
        }

        private void PaintAsNearestNeighbour(Graphics g, Action<Graphics> action)
        {
            var nnMode = g.BeginContainer();
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            action?.Invoke(g);

            g.EndContainer(nnMode);

        }

        public override FormShadowBase OnCreateShadow(Form form)
        {
            var color = Form.ActiveForm == form ? Color.FromArgb(255, 0, 122, 204) : Color.FromArgb(255, 67, 67, 70);
            return new FormGlowShadow(form, color, 16, 64, 2);
        }

        /*
        public override bool OnNcPaint(Form form, SkinningFormPaintData paintData)
        {
            if (form == null) return false;

            var isMaximized = form.WindowState == FormWindowState.Maximized;
            var isMinimized = form.WindowState == FormWindowState.Minimized;


            // prepare bounds
            var windowBounds = paintData.Bounds;
            windowBounds.Location = Point.Empty;

            var captionBounds = windowBounds;
            var borderSize = paintData.Borders;
            captionBounds.Height = /*borderSize.Height +*//* paintData.CaptionHeight;

            var textBounds = captionBounds;
            var iconBounds = captionBounds;
            iconBounds.Inflate(-borderSize.Width, 0);
            iconBounds.Y += borderSize.Height;
            iconBounds.Height -= borderSize.Height;

            // Draw Caption
            var active = paintData.Active;
            _formCaption.Draw(paintData.Graphics, captionBounds, active ? 0 : 1);

            // Paint Icon
            if (paintData.HasMenu && form.Icon != null)
            {
                iconBounds.Size = paintData.IconSize;
                var tmpIcon = new Icon(form.Icon, paintData.IconSize);
                iconBounds.Y = captionBounds.Y + (captionBounds.Height - iconBounds.Height) / 2;
                paintData.Graphics.DrawIcon(tmpIcon, iconBounds);
                textBounds.X = iconBounds.Right;
                iconBounds.Width -= iconBounds.Right;
            }

            var g = paintData.Graphics;

            var contentRect = windowBounds;
            contentRect.X += borderSize.Width;
            contentRect.Y += borderSize.Height;
            contentRect.Width -= borderSize.Width * 2;
            contentRect.Height -= borderSize.Height * 2;

            var borderWidths = new Padding(borderSize.Width, borderSize.Height, borderSize.Width, borderSize.Height);

            //ColorPaintHelper.DrawContent(g, windowBounds, Palette.Environment.WindowFrame);
            ColorPaintHelper.DrawFrame(g, paintData.Bounds, borderWidths, Palette.Environment.WindowFrame);

            // exclude caption area from painting
            var region = paintData.Graphics.Clip;
            region.Exclude(captionBounds);
            paintData.Graphics.Clip = region;

            g.ResetClip();
            return true;
        }*/
    }
}
