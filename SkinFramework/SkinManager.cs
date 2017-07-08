using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkinFramework.Drawing;
using SkinFramework.Native;
using SkinFramework.Presets;

namespace SkinFramework
{
    public partial class SkinManager : Component
    {
        private Skin _currentSkin;
        private SkinDefinition _skinDefinition;

        private DefaultSkin _defaultSkin;
        private SkinWindow _window;
        private Form _mainForm;
        /// <summary>
        ///     Gets the current skin.
        /// </summary>
        /// <value>The current skin.</value>
        public SkinDefinition CurrentSkin => _skinDefinition ?? LoadDefaultSkin();
        /// <summary>
        ///     Gets or sets the parent form which should be skinned.
        /// </summary>
        /// <value>The parent form.</value>
        [Category("Behavior")]
        [Description("Gets or sets the parent form which should be skinned")]
        public Form ParentForm
        {
            get => _mainForm;
            set
            {
                if (_mainForm == value) return;
                if (_mainForm != null && !DesignMode)
                    _mainForm.Disposed -= OnParentFormDisposed;
                _mainForm = value;
                // Start skinning 
                if (_mainForm != null && !DesignMode)
                {
                    _window = new SkinWindow(_mainForm, this);
                    _mainForm.Disposed += OnParentFormDisposed;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the default skin to load on startup.
        /// </summary>
        /// <value>The current default style.</value>
        [Category("Appearance")]
        [Description("Gets or sets the default skin to load on startup")]
        public DefaultSkin DefaultSkin
        {
            get => _defaultSkin;
            set
            {
                if (_defaultSkin == value) return;
                _defaultSkin = value;
                LoadDefaultSkin(_defaultSkin);
                if (_window != null)
                    _window.Invalidate();
            }
        }

        public SkinManager()
        {
            InitializeComponent();
        }

        public SkinManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        private void OnParentFormDisposed(object sender, EventArgs e)
        {
            Dispose();
        }


        #region DefaultSkin Loaders

        /// <summary>
        ///     Loads a default skin.
        /// </summary>
        /// <param name="skin">The skin to load.</param>
        public void LoadDefaultSkin(DefaultSkin skin)
        {
            // Dont load the skin imemdialy. Wait for first access.
            // This allows a custom skin to be loaded before the default skin
            _currentSkin = null;
            _defaultSkin = skin;
        }

        /// <summary>
        ///     Loads a skin implementation
        /// </summary>
        /// <param name="skin">The skin.</param>
        public void LoadSkin(Skin skin)
        {
            _currentSkin = skin;
            _skinDefinition = _currentSkin.Load();
        }

        /// <summary>
        ///     Loads the default skin.
        /// </summary>
        /// <returns></returns>
        private SkinDefinition LoadDefaultSkin()
        {

            // skin implementation
            switch (_defaultSkin)
            {

            }

            _currentSkin = new DefaultSkin();
            // load skin
            LoadSkin(_currentSkin);
            return _skinDefinition;
        }

        #endregion

        internal void SetRegion(SkinWindow window, Size newSize)
        {
            // Create a rounded rectangle using Gdi
            var cornerSize = new Size(1, 1);
            //var hRegion = Win32Api.CreateRectRgn(0, 0, newSize.Width + 1, newSize.Height + 1);
            var hRegion = Win32Api.CreateRoundRectRgn(0, 0, newSize.Width + 1, newSize.Height + 1, cornerSize.Width,
                cornerSize.Height);
            var region = Region.FromHrgn(hRegion);
            window.MainForm.Region = region;
            region.ReleaseHrgn(hRegion);

            window.MainForm.AllowTransparency = true;
        }

        internal void PaintSkin(SkinWindowPaintData windowData)
        {
            var painter = new FormPainter(windowData, CurrentSkin);
            painter.Paint();
        }
    }
}
