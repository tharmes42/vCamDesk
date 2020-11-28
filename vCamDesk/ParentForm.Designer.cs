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
            this.cropHCheckBox = new System.Windows.Forms.CheckBox();
            this.cropVCheckBox = new System.Windows.Forms.CheckBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // camera1Combo
            // 
            this.camera1Combo.FormattingEnabled = true;
            this.camera1Combo.Location = new System.Drawing.Point(69, 34);
            this.camera1Combo.Name = "camera1Combo";
            this.camera1Combo.Size = new System.Drawing.Size(356, 32);
            this.camera1Combo.TabIndex = 0;
            this.camera1Combo.SelectedIndexChanged += new System.EventHandler(this.camera1Combo_SelectedIndexChanged);
            // 
            // flipHCheckBox
            // 
            this.flipHCheckBox.AutoSize = true;
            this.flipHCheckBox.Location = new System.Drawing.Point(69, 282);
            this.flipHCheckBox.Name = "flipHCheckBox";
            this.flipHCheckBox.Size = new System.Drawing.Size(161, 29);
            this.flipHCheckBox.TabIndex = 1;
            this.flipHCheckBox.Text = "Flip Horizontal";
            this.flipHCheckBox.UseVisualStyleBackColor = true;
            this.flipHCheckBox.CheckedChanged += new System.EventHandler(this.flipHCheckBox_CheckedChanged);
            // 
            // useTransparencyCheckBox
            // 
            this.useTransparencyCheckBox.AutoSize = true;
            this.useTransparencyCheckBox.Location = new System.Drawing.Point(69, 331);
            this.useTransparencyCheckBox.Name = "useTransparencyCheckBox";
            this.useTransparencyCheckBox.Size = new System.Drawing.Size(159, 29);
            this.useTransparencyCheckBox.TabIndex = 2;
            this.useTransparencyCheckBox.Text = "Transparency";
            this.useTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.useTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.useTransparencyCheckBox_CheckedChanged);
            // 
            // cropHCheckBox
            // 
            this.cropHCheckBox.AutoSize = true;
            this.cropHCheckBox.Location = new System.Drawing.Point(336, 282);
            this.cropHCheckBox.Name = "cropHCheckBox";
            this.cropHCheckBox.Size = new System.Drawing.Size(173, 29);
            this.cropHCheckBox.TabIndex = 3;
            this.cropHCheckBox.Text = "Crop Horizontal";
            this.cropHCheckBox.UseVisualStyleBackColor = true;
            this.cropHCheckBox.Visible = false;
            this.cropHCheckBox.CheckedChanged += new System.EventHandler(this.cropHCheckBox_CheckedChanged);
            // 
            // cropVCheckBox
            // 
            this.cropVCheckBox.AutoSize = true;
            this.cropVCheckBox.Location = new System.Drawing.Point(336, 331);
            this.cropVCheckBox.Name = "cropVCheckBox";
            this.cropVCheckBox.Size = new System.Drawing.Size(151, 29);
            this.cropVCheckBox.TabIndex = 4;
            this.cropVCheckBox.Text = "Crop Vertical";
            this.cropVCheckBox.UseVisualStyleBackColor = true;
            this.cropVCheckBox.Visible = false;
            this.cropVCheckBox.CheckedChanged += new System.EventHandler(this.cropVCheckBox_CheckedChanged);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(69, 179);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(199, 69);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(336, 179);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(201, 69);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(69, 114);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(64, 25);
            this.resolutionLabel.TabIndex = 7;
            this.resolutionLabel.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Hint: use keys +/- to zoom and ,/. to crop video";
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resolutionLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.cropVCheckBox);
            this.Controls.Add(this.cropHCheckBox);
            this.Controls.Add(this.useTransparencyCheckBox);
            this.Controls.Add(this.flipHCheckBox);
            this.Controls.Add(this.camera1Combo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
        private System.Windows.Forms.CheckBox cropHCheckBox;
        private System.Windows.Forms.CheckBox cropVCheckBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label label1;
    }
}