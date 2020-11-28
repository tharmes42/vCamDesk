namespace PerPixelAlphaForm
{
    partial class ParentForm
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
            this.camera1Combo = new System.Windows.Forms.ComboBox();
            this.flipHCheckBox = new System.Windows.Forms.CheckBox();
            this.useTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cropAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // camera1Combo
            // 
            this.camera1Combo.FormattingEnabled = true;
            this.camera1Combo.Location = new System.Drawing.Point(16, 17);
            this.camera1Combo.Margin = new System.Windows.Forms.Padding(2);
            this.camera1Combo.Name = "camera1Combo";
            this.camera1Combo.Size = new System.Drawing.Size(196, 21);
            this.camera1Combo.TabIndex = 0;
            this.camera1Combo.SelectedIndexChanged += new System.EventHandler(this.camera1Combo_SelectedIndexChanged);
            // 
            // flipHCheckBox
            // 
            this.flipHCheckBox.AutoSize = true;
            this.flipHCheckBox.Location = new System.Drawing.Point(16, 114);
            this.flipHCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.flipHCheckBox.Name = "flipHCheckBox";
            this.flipHCheckBox.Size = new System.Drawing.Size(92, 17);
            this.flipHCheckBox.TabIndex = 1;
            this.flipHCheckBox.Text = "Flip Horizontal";
            this.flipHCheckBox.UseVisualStyleBackColor = true;
            this.flipHCheckBox.CheckedChanged += new System.EventHandler(this.flipHCheckBox_CheckedChanged);
            // 
            // useTransparencyCheckBox
            // 
            this.useTransparencyCheckBox.AutoSize = true;
            this.useTransparencyCheckBox.Location = new System.Drawing.Point(16, 140);
            this.useTransparencyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.useTransparencyCheckBox.Name = "useTransparencyCheckBox";
            this.useTransparencyCheckBox.Size = new System.Drawing.Size(238, 17);
            this.useTransparencyCheckBox.TabIndex = 2;
            this.useTransparencyCheckBox.Text = "Transparency (needs XSplit or Snap Camera)";
            this.useTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.useTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.useTransparencyCheckBox_CheckedChanged);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(16, 58);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(109, 37);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start Webcam";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(161, 58);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(110, 37);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Quit";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(236, 20);
            this.resolutionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(35, 13);
            this.resolutionLabel.TabIndex = 7;
            this.resolutionLabel.Text = "label1";
            this.resolutionLabel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 189);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "+/- to resize";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Usage in webcam mode:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "left mouse button to drag and move";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = ",/. to zoom and crop";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "right mouse button to quit";
            // 
            // cropAutoCheckBox
            // 
            this.cropAutoCheckBox.AutoSize = true;
            this.cropAutoCheckBox.Location = new System.Drawing.Point(161, 114);
            this.cropAutoCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cropAutoCheckBox.Name = "cropAutoCheckBox";
            this.cropAutoCheckBox.Size = new System.Drawing.Size(140, 17);
            this.cropAutoCheckBox.TabIndex = 3;
            this.cropAutoCheckBox.Text = "Auto-crop (experimental)";
            this.cropAutoCheckBox.UseVisualStyleBackColor = true;
            this.cropAutoCheckBox.Visible = false;
            this.cropAutoCheckBox.CheckedChanged += new System.EventHandler(this.cropAutoCheckBox_CheckedChanged);
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 236);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resolutionLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.cropAutoCheckBox);
            this.Controls.Add(this.useTransparencyCheckBox);
            this.Controls.Add(this.flipHCheckBox);
            this.Controls.Add(this.camera1Combo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vCamDesk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParentForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox camera1Combo;
        private System.Windows.Forms.CheckBox flipHCheckBox;
        private System.Windows.Forms.CheckBox useTransparencyCheckBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cropAutoCheckBox;
    }
}