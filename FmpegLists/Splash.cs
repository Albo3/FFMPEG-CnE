using System;
using System.Drawing;
using System.Windows.Forms;

namespace FmpegLists
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            AcceptButton = startButton; // Enter key starts
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var main = new Form1();
            main.FormClosed += (_, __) => this.Close(); // end app when main closes
            main.Show();
        }

        // Optional helpers so you can set an image at runtime if you want:
        public void SetLogo(Image img) => logoPictureBox.Image = img;
        public void LoadLogo(string path)
        {
            if (System.IO.File.Exists(path))
                logoPictureBox.Image = Image.FromFile(path);
        }
    }
}
