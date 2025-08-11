namespace FmpegLists
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
            panel1 = new Panel();
            label1 = new Label();
            commandNameTextBox = new TextBox();
            commandTextBox = new TextBox();
            addButton = new Button();
            commandListBox = new ListBox();
            commandSelector = new ComboBox();
            runButton = new Button();
            outputTextBox = new TextBox();
            rmButton = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AccessibleName = "dropPanel";
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Location = new Point(12, 190);
            panel1.Name = "panel1";
            panel1.Size = new Size(400, 100);
            panel1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AccessibleName = "dropLabel";
            label1.AutoSize = true;
            label1.Location = new Point(123, 37);
            label1.Name = "label1";
            label1.Size = new Size(126, 15);
            label1.TabIndex = 0;
            label1.Text = "Drop file or folder here";
            // 
            // commandNameTextBox
            // 
            commandNameTextBox.Location = new Point(12, 12);
            commandNameTextBox.Name = "commandNameTextBox";
            commandNameTextBox.Size = new Size(200, 23);
            commandNameTextBox.TabIndex = 0;
            // 
            // commandTextBox
            // 
            commandTextBox.Location = new Point(12, 41);
            commandTextBox.Name = "commandTextBox";
            commandTextBox.Size = new Size(300, 23);
            commandTextBox.TabIndex = 1;
            // 
            // addButton
            // 
            addButton.Location = new Point(318, 39);
            addButton.Name = "addButton";
            addButton.Size = new Size(100, 30);
            addButton.TabIndex = 2;
            addButton.Text = "+ Command";
            addButton.UseVisualStyleBackColor = true;
            // 
            // commandListBox
            // 
            commandListBox.FormattingEnabled = true;
            commandListBox.ItemHeight = 15;
            commandListBox.Location = new Point(12, 79);
            commandListBox.Name = "commandListBox";
            commandListBox.Size = new Size(300, 94);
            commandListBox.TabIndex = 3;
            // 
            // commandSelector
            // 
            commandSelector.FormattingEnabled = true;
            commandSelector.Location = new Point(12, 300);
            commandSelector.Name = "commandSelector";
            commandSelector.Size = new Size(300, 23);
            commandSelector.TabIndex = 6;
            // 
            // runButton
            // 
            runButton.Location = new Point(318, 300);
            runButton.Name = "runButton";
            runButton.Size = new Size(100, 30);
            runButton.TabIndex = 7;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = true;
            // 
            // outputTextBox
            // 
            outputTextBox.Location = new Point(12, 340);
            outputTextBox.Multiline = true;
            outputTextBox.Name = "outputTextBox";
            outputTextBox.ScrollBars = ScrollBars.Vertical;
            outputTextBox.Size = new Size(400, 100);
            outputTextBox.TabIndex = 8;
            // 
            // rmButton
            // 
            rmButton.Location = new Point(318, 75);
            rmButton.Name = "rmButton";
            rmButton.Size = new Size(100, 30);
            rmButton.TabIndex = 9;
            rmButton.Text = "- Command";
            rmButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 452);
            Controls.Add(rmButton);
            Controls.Add(outputTextBox);
            Controls.Add(runButton);
            Controls.Add(commandSelector);
            Controls.Add(panel1);
            Controls.Add(commandListBox);
            Controls.Add(addButton);
            Controls.Add(commandTextBox);
            Controls.Add(commandNameTextBox);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFmpeg Command Library";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private TextBox commandNameTextBox;
        private TextBox commandTextBox;
        private Button addButton;
        private ListBox commandListBox;
        private ComboBox commandSelector;
        private Button runButton;
        private TextBox outputTextBox;
        private Button rmButton;
    }
}