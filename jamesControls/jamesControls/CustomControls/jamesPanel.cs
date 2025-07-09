using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace jamesControls.CustomControls
{
    [ToolboxItem(true)]
    [DesignerCategory("Code")]
    public class jamesPanel : Panel
    {
        private Color topColor = Color.FromArgb(13, 66, 82);
        private Color middleColor = Color.FromArgb(15, 90, 105);
        private Color bottomColor = Color.FromArgb(38, 180, 210);
        private bool enableGradient = true;

        private Bitmap? cachedBackground = null;
        private Size cachedSize = Size.Empty;
        private int lastHash = 0;

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "13, 66, 82")]
        public Color TopColor
        {
            get => topColor;
            set
            {
                if (topColor != value)
                {
                    topColor = value;
                    InvalidateCache();
                }
            }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "15, 90, 105")]
        public Color MiddleColor
        {
            get => middleColor;
            set
            {
                if (middleColor != value)
                {
                    middleColor = value;
                    InvalidateCache();
                }
            }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "38, 180, 210")]
        public Color BottomColor
        {
            get => bottomColor;
            set
            {
                if (bottomColor != value)
                {
                    bottomColor = value;
                    InvalidateCache();
                }
            }
        }

        [Category("James Appearance")]
        [DefaultValue(true)]
        public bool EnableGradient
        {
            get => enableGradient;
            set
            {
                if (enableGradient != value)
                {
                    enableGradient = value;
                    InvalidateCache();
                }
            }
        }

        public jamesPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Visible || Width <= 0 || Height <= 0)
                return;

            int currentHash = ComputeHash();
            if (cachedBackground == null || cachedSize != ClientSize || currentHash != lastHash)
            {
                CreateBackgroundCache(e.Graphics.DpiX, e.Graphics.DpiY);
                lastHash = currentHash;
            }

            if (cachedBackground != null)
            {
                e.Graphics.DrawImageUnscaled(cachedBackground, 0, 0);
            }

            base.OnPaint(e);
        }

        private void CreateBackgroundCache(float dpiX, float dpiY)
        {
            cachedBackground?.Dispose();
            cachedBackground = new Bitmap(Width, Height);
            cachedBackground.SetResolution(dpiX, dpiY);

            using (Graphics g = Graphics.FromImage(cachedBackground))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle rect = new Rectangle(0, 0, Width, Height);

                if (enableGradient)
                {
                    using (var brush = new LinearGradientBrush(rect, topColor, bottomColor, LinearGradientMode.Vertical))
                    {
                        brush.InterpolationColors = new ColorBlend
                        {
                            Colors = new[] { topColor, middleColor, bottomColor },
                            Positions = new[] { 0f, 0.5f, 1f }
                        };
                        g.FillRectangle(brush, rect);
                    }
                }
                else
                {
                    using (var solid = new SolidBrush(BackColor))
                    {
                        g.FillRectangle(solid, rect);
                    }
                }
            }

            cachedSize = ClientSize;
        }

        private void InvalidateCache()
        {
            cachedBackground?.Dispose();
            cachedBackground = null;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (ClientSize != cachedSize)
            {
                InvalidateCache();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cachedBackground?.Dispose();
                cachedBackground = null;
            }
            base.Dispose(disposing);
        }

        private int ComputeHash()
        {
            unchecked
            {
                int h = 17;
                h = h * 23 + topColor.GetHashCode();
                h = h * 23 + middleColor.GetHashCode();
                h = h * 23 + bottomColor.GetHashCode();
                h = h * 23 + enableGradient.GetHashCode();
                return h;
            }
        }
    }
}
