using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace jamesControls.CustomControls
{
    public enum ProgressEffectStyle
    {
        none,
        wave,
        gradient,
        patterned
    }

    public enum logoEffectStyle
    {
        none,
        rotate,
        swivel,
        snake,
        shake
    }

    [DefaultProperty(nameof(Value))]
    [DefaultEvent("Click")]
    public class jamesProgramBar : Control
    {
        private int minimum = 0;
        private int maximum = 100;
        private int value = 0;

        private Color borderColor = Color.Gray;
        private Color barColor = Color.DodgerBlue;
        private Color backgroundColor = Color.Transparent;
        private Color gradientStartColor = Color.FromArgb(0, 95, 107);
        private Color gradientEndColor = Color.FromArgb(36, 198, 220);
        private Image? progressIcon = null;
        private Image? patternImage = null;
        private int borderRadius = 10;
        private int progressIconSizePercent = 90;

        private bool showInnerShadow = true;
        private Color innerShadowColor = Color.FromArgb(100, 0, 0, 0);
        private ProgressEffectStyle progressEffect = ProgressEffectStyle.none;
        private logoEffectStyle logoEffect = logoEffectStyle.none;

        private System.Windows.Forms.Timer animationTimer;
        private int animationOffset = 0;
        private float logoAnimationAngle = 0;
        private float swivelDirection = 1;
        private float shakeOffset = 0;
        private float snakeOffset = 0;

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color GradientStartColor 
        { 
            get => gradientStartColor;
            set 
            { 
                gradientStartColor = value; Invalidate(); 
            } 
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color GradientEndColor 
        { 
            get => gradientEndColor; 
            set 
            {
                gradientEndColor = value; Invalidate(); 
            } 
        }

        [Category("James Appearance")]
        [DefaultValue(null)]
        public Image? PatternImage 
        { 
            get => patternImage;
            set 
            {
                patternImage = value; Invalidate(); 
            } 
        }

        [Category("James Behaviour")]
        [DefaultValue(0)]
        public int Minimum 
        { 
            get => minimum; 
            set 
            {
                if (value < 0) value = 0;
                if (value > maximum)
                {
                    value = maximum;
                }
                minimum = value;
                if (this.value < minimum)
                {
                    this.value = minimum;
                }
                Invalidate(); 
            } 
        }

        [Category("James Behaviour")]
        [DefaultValue(100)]
        public int Maximum 
        { 
            get => maximum; 
            set 
            {
                if (value < minimum) 
                { 
                    value = minimum; 
                }
                maximum = value;
                if (this.value > maximum)
                {
                    this.value = maximum;
                }
                Invalidate(); 
            } 
        }

        [Category("James Behaviour")]
        [DefaultValue(0)]
        public int Value 
        {
            get => value;
            set 
            {
                if (value < minimum) 
                { 
                    value = minimum; 
                }
                if (value > maximum) 
                { 
                    value = maximum; 
                } 
                this.value = value;
                Invalidate(); 
            }
        }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color BarColor { get => barColor; set { barColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color BackgroundColor { get => backgroundColor; set { backgroundColor = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(null)]
        public Image? ProgressIcon { get => progressIcon; set { progressIcon = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(10)]
        public int BorderRadius { get => borderRadius; set { borderRadius = Math.Max(0, value); Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(90)]
        public int ProgressIconSizePercent { get => progressIconSizePercent; set { progressIconSizePercent = Math.Max(0, Math.Min(100, value)); Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(true)]
        public bool ShowInnerShadow { get => showInnerShadow; set { showInnerShadow = value; Invalidate(); } }

        [Category("James Appearance")]
        [DefaultValue(typeof(Color), "100, 0, 0, 0")]
        public Color InnerShadowColor { get => innerShadowColor; set { innerShadowColor = value; Invalidate(); } }

        [Category("James Behaviour")]
        [DefaultValue(ProgressEffectStyle.none)]
        public ProgressEffectStyle ProgressEffect { get => progressEffect; set { progressEffect = value; UpdateAnimationTimer(); Invalidate(); } }

        [Category("James Behaviour")]
        [DefaultValue(logoEffectStyle.none)]
        public logoEffectStyle LogoEffect { get => logoEffect; set { logoEffect = value; UpdateAnimationTimer(); Invalidate(); } }

        public jamesProgramBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(200, 23);
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16;
            animationTimer.Tick += (s, e) =>
            {
                animationOffset = (animationOffset + 4) % (Width + 1);
                switch (logoEffect)
                {
                    case logoEffectStyle.rotate:
                        logoAnimationAngle = (logoAnimationAngle + 5f) % 360f;
                        break;
                    case logoEffectStyle.swivel:
                        logoAnimationAngle += 2.5f * swivelDirection;
                        if (logoAnimationAngle >= 20f || logoAnimationAngle <= -20f)
                            swivelDirection *= -1;
                        break;
                    case logoEffectStyle.shake:
                        shakeOffset = (shakeOffset == 0) ? 2 : 0;
                        break;
                    case logoEffectStyle.snake:
                        snakeOffset += 0.1f;
                        if (snakeOffset > 2 * Math.PI) snakeOffset = 0;
                        break;
                }
                Invalidate();
            };
        }

        private void UpdateAnimationTimer()
        {
            if (progressEffect != ProgressEffectStyle.none || logoEffect != logoEffectStyle.none)
            {
                animationTimer.Start();
            }
            else
            {
                animationTimer.Stop();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, gradientStartColor, gradientEndColor, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float percent = (maximum > minimum) ? (float)(value - minimum) / (maximum - minimum) : 0;
            percent = Math.Min(1f, Math.Max(0f, percent));

            int barWidth = (int)(Width * 0.81f);
            int barHeight = (int)(Height * 0.315f);
            int barX = (Width - barWidth) / 2;
            int barY = (Height - barHeight) / 2;
            Rectangle barRect = new Rectangle(barX, barY, barWidth, barHeight);
            int progressWidth = (int)Math.Min(barWidth * percent, barWidth);

            using (GraphicsPath backgroundPath = GetRoundedRectPath(barRect, borderRadius))
            using (Brush backBrush = new SolidBrush(Color.FromArgb(100, Color.White)))
                e.Graphics.FillPath(backBrush, backgroundPath);

            if (showInnerShadow)
            {
                int shadowHeight = Math.Max(6, (int)(barHeight * 0.33f));
                int sideShadowWidth = Math.Max(4, (int)(barWidth * 0.08f));

                using (GraphicsPath barPath = GetRoundedRectPath(barRect, borderRadius))
                {
                    Region oldClip = e.Graphics.Clip;
                    e.Graphics.SetClip(barPath);

                    using (LinearGradientBrush topShadowBrush = new LinearGradientBrush(new Rectangle(barX, barY, barWidth, shadowHeight), Color.FromArgb(160, innerShadowColor), Color.Transparent, LinearGradientMode.Vertical))
                        e.Graphics.FillRectangle(topShadowBrush, barX, barY, barWidth, shadowHeight);

                    using (LinearGradientBrush leftShadowBrush = new LinearGradientBrush(new Rectangle(barX, barY, sideShadowWidth, barHeight), Color.FromArgb(90, innerShadowColor), Color.Transparent, LinearGradientMode.Horizontal))
                        e.Graphics.FillRectangle(leftShadowBrush, barX, barY, sideShadowWidth, barHeight);

                    using (LinearGradientBrush rightShadowBrush = new LinearGradientBrush(new Rectangle(barX + barWidth - sideShadowWidth, barY, sideShadowWidth, barHeight), Color.Transparent, Color.FromArgb(90, innerShadowColor), LinearGradientMode.Horizontal))
                        e.Graphics.FillRectangle(rightShadowBrush, barX + barWidth - sideShadowWidth, barY, sideShadowWidth, barHeight);

                    e.Graphics.SetClip(oldClip, CombineMode.Replace);
                }
            }

            if (progressWidth > 0)
            {
                Rectangle progressRect = new Rectangle(barX, barY, progressWidth, barHeight);
                using (GraphicsPath clipPath = GetRoundedRectPath(barRect, borderRadius))
                {
                    Region oldClip = e.Graphics.Clip;
                    e.Graphics.SetClip(clipPath);

                    switch (progressEffect)
                    {
                        case ProgressEffectStyle.wave:
                            using (TextureBrush waveBrush = CreateWaveBrush(barColor))
                            {
                                waveBrush.TranslateTransform(animationOffset % 40, 0);
                                e.Graphics.FillRectangle(waveBrush, progressRect);
                            }
                            break;
                        case ProgressEffectStyle.gradient:
                            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(progressRect, gradientStartColor, gradientEndColor, LinearGradientMode.Horizontal))
                                e.Graphics.FillRectangle(gradientBrush, progressRect);
                            break;
                        case ProgressEffectStyle.patterned:
                            if (patternImage != null)
                            {
                                using (TextureBrush patternBrush = new TextureBrush(patternImage))
                                {
                                    patternBrush.WrapMode = WrapMode.Tile;
                                    patternBrush.TranslateTransform(animationOffset % patternImage.Width, 0);
                                    e.Graphics.FillRectangle(patternBrush, progressRect);
                                }
                            }
                            else
                            {
                                using (Brush fallbackBrush = new SolidBrush(barColor))
                                    e.Graphics.FillRectangle(fallbackBrush, progressRect);
                            }
                            break;
                        default:
                            using (Brush solid = new SolidBrush(barColor))
                                e.Graphics.FillRectangle(solid, progressRect);
                            break;
                    }

                    e.Graphics.SetClip(oldClip, CombineMode.Replace);
                }
            }

            using (GraphicsPath borderPath = GetRoundedRectPath(barRect, borderRadius))
            using (Pen borderPen = new Pen(borderColor, 1.0f))
            {
                borderPen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(borderPen, borderPath);
            }

            if (progressIcon != null && progressWidth > 0 && progressIconSizePercent > 0)
            {
                int iconSize = (int)(Height * (progressIconSizePercent / 100.0));
                int iconX = barX + progressWidth - iconSize / 2;
                iconX = Math.Max(barX, Math.Min(barX + barWidth - iconSize, iconX));
                int iconY = (Height - iconSize) / 2;

                float adjustedX = iconX + iconSize / 2f;
                float adjustedY = iconY + iconSize / 2f;

                e.Graphics.TranslateTransform(adjustedX, adjustedY);
                switch (logoEffect)
                {
                    case logoEffectStyle.rotate:
                    case logoEffectStyle.swivel:
                        e.Graphics.RotateTransform(logoAnimationAngle);
                        break;
                    case logoEffectStyle.shake:
                        e.Graphics.TranslateTransform(shakeOffset, 0);
                        break;
                    case logoEffectStyle.snake:
                        e.Graphics.TranslateTransform(0, (float)(Math.Sin(snakeOffset) * 3));
                        break;
                }

                e.Graphics.TranslateTransform(-adjustedX, -adjustedY);
                e.Graphics.DrawImage(progressIcon, iconX, iconY, iconSize, iconSize);
                e.Graphics.ResetTransform();
            }
        }

        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius, bool clipRight = false)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }
            if (clipRight)
            {
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Y);
                path.CloseFigure();
            }
            else
            {
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
            }
            return path;
        }

        private TextureBrush CreateWaveBrush(Color baseColor)
        {
            Bitmap waveBitmap = new Bitmap(40, 10);
            using (Graphics g = Graphics.FromImage(waveBitmap))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(ControlPaint.Light(baseColor), 2))
                {
                    for (int i = 0; i < waveBitmap.Width; i += 4)
                    {
                        int y = (int)(5 + Math.Sin((i + animationOffset) * 0.3) * 3);
                        g.DrawLine(pen, i, y, i + 2, y);
                    }
                }
            }
            return new TextureBrush(waveBitmap);
        }
    }
}
