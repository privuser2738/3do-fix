namespace CDImageConverter
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Button btnSelectOutput;
        private System.Windows.Forms.Label lblChdmanPath;
        private System.Windows.Forms.Button btnSelectChdman;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.btnSelectOutput = new System.Windows.Forms.Button();
            this.lblChdmanPath = new System.Windows.Forms.Label();
            this.btnSelectChdman = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(280, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CD Image Converter (CHD/ISO)";
            //
            // txtInputFile
            //
            this.txtInputFile.Location = new System.Drawing.Point(12, 50);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.PlaceholderText = "Select input file (.chd or .iso)";
            this.txtInputFile.ReadOnly = true;
            this.txtInputFile.Size = new System.Drawing.Size(500, 23);
            this.txtInputFile.TabIndex = 1;
            //
            // btnSelectFile
            //
            this.btnSelectFile.Location = new System.Drawing.Point(518, 49);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(100, 25);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Browse...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            //
            // lblOutputFolder
            //
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(12, 85);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(200, 15);
            this.lblOutputFolder.TabIndex = 3;
            this.lblOutputFolder.Text = "Output: C:\\Users\\rob\\Games\\3DO";
            //
            // btnSelectOutput
            //
            this.btnSelectOutput.Location = new System.Drawing.Point(518, 80);
            this.btnSelectOutput.Name = "btnSelectOutput";
            this.btnSelectOutput.Size = new System.Drawing.Size(100, 25);
            this.btnSelectOutput.TabIndex = 4;
            this.btnSelectOutput.Text = "Change...";
            this.btnSelectOutput.UseVisualStyleBackColor = true;
            this.btnSelectOutput.Click += new System.EventHandler(this.btnSelectOutput_Click);
            //
            // lblChdmanPath
            //
            this.lblChdmanPath.AutoSize = true;
            this.lblChdmanPath.Location = new System.Drawing.Point(12, 115);
            this.lblChdmanPath.Name = "lblChdmanPath";
            this.lblChdmanPath.Size = new System.Drawing.Size(300, 15);
            this.lblChdmanPath.TabIndex = 5;
            this.lblChdmanPath.Text = "chdman: C:\\Users\\rob\\Games\\3DO\\MAME\\chdman.exe";
            //
            // btnSelectChdman
            //
            this.btnSelectChdman.Location = new System.Drawing.Point(518, 110);
            this.btnSelectChdman.Name = "btnSelectChdman";
            this.btnSelectChdman.Size = new System.Drawing.Size(100, 25);
            this.btnSelectChdman.TabIndex = 6;
            this.btnSelectChdman.Text = "Configure...";
            this.btnSelectChdman.UseVisualStyleBackColor = true;
            this.btnSelectChdman.Click += new System.EventHandler(this.btnSelectChdman_Click);
            //
            // btnConvert
            //
            this.btnConvert.BackColor = System.Drawing.Color.Green;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.Location = new System.Drawing.Point(12, 150);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(606, 40);
            this.btnConvert.TabIndex = 7;
            this.btnConvert.Text = "Convert to BIN + CUE";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(12, 200);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(606, 23);
            this.progressBar.TabIndex = 8;
            //
            // txtLog
            //
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(12, 235);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(606, 200);
            this.txtLog.TabIndex = 9;
            this.txtLog.Text = "Ready to convert CD images.\r\n\r\n";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnSelectChdman);
            this.Controls.Add(this.lblChdmanPath);
            this.Controls.Add(this.btnSelectOutput);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CD Image Converter - CHD/ISO to BIN/CUE";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
