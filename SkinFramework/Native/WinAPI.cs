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
/*
 * The Enumerations in this File are stripped down to the values needed in this lib
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace SkinFramework.Native
{
    /// <summary>
    ///     This class contains some win32 functions needed for nc drawing
    /// </summary>
    internal static class Win32Api
    {
        #region Shell32.dll

        [DllImport("Shell32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int SHAppBarMessage(int dwMessage, IntPtr pData);

        #endregion

        #region User32.dll

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
            IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, GWLIndex nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, GWLIndex nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);


        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DCXFlags flags);

        #endregion

        #region Gdi32.dll

        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2,
            int cx, int cy);

        #endregion
    }

    #region Enums

    public enum ABMsg
    {
        ABM_NEW = 0,
        ABM_REMOVE = 1,
        ABM_QUERYPOS = 2,
        ABM_SETPOS = 3,
        ABM_GETSTATE = 4,
        ABM_GETTASKBARPOS = 5,
        ABM_ACTIVATE = 6,
        ABM_GETAUTOHIDEBAR = 7,
        ABM_SETAUTOHIDEBAR = 8,
        ABM_WINDOWPOSCHANGED = 9,
        ABM_SETSTATE = 10
    }

    public enum ABEdge
    {
        ABE_LEFT = 0,
        ABE_TOP = 1,
        ABE_RIGHT = 2,
        ABE_BOTTOM = 3
    }

    public enum Bool
    {
        False = 0,
        True
    }

    /*public enum WindowStyles : uint
    {
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,

    }*/

    [Flags]
    public enum WindowStyles : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,

        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,

        WS_CAPTION = WS_BORDER | WS_DLGFRAME,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_CHILDWINDOW = WS_CHILD,


        /// <summary>Specifies a window that accepts drag-drop files.</summary>
        WS_EX_ACCEPTFILES = 0x00000010,

        /// <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
        WS_EX_APPWINDOW = 0x00040000,

        /// <summary>Specifies a window that has a border with a sunken edge.</summary>
        WS_EX_CLIENTEDGE = 0x00000200,

        /// <summary>
        /// Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
        /// </summary>
        /// <remarks>
        /// With WS_EX_COMPOSITED set, all descendants of a window get bottom-to-top painting order using double-buffering.
        /// Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        /// but only if the descendent window also has the WS_EX_TRANSPARENT bit set.
        /// Double-buffering allows the window and its descendents to be painted without flicker.
        /// </remarks>
        WS_EX_COMPOSITED = 0x02000000,

        /// <summary>
        /// Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
        /// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
        /// The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
        /// The Help application displays a pop-up window that typically contains help for the child window.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        WS_EX_CONTEXTHELP = 0x00000400,

        /// <summary>
        /// Specifies a window which contains child windows that should take part in dialog box navigation.
        /// If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
        /// such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        WS_EX_CONTROLPARENT = 0x00010000,

        /// <summary>Specifies a window that has a double border.</summary>
        WS_EX_DLGMODALFRAME = 0x00000001,

        /// <summary>
        /// Specifies a window that is a layered window.
        /// This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// </summary>
        WS_EX_LAYERED = 0x00080000,

        /// <summary>
        /// Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_LAYOUTRTL = 0x00400000,

        /// <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
        WS_EX_LEFT = 0x00000000,

        /// <summary>
        /// Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_LEFTSCROLLBAR = 0x00004000,

        /// <summary>
        /// Specifies a window that displays text using left-to-right reading-order properties. This is the default.
        /// </summary>
        WS_EX_LTRREADING = 0x00000000,

        /// <summary>
        /// Specifies a multiple-document interface (MDI) child window.
        /// </summary>
        WS_EX_MDICHILD = 0x00000040,

        /// <summary>
        /// Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
        /// The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        /// </summary>
        WS_EX_NOACTIVATE = 0x08000000,

        /// <summary>
        /// Specifies a window which does not pass its window layout to its child windows.
        /// </summary>
        WS_EX_NOINHERITLAYOUT = 0x00100000,

        /// <summary>
        /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        /// </summary>
        WS_EX_NOPARENTNOTIFY = 0x00000004,

        /// <summary>Specifies an overlapped window.</summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,

        /// <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,

        /// <summary>
        /// Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        /// The shell language must support reading-order alignment for this to take effect.
        /// Using the WS_EX_RIGHT style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
        /// </summary>
        WS_EX_RIGHT = 0x00001000,

        /// <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
        WS_EX_RIGHTSCROLLBAR = 0x00000000,

        /// <summary>
        /// Specifies a window that displays text using right-to-left reading-order properties.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_RTLREADING = 0x00002000,

        /// <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
        WS_EX_STATICEDGE = 0x00020000,

        /// <summary>
        /// Specifies a window that is intended to be used as a floating toolbar.
        /// A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
        /// A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
        /// If a tool window has a system menu, its icon is not displayed on the title bar.
        /// However, you can display the system menu by right-clicking or by typing ALT+SPACE. 
        /// </summary>
        WS_EX_TOOLWINDOW = 0x00000080,

        /// <summary>
        /// Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        /// To add or remove this style, use the SetWindowPos function.
        /// </summary>
        WS_EX_TOPMOST = 0x00000008,

        /// <summary>
        /// Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        /// The window appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        WS_EX_TRANSPARENT = 0x00000020,

        /// <summary>Specifies a window that has a border with a raised edge.</summary>
        WS_EX_WINDOWEDGE = 0x00000100

    }

    //[Flags]
    //public enum DCXFlags : long
    //{
    //    DCXWindow = 0x00000001L,
    //    DCXCache = 0x00000002L,
    //    DCXClipsiblings = 0x00000010L,
    //}

    /// <summary>Values to pass to the GetDCEx method.</summary>
    [Flags]
    public enum DCXFlags : uint
    {
        /// <summary>
        ///     DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather
        ///     than the client rectangle.
        /// </summary>
        Window = 0x00000001,

        /// <summary>
        ///     DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC
        ///     window. Essentially overrides CS_OWNDC and CS_CLASSDC.
        /// </summary>
        Cache = 0x00000002,

        /// <summary>
        ///     DCX_NORESETATTRS: Does not reset the attributes of this DC to the
        ///     default attributes when this DC is released.
        /// </summary>
        NoResetAttrs = 0x00000004,

        /// <summary>
        ///     DCX_CLIPCHILDREN: Excludes the visible regions of all child windows
        ///     below the window identified by hWnd.
        /// </summary>
        ClipChildren = 0x00000008,

        /// <summary>
        ///     DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows
        ///     above the window identified by hWnd.
        /// </summary>
        ClipSiblings = 0x00000010,

        /// <summary>
        ///     DCX_PARENTCLIP: Uses the visible region of the parent window. The
        ///     parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is
        ///     set to the upper-left corner of the window identified by hWnd.
        /// </summary>
        ParentClip = 0x00000020,

        /// <summary>
        ///     DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded
        ///     from the visible region of the returned DC.
        /// </summary>
        ExcludeRgn = 0x00000040,

        /// <summary>
        ///     DCX_INTERSECTRGN: The clipping region identified by hrgnClip is
        ///     intersected with the visible region of the returned DC.
        /// </summary>
        IntersectRgn = 0x00000080,

        /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
        ExcludeUpdate = 0x00000100,

        /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
        IntersectUpdate = 0x00000200,

        /// <summary>
        ///     DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate
        ///     call in effect that would otherwise exclude this window. Used for drawing during
        ///     tracking.
        /// </summary>
        LockWindowUpdate = 0x00000400,

        /// <summary>DCX_USESTYLE: Undocumented, something related to WM_NCPAINT message.</summary>
        UseStyle = 0x00010000,

        /// <summary>
        ///     DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to
        ///     be completely validated. Using this function with both DCX_INTERSECTUPDATE and
        ///     DCX_VALIDATE is identical to using the BeginPaint function.
        /// </summary>
        Validate = 0x00200000
    }


    public enum HitTest
    {
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTCLOSE = 20
    }

    public enum RedrawWindowFlags
    {
        RDW_INVALIDATE = 0x0001,
        RDW_ALLCHILDREN = 0x0080,
        RDW_UPDATENOW = 0x0100,
        RDW_ERASENOW = 0x0200,
        RDW_FRAME = 0x0400
    }

    public enum WAFlags
    {
        WA_ACTIVE = 1,
        WA_CLICKACTIVE = 2
    }

    public enum GWLIndex
    {
        GWL_STYLE = -16
    }

    [Flags]
    public enum SWPFlags
    {
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOREDRAW = 0x0008,
        SWP_NOACTIVATE = 0x0010,
        SWP_FRAMECHANGED = 0x0020
    }

    /// <summary>
    ///     The SysCommands which can be executed by a form button.
    /// </summary>
    public enum SysCommand
    {
        /// <summary>
        ///     If no SysCommands should be executed
        /// </summary>
        SC_NONE = 0x0,

        /// <summary>
        ///     If the Form should be closed.
        /// </summary>
        SC_CLOSE = 0xf060,

        /// <summary>
        ///     If the Form should be maximized
        /// </summary>
        SC_MAXIMIZE = 0xf030,

        /// <summary>
        ///     If the Form should be minimized
        /// </summary>
        SC_MINIMIZE = 0xf020,

        /// <summary>
        ///     If the form should be restored from the maximize mode.
        /// </summary>
        SC_RESTORE = 0xf120
    }

    /// <summary>
    ///     Windows Messages
    ///     Defined in winuser.h from Windows SDK v6.1
    ///     Documentation pulled from MSDN.
    /// </summary>
    public enum Win32Messages : uint
    {
        /// <summary>
        ///     The WM_SIZE message is sent to a window after its size has changed.
        /// </summary>
        SIZE = 0x0005,

        /// <summary>
        ///     The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows
        ///     use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window
        ///     being deactivated, then to the window procedure of the top-level window being activated. If the windows use
        ///     different input queues, the message is sent asynchronously, so the window is activated immediately.
        /// </summary>
        ACTIVATE = 0x0006,

        /// <summary>
        ///     The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
        /// </summary>
        SHOWWINDOW = 0x0018,

        /// <summary>
        ///     The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is
        ///     about to be activated. The message is sent to the application whose window is being activated and to the
        ///     application whose window is being deactivated.
        /// </summary>
        ACTIVATEAPP = 0x001C,
        GETMINMAXINFO = 0x0024,

        /// <summary>
        ///     The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to
        ///     change as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WINDOWPOSCHANGING = 0x0046,

        /// <summary>
        ///     The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a
        ///     result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WINDOWPOSCHANGED = 0x0047,

        /// <summary>
        ///     The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the
        ///     window's styles
        /// </summary>
        STYLECHANGED = 0x007D,

        /// <summary>
        ///     The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By
        ///     processing this message, an application can control the content of the window's client area when the size or
        ///     position of the window changes.
        /// </summary>
        NCCALCSIZE = 0x0083,

        /// <summary>
        ///     The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released.
        ///     If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent
        ///     to the window that has captured the mouse.
        /// </summary>
        NCHITTEST = 0x0084,

        /// <summary>
        ///     The WM_NCPAINT message is sent to a window when its frame must be painted.
        /// </summary>
        NCPAINT = 0x0085,

        /// <summary>
        ///     The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or
        ///     inactive state.
        /// </summary>
        NCACTIVATE = 0x0086,

        /// <summary>
        ///     The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window.
        ///     This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is
        ///     not posted.
        /// </summary>
        NCMOUSEMOVE = 0x00A0,

        /// <summary>
        ///     The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the
        ///     nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured
        ///     the mouse, this message is not posted.
        /// </summary>
        NCLBUTTONDOWN = 0x00A1,

        /// <summary>
        ///     The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the
        ///     nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured
        ///     the mouse, this message is not posted.
        /// </summary>
        NCLBUTTONUP = 0x00A2,

        /// <summary>
        ///     A window receives this message when the user chooses a command from the Window menu, clicks the maximize button,
        ///     minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering
        ///     this out.
        /// </summary>
        SYSCOMMAND = 0x0112,

        /// <summary>
        ///     The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client
        ///     area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise,
        ///     the message is posted to the window that has captured the mouse.
        /// </summary>
        LBUTTONDOWN = 0x0201,

        /// <summary>
        ///     An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct
        ///     the client window to activate a different MDI child window.
        /// </summary>
        MDIACTIVATE = 0x0222,

        /// <summary>
        ///     The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a
        ///     prior call to TrackMouseEvent.
        /// </summary>
        MOUSELEAVE = 0x02A3,

        /// <summary>
        ///     The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified
        ///     in a prior call to TrackMouseEvent.
        /// </summary>
        NCMOUSELEAVE = 0x02A2,

        MOUSEHOVER = 0x2A1
    }

    #endregion

    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public Int32 cx;
        public Int32 cy;

        public SIZE(Int32 cx, Int32 cy) { this.cx = cx; this.cy = cy; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public Int32 x;
        public Int32 y;

        public POINT(Int32 x, Int32 y) { this.x = x; this.y = y; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public int uCallbackMessage;
        public int uEdge;
        public RECT rc;
        public IntPtr lParam;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rect0;
        public RECT rect1;
        public RECT rect2;
        public WINDOWPOS IntPtr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(Rectangle Rectangle)
            : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        {
        }

        public RECT(int Left, int Top, int Right, int Bottom)
        {
            X = Left;
            Y = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Left
        {
            get => X;
            set => X = value;
        }

        public int Top
        {
            get => Y;
            set => Y = value;
        }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Height
        {
            get => Bottom - Y;
            set => Bottom = value - Y;
        }

        public int Width
        {
            get => Right - X;
            set => Right = value + X;
        }

        public Point Location
        {
            get => new Point(Left, Top);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Right = value.Height + X;
                Bottom = value.Height + Y;
            }
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Width, Height);
        }

        public static Rectangle ToRectangle(RECT Rectangle)
        {
            return Rectangle.ToRectangle();
        }

        public static RECT FromRectangle(Rectangle Rectangle)
        {
            return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        }

        public static implicit operator Rectangle(RECT Rectangle)
        {
            return Rectangle.ToRectangle();
        }

        public static implicit operator RECT(Rectangle Rectangle)
        {
            return new RECT(Rectangle);
        }

        public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
        {
            return Rectangle1.Equals(Rectangle2);
        }

        public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
        {
            return !Rectangle1.Equals(Rectangle2);
        }

        public override string ToString()
        {
            return "{Left: " + X + "; " + "Top: " + Y + "; Right: " + Right + "; Bottom: " + Bottom + "}";
        }

        public bool Equals(RECT Rectangle)
        {
            return Rectangle.Left == X && Rectangle.Top == Y && Rectangle.Right == Right && Rectangle.Bottom == Bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is RECT)
                return Equals((RECT)Object);
            if (Object is Rectangle)
                return Equals(new RECT((Rectangle)Object));

            return false;
        }
    }

    #endregion
}