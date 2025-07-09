namespace jamesControls
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer progressTimer;
        public Form1()
        {
            InitializeComponent();
            progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 50; 
            progressTimer.Tick += ProgressTimer_Tick;
            logoProgramBar1.Value = logoProgramBar1.Minimum;
            progressTimer.Start();
        }
        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (logoProgramBar1.Value < logoProgramBar1.Maximum)
            {
                logoProgramBar1.Value += 1;
            }
            else
            {
                progressTimer.Stop();
            }
        }


        private void logoProgramBar1_Click(object sender, EventArgs e)
        {

        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void logoProgramBar1_Click_1(object sender, EventArgs e)
        {

        }

        private void jamesLabel1_Click(object sender, EventArgs e)
        {

        }

        private void jamesLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
