using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace jamesControls.CustomControls
{
    [ToolboxItem(true)]
    public class jamesLabel : Label
    {
        private bool innerShadow = true;
        private Color innerShadowColor = Color.FromArgb(100, 0, 0, 0);
        private int innerShadowDepth = 2;
        private bool outerShadow = false;
        private Color outerShadowColor = Color.FromArgb(80, 0, 0, 0);
        private int outerShadowSize = 4;
        private bool shine = true;
        private float shineAlpha = 0.5f;
        private bool glow = false;
        private Color glowColor = Color.Cyan;
        private int glowSize = 8;
        private float letterSpacing = 0f;
        private FontStyle fontStyle = FontStyle.Regular;
        private string fontFamily = "Segoe UI";
        private float fontSize = 9f;

        [Category("James Appearance")]
        [DefaultValue(true)]
        public bool InnerShadow
        {
            get => innerShadow;
            set { innerShadow = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "100, 0, 0, 0")]
        public Color InnerShadowColor
        {
            get => innerShadowColor;
            set { innerShadowColor = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(2)]
        public int InnerShadowDepth
        {
            get => innerShadowDepth;
            set { innerShadowDepth = Math.Max(1, value); Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(false)]
        public bool OuterShadow
        {
            get => outerShadow;
            set { outerShadow = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "80, 0, 0, 0")]
        public Color OuterShadowColor
        {
            get => outerShadowColor;
            set { outerShadowColor = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(4)]
        public int OuterShadowSize
        {
            get => outerShadowSize;
            set { outerShadowSize = Math.Max(1, value); Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(true)]
        public bool Shine
        {
            get => shine;
            set { shine = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(0.5f)]
        public float ShineAlpha
        {
            get => shineAlpha;
            set { shineAlpha = Math.Max(0, Math.Min(1, value)); Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(false)]
        public bool Glow
        {
            get => glow;
            set { glow = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "Cyan")]
        public Color GlowColor
        {
            get => glowColor;
            set { glowColor = value; Invalidate(); }
        }

        [Category("James Appearance")]
        [DefaultValue(8)]
        public int GlowSize
        {
            get => glowSize;
            set { glowSize = Math.Max(1, value); Invalidate(); }
        }

        [Category("James Font")]
        [DefaultValue(FontStyle.Regular)]
        public FontStyle LabelFontStyle
        {
            get => fontStyle;
            set { fontStyle = value; UpdateFont(); }
        }

        [Category("James Font")]
        [DefaultValue("Segoe UI")]
        public string LabelFontFamily
        {
            get => fontFamily;
            set { fontFamily = value; UpdateFont(); }
        }

        [Category("James Font")]
        [DefaultValue(9f)]
        public float LabelFontSize
        {
            get => fontSize;
            set { fontSize = Math.Max(1, value); UpdateFont(); }
        }

        [Category("James Font")]
        [DefaultValue(0f)]
        public float LetterSpacing
        {
            get => letterSpacing;
            set { letterSpacing = Math.Max(-10f, Math.Min(50f, value)); Invalidate(); }
        }

        public jamesLabel()
        {
            DoubleBuffered = true;
            AutoSize = true;
            UpdateFont();
        }

        private void UpdateFont()
        {
            Font = new Font(fontFamily, fontSize, fontStyle);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Rectangle textRect = ClientRectangle;
            string text = Text;

            using (GraphicsPath path = GetTextPath(text, Font, textRect, TextAlign, e.Graphics, letterSpacing))
            {
                if (outerShadow)
                {
                    using (Pen shadowPen = new Pen(outerShadowColor, outerShadowSize) { LineJoin = LineJoin.Round })
                    {
                        e.Graphics.DrawPath(shadowPen, path);
                    }
                }

                if (glow)
                {
                    for (int i = glowSize; i > 0; i--)
                    {
                        using (Pen glowPen = new Pen(Color.FromArgb((int)(255 * (0.2 + 0.8 * (1 - i / (float)glowSize))), glowColor), i))
                        {
                            glowPen.LineJoin = LineJoin.Round;
                            e.Graphics.DrawPath(glowPen, path);
                        }
                    }
                }

                if (innerShadow)
                {
                    using (PathGradientBrush shadowBrush = new PathGradientBrush(path))
                    {
                        shadowBrush.CenterColor = Color.Transparent;
                        shadowBrush.SurroundColors = new[] { innerShadowColor };
                        Matrix m = new Matrix();
                        m.Translate(innerShadowDepth, innerShadowDepth);
                        path.Transform(m);
                        e.Graphics.FillPath(shadowBrush, path);
                    }
                }

                if (shine)
                {
                    using (LinearGradientBrush shineBrush = new LinearGradientBrush(
                        textRect,
                        Color.FromArgb((int)(255 * shineAlpha), Color.White),
                        Color.FromArgb(0, Color.White),
                        LinearGradientMode.Vertical))
                    {
                        Region oldClip = e.Graphics.Clip;
                        e.Graphics.SetClip(path, CombineMode.Intersect);
                        e.Graphics.FillRectangle(shineBrush, textRect);
                        e.Graphics.SetClip(oldClip, CombineMode.Replace);
                    }
                }

                using (SolidBrush textBrush = new SolidBrush(ForeColor))
                {
                    e.Graphics.FillPath(textBrush, path);
                }
            }
        }

        private GraphicsPath GetTextPath(string text, Font font, Rectangle rect, ContentAlignment align, Graphics g, float spacing)
        {
            StringFormat format = new StringFormat();
            switch (align)
            {
                case ContentAlignment.TopLeft: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.TopCenter: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.TopRight: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Near; break;
                case ContentAlignment.MiddleLeft: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.MiddleCenter: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.MiddleRight: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Center; break;
                case ContentAlignment.BottomLeft: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Far; break;
                case ContentAlignment.BottomCenter: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Far; break;
                case ContentAlignment.BottomRight: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Far; break;
            }

            GraphicsPath path = new GraphicsPath();
            if (spacing == 0f)
            {
                path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, rect, format);
            }
            else
            {
                float x = rect.Left;
                float y = rect.Top + (rect.Height - font.Height) / 2f;
                foreach (char c in text)
                {
                    string s = c.ToString();
                    SizeF charSize = g.MeasureString(s, font);
                    path.AddString(s, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, new RectangleF(x, y, charSize.Width, charSize.Height), format);
                    x += charSize.Width + spacing;
                }
            }
            return path;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            using (var g = CreateGraphics())
            {
                if (letterSpacing == 0f)
                {
                    var size = TextRenderer.MeasureText(g, Text, Font, proposedSize, TextFormatFlags.SingleLine);
                    return size;
                }
                else
                {
                    float width = 0;
                    foreach (char c in Text)
                    {
                        width += g.MeasureString(c.ToString(), Font).Width + letterSpacing;
                    }
                    return new Size((int)Math.Ceiling(width), Font.Height);
                }
            }
        }
    }
}
