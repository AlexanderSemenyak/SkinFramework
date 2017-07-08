using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkinFramework.Drawing;
using SkinFramework.Native;
using SkinFramework.Utils;

namespace SkinFramework
{
    public class SkinWindow : NativeWindow
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether whe should process the nc area.
        /// </summary>
        /// <value>
        ///     <c>true</c> if we should process the nc area; otherwise, <c>false</c>.
        /// </value>
        private bool IsProcessNcArea => !(MainForm == null ||
                                          MainForm.MdiParent != null &&
                                          MainForm.WindowState == FormWindowState.Maximized);

        #endregion


        internal Form MainForm { get; private set; }
        public bool IsActive { get; private set; }

        private readonly SkinManager _skinManager;

        // graphics data
        private readonly BufferedGraphicsContext _bufferContext;
        private BufferedGraphics _bufferGraphics;
        private Size _currentCacheSize;

        private Size _a;

        public SkinWindow(Form mainForm, SkinManager skinManager)
        {
            MainForm = mainForm;
            _skinManager = skinManager;

            _bufferContext = BufferedGraphicsManager.Current;
            _bufferGraphics = null;

            if (mainForm.Handle != IntPtr.Zero)
                OnHandleCreated(mainForm, EventArgs.Empty);

            RegisterEventHandlers();
        }

        ~SkinWindow()
        {
            UnregisterEventHandlers();
        }


        #region Parent Form Handlers

        /// <summary>
        ///     Registers all important eventhandlers.
        /// </summary>
        private void RegisterEventHandlers()
        {
            MainForm.HandleCreated += OnHandleCreated;
            MainForm.HandleDestroyed += OnHandleDestroyed;
            MainForm.TextChanged += OnTextChanged;
            MainForm.Disposed += OnParentDisposed;
            //MainForm.Closed += OnParentClosed;
            //MainForm.Load += OnParentLoad;
            //MainForm.Activated += OnParentActivated;
           // MainForm.Deactivate += OnParentDeactivate;
        }

        private void OnParentDeactivate(object sender, EventArgs eventArgs)
        {
            //RemoveShadow();
            //CreateShadow(IsActive);
        }

        private void OnParentActivated(object sender, EventArgs eventArgs)
        {
            //RemoveShadow();
            //CreateShadow(IsActive);
        }

        private void OnParentLoad(object sender, EventArgs eventArgs)
        {
        }

        private void OnParentClosed(object sender, EventArgs eventArgs)
        {
            if (MainForm.Owner != null) this.MainForm.Owner = null;
        }

        /// <summary>
        ///     Unregisters all important eventhandlers.
        /// </summary>
        private void UnregisterEventHandlers()
        {
            MainForm.HandleCreated -= OnHandleCreated;
            MainForm.HandleDestroyed -= OnHandleDestroyed;
            MainForm.TextChanged -= OnTextChanged;
            MainForm.Disposed -= OnParentDisposed;
            //MainForm.Closed -= OnParentClosed;
            //MainForm.Load -= OnParentLoad;
            //MainForm.Activated -= OnParentActivated;
            //MainForm.Deactivate -= OnParentDeactivate;
        }

        /// <summary>
        ///     Called when the handle of the parent form is created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnHandleCreated(object sender, EventArgs e)
        {
            // this little line allows us to handle the windowMessages of the parent form in this class
            AssignHandle(((Form)sender).Handle);
            if (IsProcessNcArea)
            {
                UpdateStyle();
                UpdateCaption();
            }
        }

