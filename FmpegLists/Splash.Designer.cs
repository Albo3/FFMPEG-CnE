namespace FmpegLists
{
    partial class SplashForm
    {
        private global::System.ComponentModel.IContainer components = null;
        private global::System.Windows.Forms.Button startButton;
        private global::System.Windows.Forms.Label titleLabel;
        private global::System.Windows.Forms.PictureBox logoPictureBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            startButton = new Button();
            titleLabel = new Label();
            logoPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.FlatStyle = FlatStyle.System;
            startButton.Font = new Font("Helvetica Neue", 13F, FontStyle.Bold);
            startButton.Location = new Point(246, 299);
            startButton.Name = "startButton";
            startButton.Size = new Size(138, 47);
            startButton.TabIndex = 0;
            startButton.Text = "[ START ]";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Helvetica Neue", 16F, FontStyle.Bold);
            titleLabel.ForeColor = SystemColors.ButtonHighlight;
            titleLabel.Location = new Point(184, 36);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(254, 27);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "[FFmpeg Command Library]";
            // 
            // logoPictureBox
            // 
            logoPictureBox.BorderStyle = BorderStyle.FixedSingle;
            logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
            logoPictureBox.Location = new Point(209, 66);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(202, 200);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 2;
            logoPictureBox.TabStop = false;
            // 
            // SplashForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowText;
            ClientSize = new Size(600, 380);
            Controls.Add(logoPictureBox);
            Controls.Add(titleLabel);
            Controls.Add(startButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SplashForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Welcome";
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
