using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkinFramework.DefaultSkins.VS2017.Shadows
{
    /**
     * ShadowBase class originally from https://github.com/dennismagno/metroframework-modern-ui
     */
    [DesignerCategory("")]
    public abstract class FormShadowBase : Form
    {
        protected Form TargetForm { get; private set; }

        protected readonly int ShadowSize;
        private readonly int wsExStyle;

        protected FormShadowBase(Form targetForm, int shadowSize, int wsExStyle)
        {
            TargetForm = targetForm;
            this.ShadowSize = shadowSize;
            this.wsExStyle = wsExStyle;

            TargetForm.Activated += OnTargetFormActivated;
            TargetForm.ResizeBegin += OnTargetFormResizeBegin;
            TargetForm.ResizeEnd += OnTargetFormResizeEnd;
            TargetForm.VisibleChanged += OnTargetFormVisibleChanged;
            TargetForm.SizeChanged += OnTargetFormSizeChanged;

            TargetForm.Move += OnTargetFormMove;
            TargetForm.Resize += OnTargetFormResize;

            if (TargetForm.Owner != null)
                Owner = TargetForm.Owner;

            TargetForm.Owner = this;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ShowIcon = false;
            FormBorderStyle = FormBorderStyle.None;

            Bounds = GetShadowBounds();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= wsExStyle;
                return cp;
            }
        }

        private Rectangle GetShadowBounds()
        {
            Rectangle r = TargetForm.Bounds;
            r.Inflate(ShadowSize, ShadowSize);
            //r.Offset(-ShadowSize, -ShadowSize);
            return r;
        }

        protected abstract void PaintShadow();

        protected abstract void ClearShadow();

        #region Event Handlers

        private bool isBringingToFront;

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isBringingToFront = true;
        }

        private void OnTargetFormActivated(object sender, EventArgs e)
        {
            if (Visible) Update();
            if (isBringingToFront)
            {
                Visible = true;
                isBringingToFront = false;
                return;
            }
            BringToFront();
        }

        private void OnTargetFormVisibleChanged(object sender, EventArgs e)
        {
            Visible = TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized;
            Update();
        }

        private long lastResizedOn;

        private bool IsResizing { get { return lastResizedOn > 0; } }

        private void OnTargetFormResizeBegin(object sender, EventArgs e)
        {
            lastResizedOn = DateTime.Now.Ticks;
        }

        private void OnTargetFormMove(object sender, EventArgs e)
        {
            if (!TargetForm.Visible || TargetForm.WindowState != FormWindowState.Normal)
            {
                Visible = false;
            }
            else
            {
                Bounds = GetShadowBounds();
            }
        }

        private void OnTargetFormResize(object sender, EventArgs e)
        {
            ClearShadow();
        }

        private void OnTargetFormSizeChanged(object sender, EventArgs e)
        {
            Bounds = GetShadowBounds();

            if (IsResizing)
            {
                return;
            }

            PaintShadowIfVisible();
        }

        private void OnTargetFormResizeEnd(object sender, EventArgs e)
        {
            lastResizedOn = 0;
            PaintShadowIfVisible();
        }

        private void PaintShadowIfVisible()
        {
            if (TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized)
                PaintShadow();
        }

        protected override void Dispose(bool disposing)
        {
            TargetForm.Activated -= OnTargetFormActivated;
            TargetForm.ResizeBegin -= OnTargetFormResizeBegin;
            TargetForm.ResizeEnd -= OnTargetFormResizeEnd;
            TargetForm.VisibleChanged -= OnTargetFormVisibleChanged;
            TargetForm.SizeChanged -= OnTargetFormSizeChanged;

            TargetForm.Move -= OnTargetFormMove;
            TargetForm.Resize -= OnTargetFormResize;
            base.Dispose(disposing);
        }

        #endregion

        #region Constants

        protected const int WS_EX_TRANSPARENT = 0x20;
        protected const int WS_EX_LAYERED = 0x80000;
        protected const int WS_EX_NOACTIVATE = 0x8000000;

        private const int TICKS_PER_MS = 10000;
        private const long RESIZE_REDRAW_INTERVAL = 1000 * TICKS_PER_MS;

        #endregion
    }
}
