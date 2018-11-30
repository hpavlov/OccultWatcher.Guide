namespace OccultWatcher.Guide
{
    partial class guideFrmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(guideFrmConfig));
            this.lblGuidePath = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.tbxPathToGuide = new System.Windows.Forms.TextBox();
            this.lblOptional = new System.Windows.Forms.Label();
            this.tbxGuideConfig = new System.Windows.Forms.TextBox();
            this.cbxAlwaysInNewInstance = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblGuidePath
            // 
            this.lblGuidePath.AutoSize = true;
            this.lblGuidePath.Location = new System.Drawing.Point(12, 9);
            this.lblGuidePath.Name = "lblGuidePath";
            this.lblGuidePath.Size = new System.Drawing.Size(259, 13);
            this.lblGuidePath.TabIndex = 0;
            this.lblGuidePath.Text = "Set the path to \'Guide\'\'s executable on your computer";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(334, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(253, 178);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(289, 33);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(120, 26);
            this.btnOpenFileDialog.TabIndex = 2;
            this.btnOpenFileDialog.Text = "Browse ...";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // tbxPathToGuide
            // 
            this.tbxPathToGuide.Location = new System.Drawing.Point(15, 36);
            this.tbxPathToGuide.Name = "tbxPathToGuide";
            this.tbxPathToGuide.Size = new System.Drawing.Size(268, 20);
            this.tbxPathToGuide.TabIndex = 1;
            // 
            // lblOptional
            // 
            this.lblOptional.AutoSize = true;
            this.lblOptional.Location = new System.Drawing.Point(14, 71);
            this.lblOptional.Name = "lblOptional";
            this.lblOptional.Size = new System.Drawing.Size(314, 13);
            this.lblOptional.TabIndex = 6;
            this.lblOptional.Text = "Optional: Add name of Guide- configuration (exactly 8 characters)";
            // 
            // tbxGuideConfig
            // 
            this.tbxGuideConfig.Location = new System.Drawing.Point(15, 98);
            this.tbxGuideConfig.Name = "tbxGuideConfig";
            this.tbxGuideConfig.Size = new System.Drawing.Size(209, 20);
            this.tbxGuideConfig.TabIndex = 7;
            // 
            // cbxAlwaysInNewInstance
            // 
            this.cbxAlwaysInNewInstance.AutoSize = true;
            this.cbxAlwaysInNewInstance.Location = new System.Drawing.Point(17, 143);
            this.cbxAlwaysInNewInstance.Name = "cbxAlwaysInNewInstance";
            this.cbxAlwaysInNewInstance.Size = new System.Drawing.Size(216, 17);
            this.cbxAlwaysInNewInstance.TabIndex = 8;
            this.cbxAlwaysInNewInstance.Text = "Open events in a new instance of Guide";
            this.cbxAlwaysInNewInstance.UseVisualStyleBackColor = true;
            // 
            // guideFrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 213);
            this.Controls.Add(this.cbxAlwaysInNewInstance);
            this.Controls.Add(this.tbxGuideConfig);
            this.Controls.Add(this.lblOptional);
            this.Controls.Add(this.tbxPathToGuide);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblGuidePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "guideFrmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OW Guide Plugin Configuration";
            this.Load += new System.EventHandler(this.guideFrmConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGuidePath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.TextBox tbxPathToGuide;
        private System.Windows.Forms.Label lblOptional;
        private System.Windows.Forms.TextBox tbxGuideConfig;
        private System.Windows.Forms.CheckBox cbxAlwaysInNewInstance;
    }
}