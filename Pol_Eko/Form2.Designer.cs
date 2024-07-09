namespace Pol_Eko
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBoxFiles = new ListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            buttonBackup = new Button();
            SuspendLayout();
            // 
            // listBoxFiles
            // 
            listBoxFiles.AllowDrop = true;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.Location = new Point(185, 0);
            listBoxFiles.Margin = new Padding(3, 4, 3, 4);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(535, 604);
            listBoxFiles.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(5, 0);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(174, 36);
            button1.TabIndex = 2;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(5, 44);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(171, 40);
            button2.TabIndex = 3;
            button2.Text = "Delete";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(5, 92);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(171, 35);
            button3.TabIndex = 4;
            button3.Text = "CAll";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // buttonBackup
            // 
            buttonBackup.Location = new Point(5, 134);
            buttonBackup.Name = "buttonBackup";
            buttonBackup.Size = new Size(171, 29);
            buttonBackup.TabIndex = 5;
            buttonBackup.Text = "Backup";
            buttonBackup.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(buttonBackup);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBoxFiles);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
        }

        #endregion
        private ListBox listBoxFiles;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button btnGenerateReport;
        private Button buttonBackup;
    }
}