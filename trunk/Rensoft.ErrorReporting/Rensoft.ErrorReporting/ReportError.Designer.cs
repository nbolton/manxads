namespace Rensoft.ErrorReporting
{
    partial class ReportError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportError));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.commentsGroupBox = new System.Windows.Forms.GroupBox();
            this.datPathTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.commentsTextBox = new System.Windows.Forms.TextBox();
            this.restartCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.commentsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::Rensoft.ErrorReporting.Properties.Resources.RensoftLogoStrip;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(498, 71);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(473, 30);
            this.label2.TabIndex = 6;
            this.label2.Text = "An error has occurred in the application you were using. To help us improve the q" +
                "uality of this application, please send an error report to Rensoft using this wi" +
                "ndow.";
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(411, 260);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _acceptButton
            // 
            this._acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._acceptButton.Location = new System.Drawing.Point(330, 260);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 2;
            this._acceptButton.Text = "&Send";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // commentsGroupBox
            // 
            this.commentsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commentsGroupBox.Controls.Add(this.datPathTextBox);
            this.commentsGroupBox.Controls.Add(this.label5);
            this.commentsGroupBox.Controls.Add(this.commentsTextBox);
            this.commentsGroupBox.Location = new System.Drawing.Point(12, 116);
            this.commentsGroupBox.Name = "commentsGroupBox";
            this.commentsGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.commentsGroupBox.Size = new System.Drawing.Size(474, 138);
            this.commentsGroupBox.TabIndex = 0;
            this.commentsGroupBox.TabStop = false;
            this.commentsGroupBox.Text = "Comments (optional)";
            // 
            // datPathTextBox
            // 
            this.datPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.datPathTextBox.Location = new System.Drawing.Point(13, 105);
            this.datPathTextBox.Name = "datPathTextBox";
            this.datPathTextBox.ReadOnly = true;
            this.datPathTextBox.Size = new System.Drawing.Size(448, 20);
            this.datPathTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(306, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Please describe what you were doing before the error occurred.";
            // 
            // commentsTextBox
            // 
            this.commentsTextBox.AcceptsReturn = true;
            this.commentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commentsTextBox.Location = new System.Drawing.Point(13, 39);
            this.commentsTextBox.Multiline = true;
            this.commentsTextBox.Name = "commentsTextBox";
            this.commentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commentsTextBox.Size = new System.Drawing.Size(448, 60);
            this.commentsTextBox.TabIndex = 0;
            // 
            // restartCheckBox
            // 
            this.restartCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.restartCheckBox.AutoSize = true;
            this.restartCheckBox.Checked = true;
            this.restartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.restartCheckBox.Location = new System.Drawing.Point(12, 264);
            this.restartCheckBox.Name = "restartCheckBox";
            this.restartCheckBox.Size = new System.Drawing.Size(237, 17);
            this.restartCheckBox.TabIndex = 1;
            this.restartCheckBox.Text = "&Restart application when this window closes.";
            this.restartCheckBox.UseVisualStyleBackColor = true;
            // 
            // ReportError
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(498, 295);
            this.Controls.Add(this.restartCheckBox);
            this.Controls.Add(this.commentsGroupBox);
            this.Controls.Add(this._acceptButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rensoft Error Reporting";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.commentsGroupBox.ResumeLayout(false);
            this.commentsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.GroupBox commentsGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox commentsTextBox;
        private System.Windows.Forms.CheckBox restartCheckBox;
        private System.Windows.Forms.TextBox datPathTextBox;
    }
}