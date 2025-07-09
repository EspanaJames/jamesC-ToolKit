using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace jamesControls.CustomControls
{
    public class jamesButton : Button
    {
        private int borderRadius = 20;
        private int borderThickness = 2;
        private Color borderColor = Color.DodgerBlue;
        private Color backFillColor = Color.Transparent;
        private bool useGradient = false;
        private Color gradientTop = Color.White;
        private Color gradientBottom = Color.LightGray;
        private bool enableInnerShadow = true;
        private Color innerShadowColor = Color.FromArgb(100, 0, 0, 0);
        private Image iconImage;
        private Size iconSize = new Size(24, 24);
        private int iconTextSpacing = 6;

        [Category("James Appearance")]
        [DefaultValue(20)]
        public int BorderRadius { get => borderRadius; set { borderRadius = Math.Max(0, value); Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(2)]
        public int BorderThickness { get => borderThickness; set { borderThickness = Math.Max(0, value); Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BackFillColor { get => backFillColor; set { backFillColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(false)]
        public bool UseGradient { get => useGradient; set { useGradient = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(true)]
        public bool EnableInnerShadow { get => enableInnerShadow; set { enableInnerShadow = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "100, 0, 0, 0")]
        public Color InnerShadowColor { get => innerShadowColor; set { innerShadowColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public Image IconImage { get => iconImage; set { iconImage = value; Invalidate(); } }

        [Category("James Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size IconSize { get => iconSize; set { iconSize = value; Invalidate(); } }

        [Category("James Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IconTextSpacing { get => iconTextSpacing; set { iconTextSpacing = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color GradientTop { get => gradientTop; set { gradientTop = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color GradientBottom { get => gradientBottom; set { gradientBottom = value; Invalidate(); } }

        public jamesButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;

            using (GraphicsPath path = GetRoundedPath(rect, borderRadius))
            {
                Region = new Region(path);

                if (useGradient)
                {
                    using (LinearGradientBrush b = new LinearGradientBrush(rect, gradientTop, gradientBottom, LinearGradientMode.Vertical))
                        g.FillPath(b, path);
                }
                else if (backFillColor != Color.Transparent)
                {
                    using (SolidBrush b = new SolidBrush(backFillColor))
                        g.FillPath(b, path);
                }

                if (enableInnerShadow)
                {
                    using (GraphicsPath shadowPath = GetRoundedPath(new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2), borderRadius))
                    using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                    {
                        shadowBrush.CenterColor = Color.Transparent;
                        shadowBrush.SurroundColors = new[] { innerShadowColor };
                        g.FillPath(shadowBrush, shadowPath);
                    }
                }

                if (borderThickness > 0)
                {
                    using (Pen pen = new Pen(borderColor, borderThickness))
                        g.DrawPath(pen, path);
                }

                Rectangle contentRect = rect;
                if (iconImage != null)
                {
                    Size imgSize = iconSize;
                    Point imgPoint = new Point(contentRect.X + 10, (contentRect.Height - imgSize.Height) / 2);
                    Rectangle imgRect = new Rectangle(imgPoint, imgSize);
                    g.DrawImage(iconImage, imgRect);

                    int textX = imgRect.Right + iconTextSpacing;
                    Rectangle textRect = new Rectangle(textX, contentRect.Y, contentRect.Width - textX - 10, contentRect.Height);
                    TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
                }
                else
                {
                    TextRenderer.DrawText(g, Text, Font, contentRect, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
