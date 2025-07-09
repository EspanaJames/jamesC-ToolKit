namespace jamesControls
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            gradientPanel1 = new jamesControls.CustomControls.jamesPanel();
            jamesButton1 = new jamesControls.CustomControls.jamesButton();
            jamesLabel1 = new jamesControls.CustomControls.jamesLabel();
            logoProgramBar1 = new jamesControls.CustomControls.jamesProgramBar();
            gradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // gradientPanel1
            // 
            gradientPanel1.Controls.Add(jamesButton1);
            gradientPanel1.Controls.Add(jamesLabel1);
            gradientPanel1.Controls.Add(logoProgramBar1);
            gradientPanel1.Dock = DockStyle.Fill;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(853, 463);
            gradientPanel1.TabIndex = 1;
            gradientPanel1.Paint += gradientPanel1_Paint;
            // 
            // jamesButton1
            // 
            jamesButton1.BackColor = Color.Transparent;
            jamesButton1.BorderColor = Color.Black;
            jamesButton1.BorderRadius = 15;
            jamesButton1.EnableInnerShadow = false;
            jamesButton1.FlatAppearance.BorderSize = 0;
            jamesButton1.FlatStyle = FlatStyle.Flat;
            jamesButton1.ForeColor = Color.Black;
            jamesButton1.GradientBottom = Color.LightSalmon;
            jamesButton1.GradientTop = SystemColors.WindowFrame;
            jamesButton1.Location = new Point(33, 264);
            jamesButton1.Name = "jamesButton1";
            jamesButton1.Size = new Size(466, 83);
            jamesButton1.TabIndex = 4;
            jamesButton1.Text = "jamesButton1";
            jamesButton1.UseVisualStyleBackColor = false;
            // 
            // jamesLabel1
            // 
            jamesLabel1.AutoSize = true;
            jamesLabel1.BackColor = Color.Transparent;
            jamesLabel1.Font = new Font("Calibri", 32F);
            jamesLabel1.ForeColor = SystemColors.ButtonHighlight;
            jamesLabel1.Glow = true;
            jamesLabel1.GlowColor = Color.FromArgb(128, 255, 255);
            jamesLabel1.GlowSize = 2;
            jamesLabel1.LabelFontFamily = "calibri";
            jamesLabel1.LabelFontSize = 32F;
            jamesLabel1.LetterSpacing = 0.1F;
            jamesLabel1.Location = new Point(159, 90);
            jamesLabel1.Name = "jamesLabel1";
            jamesLabel1.OuterShadow = true;
            jamesLabel1.OuterShadowSize = 6;
            jamesLabel1.ShineAlpha = 1F;
            jamesLabel1.Size = new Size(682, 53);
            jamesLabel1.TabIndex = 3;
            jamesLabel1.Text = "COMPESA ATTENDANCE";
            jamesLabel1.Click += jamesLabel1_Click;
            // 
            // logoProgramBar1
            // 
            logoProgramBar1.BackColor = Color.FromArgb(38, 180, 210);
            logoProgramBar1.BackgroundColor = Color.Ivory;
            logoProgramBar1.BarColor = Color.FromArgb(32, 64, 96);
            logoProgramBar1.BorderColor = Color.Black;
            logoProgramBar1.BorderRadius = 0;
            logoProgramBar1.GradientEndColor = Color.FromArgb(36, 174, 203);
            logoProgramBar1.GradientStartColor = Color.FromArgb(30, 151, 177);
            logoProgramBar1.InnerShadowColor = Color.Black;
            logoProgramBar1.Location = new Point(441, 377);
            logoProgramBar1.LogoEffect = CustomControls.logoEffectStyle.rotate;
            logoProgramBar1.Name = "logoProgramBar1";
            logoProgramBar1.PatternImage = (Image)resources.GetObject("logoProgramBar1.PatternImage");
            logoProgramBar1.ProgressEffect = CustomControls.ProgressEffectStyle.gradient;
            logoProgramBar1.ProgressIcon = (Image)resources.GetObject("logoProgramBar1.ProgressIcon");
            logoProgramBar1.ProgressIconSizePercent = 70;
            logoProgramBar1.Size = new Size(412, 83);
            logoProgramBar1.TabIndex = 0;
            logoProgramBar1.Text = "logoProgramBar1";
            logoProgramBar1.Click += logoProgramBar1_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(853, 463);
            Controls.Add(gradientPanel1);
            FormBorderStyle = FormBorderStyle.None;
            ImeMode = ImeMode.On;
            Name = "Form1";
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private CustomControls.jamesPanel gradientPanel1;
        private CustomControls.jamesProgramBar logoProgramBar1;
        private CustomControls.jamesLabel jamesLabel1;
        private CustomControls.jamesButton jamesButton1;
    }
}