        /// <summary>
        ///     Called when the handle of the parent form is destroyed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            // release handle as it is destroyed
            ReleaseHandle();
        }

        /// <summary>
        ///     Called when the parent of the parent form is disposed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnParentDisposed(object sender, EventArgs e)
        {
            //RemoveShadow();
            // unregister events as the parent of the form is disposed
            if (MainForm != null)
                UnregisterEventHandlers();
            MainForm = null;
        }

        /// <summary>
        ///     Called when the text on the parent form has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnTextChanged(object sender, EventArgs e)
        {
            // Redraw on title change
            if (IsProcessNcArea)
                NcPaint(true);
        }

        #endregion


        /// <summary>
        ///     Updates the window style for the parent form.
        /// </summary>
        private void UpdateStyle()
        {
            // remove the border style
            var currentStyle = Win32Api.GetWindowLong(Handle, GWLIndex.GWL_STYLE);
            if ((currentStyle & (int)WindowStyles.WS_BORDER) != 0 || (currentStyle & (int)WindowStyles.WS_EX_TRANSPARENT) == 0 || (currentStyle & (int)WindowStyles.WS_EX_LAYERED) == 0)
            {
                currentStyle &= ~(int)WindowStyles.WS_BORDER;
                currentStyle |= (int)WindowStyles.WS_EX_TRANSPARENT;
                currentStyle |= (int)WindowStyles.WS_EX_LAYERED;

                Win32Api.SetWindowLong(MainForm.Handle, GWLIndex.GWL_STYLE, currentStyle);
                Win32Api.SetWindowPos(MainForm.Handle, (IntPtr)0, -1, -1, -1, -1,
                    (int)(SWPFlags.SWP_NOZORDER | SWPFlags.SWP_NOSIZE | SWPFlags.SWP_NOMOVE |
                          SWPFlags.SWP_FRAMECHANGED | SWPFlags.SWP_NOREDRAW | SWPFlags.SWP_NOACTIVATE));
                
            }
        }

        /// <summary>
        ///     Redraws the non client area.
        /// </summary>
        /// <param name="invalidateBuffer">if set to <c>true</c> the buffer is invalidated.</param>
        /// <returns>true if the original painting should be suppressed otherwise false.</returns>
        private bool NcPaint(bool invalidateBuffer)
        {
            if (!IsProcessNcArea)
                return false;
            var result = false;

            var hdc = (IntPtr) 0;
            Graphics g = null;
            Region region = null;
            var hrgn = (IntPtr) 0;

            try
            {
                // no drawing needed
                if (MainForm.MdiParent != null && MainForm.WindowState == FormWindowState.Maximized)
                {
                    _currentCacheSize = Size.Empty;
                    return false;
                }

                var borderSize = MainForm.GetBorderSize();
                var captionHeight = 30;

                var rectScreen = new RECT();
                Win32Api.GetWindowRect(MainForm.Handle, ref rectScreen);


                var rectBounds = rectScreen.ToRectangle();
                rectBounds.Offset(-rectBounds.X, -rectBounds.Y);

                // create graphics handle
                hdc = Win32Api.GetDCEx(MainForm.Handle, (IntPtr)0,
                    DCXFlags.Cache | DCXFlags.ClipSiblings | DCXFlags.Window | DCXFlags.ParentClip);
                g = Graphics.FromHdc(hdc);

                // prepare clipping
                var rectClip = rectBounds;
                region = new Region(rectClip);
                
                rectClip.X = _skinManager.CurrentSkin.WindowBorderSize.Left;
                rectClip.Y = _skinManager.CurrentSkin.WindowBorderSize.Top + captionHeight;
                rectClip.Width -= rectClip.X + _skinManager.CurrentSkin.WindowBorderSize.Right;
                rectClip.Height -= rectClip.Y + _skinManager.CurrentSkin.WindowBorderSize.Bottom;


                // Apply clipping
                region.Exclude(rectClip);
                hrgn = region.GetHrgn(g);
                Win32Api.SelectClipRgn(hdc, hrgn);


                // create new buffered graphics if needed
                if (_bufferGraphics == null || _currentCacheSize != rectBounds.Size)
                {
                    if (_bufferGraphics != null)
                        _bufferGraphics.Dispose();

                    _bufferGraphics = _bufferContext.Allocate(g, new Rectangle(0, 0,
                        rectBounds.Width, rectBounds.Height));
                    _currentCacheSize = rectBounds.Size;
                    invalidateBuffer = true;
                }


                if (invalidateBuffer)
                {
                    var data = new SkinWindowPaintData(this, _bufferGraphics.Graphics, rectBounds);
                    _skinManager.PaintSkin(data);
                }

                // render buffered graphics 
                _bufferGraphics?.Render(g);

                result = true;
            }
            catch (Exception)
            {
                // error drawing
                result = false;
            }

            // cleanup data
            if (hdc != (IntPtr)0)
            {
                Win32Api.SelectClipRgn(hdc, (IntPtr)0);
                Win32Api.ReleaseDC(MainForm.Handle, hdc);
            }
            if (region != null && hrgn != (IntPtr)0)
                region.ReleaseHrgn(hrgn);

            region?.Dispose();

            g?.Dispose();

            return result;
        }

        /// <summary>
        ///     Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            var supressOriginalMessage = false;
            if (IsProcessNcArea)
                switch ((Win32Messages)m.Msg)
                {
                    // update form data on style change
                    case Win32Messages.STYLECHANGED:
                        UpdateStyle();
                        _skinManager.SetRegion(this, MainForm.Size);
                        break;

                    #region Handle Form Activation

                    case Win32Messages.ACTIVATEAPP:
                        // redraw
                        IsActive = (int)m.WParam != 0;
                        NcPaint(true);
                        break;

                    case Win32Messages.ACTIVATE:
                        // Set active state and redraw
                        IsActive = (int)WAFlags.WA_ACTIVE == (int)m.WParam ||
                                   (int)WAFlags.WA_CLICKACTIVE == (int)m.WParam;
                        NcPaint(true);
                        break;
                    case Win32Messages.MDIACTIVATE:
                        // set active and redraw on activation 
                        if (m.WParam == MainForm.Handle)
                            IsActive = false;
                        else if (m.LParam == MainForm.Handle)
                            IsActive = true;
                        NcPaint(true);
                        break;

                    #endregion

                    #region Handle Mouse Processing
/*
                    // Set Pressed button on mousedown
                    case Win32Messages.NCLBUTTONDOWN:
                        supressOriginalMessage = OnNcLButtonDown(ref m);
                        break;
                    // Set hovered button on mousemove
                    case Win32Messages.NCMOUSEMOVE:
                        OnNcMouseMove(m);
                        break;
                    // perform button actions if a button was clicked
                    case Win32Messages.NCLBUTTONUP:
                        // Handle button up
                        if (OnNcLButtonUp(m))
                            supressOriginalMessage = true;
                        break;
                    // restore button states on mouseleave
                    case Win32Messages.NCMOUSELEAVE:
                    case Win32Messages.MOUSELEAVE:
                    case Win32Messages.MOUSEHOVER:
                        if (_pressedButton != null)
                            _pressedButton.Pressed = false;
                        if (_hoveredButton != null)
                        {
                            _hoveredButton.Hovered = false;
                            _hoveredButton = null;
                        }
                        NcPaint(true);
                        break;
                        */
                    #endregion

                    #region Size Processing

                    // Set region as window is shown                    
                    case Win32Messages.SHOWWINDOW:
                        _skinManager.SetRegion(this, MainForm.Size);
                        break;
                    // adjust region on resize
                    case Win32Messages.SIZE:
                        OnSize(m);
                        break;
                    // ensure that the window doesn't overlap docked toolbars on desktop (like taskbar)
                    case Win32Messages.GETMINMAXINFO:
                        supressOriginalMessage = CalculateMaxSize(ref m);
                        break;
                    // update region on resize
                    case Win32Messages.WINDOWPOSCHANGING:
                        var wndPos = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));
                        if ((wndPos.flags & (int)SWPFlags.SWP_NOSIZE) == 0)
                            _skinManager.SetRegion(this, new Size(wndPos.cx, wndPos.cy));
                        break;
                    // remove region on maximize or repaint on resize
                    case Win32Messages.WINDOWPOSCHANGED:
                        if (MainForm.WindowState == FormWindowState.Maximized)
                            MainForm.Region = null;

                        var wndPos2 = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));
                        if ((wndPos2.flags & (int)SWPFlags.SWP_NOSIZE) == 0)
                        {
                            UpdateCaption();
                            NcPaint(true);
                        }
                        break;

                    #endregion

                    #region Non Client Area Handling

                    // paint the non client area
                    case Win32Messages.NCPAINT:
                        if (NcPaint(true))
                        {
                            m.Result = (IntPtr)1;
                            supressOriginalMessage = true;
                        }
                        break;
                    // calculate the non client area size
                    case Win32Messages.NCCALCSIZE:
                        //supressOriginalMessage = true;
                        if (m.WParam == (IntPtr)1)
                        {
                            if (MainForm.MdiParent != null)
                                break;
                            // add caption height to non client area
                            var p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));

                            //_a.Height = (int)_skinManager.CurrentSkin
                            //    .CaptionHeight; // FormExtenders.GetCaptionHeight(_parentForm);
                            //_a.Height = (int) MainForm.GetCaptionHeight();
                            _a.Height = 30;

                            p.rect0.Top += _a.Height + _skinManager.CurrentSkin.WindowBorderSize.Top;
                            p.rect0.Left += _skinManager.CurrentSkin.WindowBorderSize.Left;
                            p.rect0.Right -= _skinManager.CurrentSkin.WindowBorderSize.Right;
                            p.rect0.Bottom -= _skinManager.CurrentSkin.WindowBorderSize.Bottom;

                            Marshal.StructureToPtr(p, m.LParam, true);
                        }
                        else
                        {
                            var r = (RECT)m.GetLParam(typeof(RECT));
                        }
                        m.Result = new IntPtr(1);
                        return;
                        break;
                    // non client hit test
                    case Win32Messages.NCHITTEST:
                        //if (NcHitTest(ref m))
                        //    supressOriginalMessage = true;
                        break;

                    #endregion

                    case Win32Messages.NCACTIVATE:
                        if (MainForm.FormBorderStyle == FormBorderStyle.Sizable ||
                            MainForm.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                        {
                            IsActive = (int)m.WParam == 1;
                            if (NcPaint(true))
                            {
                                supressOriginalMessage = true;
                                m.Result = IntPtr.Zero;
                            }
                        }
                        break;
                }

            if (!supressOriginalMessage)
                base.WndProc(ref m);
        }
        /// <summary>
        ///     Handles the window sizing
        /// </summary>
        /// <param name="m">The m.</param>
        private void OnSize(Message m)
        {
            UpdateCaption();
            // update form styles on maximize/restore
            if (MainForm.MdiParent != null)
            {
                if ((int)m.WParam == 0)
                    UpdateStyle();
                if ((int)m.WParam == 2)
                    MainForm.Refresh();
            }

            // update region if needed
            var wasMaxMin = MainForm.WindowState == FormWindowState.Maximized ||
                            MainForm.WindowState == FormWindowState.Minimized;

            var rect1 = new RECT();
            Win32Api.GetWindowRect(MainForm.Handle, ref rect1);

            var rc = new Rectangle(rect1.Left, rect1.Top, rect1.Right - rect1.Left, rect1.Bottom - rect1.Top - 1);

            if (wasMaxMin && MainForm.WindowState == FormWindowState.Normal &&
                rc.Size == MainForm.RestoreBounds.Size)
            {
                _skinManager.SetRegion(this,
                    new Size(rect1.Right - rect1.Left, rect1.Bottom - rect1.Top));
                NcPaint(true);
            }

            /*
            if (MainForm.WindowState == FormWindowState.Normal)
            {
                if (_shadowForm != null)
                    _shadowForm.Visible = true;
            }*/
        }
        /// <summary>
        ///     Ensure that the window doesn't overlap docked toolbars on desktop (like taskbar)
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns></returns>
        private bool CalculateMaxSize(ref Message m)
        {
            if (MainForm.Parent == null)
            {
                // create minMax info for maximize data
                var info = (MINMAXINFO)m.GetLParam(typeof(MINMAXINFO));
                var rect = Screen.FromHandle(Handle).WorkingArea;
                rect.Offset(-rect.X, -rect.Y);

                var fullBorderSize = new Size(SystemInformation.Border3DSize.Width + SystemInformation.BorderSize.Width,
                    SystemInformation.Border3DSize.Height + SystemInformation.BorderSize.Height);

                info.ptMaxPosition.x = rect.Left - fullBorderSize.Width;
                info.ptMaxPosition.y = rect.Top - fullBorderSize.Height;
                info.ptMaxSize.x = rect.Width + fullBorderSize.Width * 2;
                info.ptMaxSize.y = rect.Height + fullBorderSize.Height * 2;

                info.ptMinTrackSize.y += MainForm.GetCaptionHeight();


                if (!MainForm.MaximumSize.IsEmpty)
                {
                    info.ptMaxSize.x = Math.Min(info.ptMaxSize.x, MainForm.MaximumSize.Width);
                    info.ptMaxSize.y = Math.Min(info.ptMaxSize.y, MainForm.MaximumSize.Height);
                    info.ptMaxTrackSize.x = Math.Min(info.ptMaxTrackSize.x, MainForm.MaximumSize.Width);
                    info.ptMaxTrackSize.y = Math.Min(info.ptMaxTrackSize.y, MainForm.MaximumSize.Height);
                }

                if (!MainForm.MinimumSize.IsEmpty)
                {
                    info.ptMinTrackSize.x = Math.Max(info.ptMinTrackSize.x, MainForm.MinimumSize.Width);
                    info.ptMinTrackSize.y = Math.Max(info.ptMinTrackSize.y, MainForm.MinimumSize.Height);
                }

                // set wished maximize size
                Marshal.StructureToPtr(info, m.LParam, true);

                m.Result = (IntPtr)0;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Updates the caption.
        /// </summary>
        private void UpdateCaption()
        {
            // create buttons
            /*if (_captionButtons.Count == 0)
            {
                _captionButtons.Add(new CaptionButton(HitTest.HTCLOSE));
                if (FormExtenders.IsDrawMaximizeBox(_parentForm))
                {
                    var button = new CaptionButton(HitTest.HTMAXBUTTON);
                    _captionButtons.Add(button);
                }
                if (FormExtenders.IsDrawMinimizeBox(_parentForm))
                {
                    var button = new CaptionButton(HitTest.HTMINBUTTON);
                    _captionButtons.Add(button);
                }

                // add command handlers
                foreach (var button in _captionButtons)
                    button.PropertyChanged += OnCommandButtonPropertyChanged;
            }*/

            // Calculate Caption Button Bounds
            var rectScreen = new RECT();
            Win32Api.GetWindowRect(MainForm.Handle, ref rectScreen);
            var rect = rectScreen.ToRectangle();

            var borderSize = MainForm.GetBorderSize();
            rect.Offset(-rect.Left, -rect.Top);
            //rect.Width -= 4;
            //rect.Height += 4;

            var captionButtonSize = MainForm.GetCaptionButtonSize();
            var buttonRect = new Rectangle(rect.Right - borderSize.Width - captionButtonSize.Width,
                rect.Top + borderSize.Height,
                captionButtonSize.Width, captionButtonSize.Height);

            /* foreach (var button in _captionButtons)
             {
                 button.Bounds = buttonRect;
                 buttonRect.X -= captionButtonSize.Width;
             }*/
        }


        /// <summary>
        ///     Redraws the non client area..
        /// </summary>
        public void Invalidate()
        {
            if (IsProcessNcArea)
                NcPaint(true);
        }
    }
}
