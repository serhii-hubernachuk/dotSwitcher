namespace dotSwitcher.UI
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonCancelSettings = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.checkBoxTrayIcon = new System.Windows.Forms.CheckBox();
            this.checkBoxAutorun = new System.Windows.Forms.CheckBox();
            this.checkBoxSmartSelection = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.buttonGithub = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSwitchHotkey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxConvertHotkey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSwitchLayoutHotkey = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancelSettings
            // 
            this.buttonCancelSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelSettings.Location = new System.Drawing.Point(359, 198);
            this.buttonCancelSettings.Name = "buttonCancelSettings";
            this.buttonCancelSettings.Size = new System.Drawing.Size(88, 28);
            this.buttonCancelSettings.TabIndex = 6;
            this.buttonCancelSettings.Text = "Cancel";
            this.buttonCancelSettings.UseVisualStyleBackColor = true;
            this.buttonCancelSettings.Click += new System.EventHandler(this.buttonCancelSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(265, 198);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(88, 28);
            this.buttonSaveSettings.TabIndex = 5;
            this.buttonSaveSettings.Text = "Apply";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(14, 198);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(95, 28);
            this.buttonExit.TabIndex = 7;
            this.buttonExit.Text = "Exit program";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // checkBoxTrayIcon
            // 
            this.checkBoxTrayIcon.AutoSize = true;
            this.checkBoxTrayIcon.Location = new System.Drawing.Point(14, 44);
            this.checkBoxTrayIcon.Name = "checkBoxTrayIcon";
            this.checkBoxTrayIcon.Size = new System.Drawing.Size(104, 19);
            this.checkBoxTrayIcon.TabIndex = 9;
            this.checkBoxTrayIcon.Text = "Show tray icon";
            this.checkBoxTrayIcon.UseVisualStyleBackColor = true;
            this.checkBoxTrayIcon.CheckedChanged += new System.EventHandler(this.checkBoxTrayIcon_CheckedChanged);
            // 
            // checkBoxAutorun
            // 
            this.checkBoxAutorun.AutoSize = true;
            this.checkBoxAutorun.Location = new System.Drawing.Point(14, 18);
            this.checkBoxAutorun.Name = "checkBoxAutorun";
            this.checkBoxAutorun.Size = new System.Drawing.Size(158, 19);
            this.checkBoxAutorun.TabIndex = 8;
            this.checkBoxAutorun.Text = "Start on Windows startup";
            this.checkBoxAutorun.UseVisualStyleBackColor = true;
            this.checkBoxAutorun.CheckedChanged += new System.EventHandler(this.checkBoxAutorun_CheckedChanged);
            // 
            // checkBoxSmartSelection
            // 
            this.checkBoxSmartSelection.AutoSize = true;
            this.checkBoxSmartSelection.Checked = true;
            this.checkBoxSmartSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSmartSelection.Location = new System.Drawing.Point(14, 70);
            this.checkBoxSmartSelection.Name = "checkBoxSmartSelection";
            this.checkBoxSmartSelection.Size = new System.Drawing.Size(170, 19);
            this.checkBoxSmartSelection.TabIndex = 10;
            this.checkBoxSmartSelection.Text = "Use smart convert selection";
            this.checkBoxSmartSelection.UseVisualStyleBackColor = true;
            this.checkBoxSmartSelection.CheckedChanged += new System.EventHandler(this.smartSelection_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Delay before switching:";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(14, 118);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(75, 23);
            this.textBoxDelay.TabIndex = 12;
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
            // 
            // buttonGithub
            // 
            this.buttonGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonGithub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGithub.Image = global::dotSwitcher.Properties.Resources.github;
            this.buttonGithub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonGithub.Location = new System.Drawing.Point(14, 152);
            this.buttonGithub.Name = "buttonGithub";
            this.buttonGithub.Size = new System.Drawing.Size(138, 28);
            this.buttonGithub.TabIndex = 13;
            this.buttonGithub.Text = "Report an issue";
            this.buttonGithub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGithub.UseVisualStyleBackColor = true;
            this.buttonGithub.Click += new System.EventHandler(this.buttonGithub_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxSwitchHotkey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxConvertHotkey);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxSwitchLayoutHotkey);
            this.groupBox1.Location = new System.Drawing.Point(205, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hotkeys";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Convert-last-word:";
            // 
            // textBoxSwitchHotkey
            // 
            this.textBoxSwitchHotkey.Location = new System.Drawing.Point(124, 25);
            this.textBoxSwitchHotkey.Name = "textBoxSwitchHotkey";
            this.textBoxSwitchHotkey.Size = new System.Drawing.Size(106, 23);
            this.textBoxSwitchHotkey.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Convert-selection:";
            // 
            // textBoxConvertHotkey
            // 
            this.textBoxConvertHotkey.Location = new System.Drawing.Point(124, 65);
            this.textBoxConvertHotkey.Name = "textBoxConvertHotkey";
            this.textBoxConvertHotkey.Size = new System.Drawing.Size(106, 23);
            this.textBoxConvertHotkey.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Switch layout:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(92, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "?";
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            this.label5.MouseHover += new System.EventHandler(this.label5_MouseHover);
            // 
            // textBoxSwitchLayoutHotkey
            // 
            this.textBoxSwitchLayoutHotkey.Location = new System.Drawing.Point(124, 105);
            this.textBoxSwitchLayoutHotkey.Name = "textBoxSwitchLayoutHotkey";
            this.textBoxSwitchLayoutHotkey.Size = new System.Drawing.Size(106, 23);
            this.textBoxSwitchLayoutHotkey.TabIndex = 3;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelSettings;
            this.ClientSize = new System.Drawing.Size(462, 240);
            this.Controls.Add(this.buttonGithub);
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxSmartSelection);
            this.Controls.Add(this.checkBoxTrayIcon);
            this.Controls.Add(this.checkBoxAutorun);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonCancelSettings);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dotSwitcher Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancelSettings;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.CheckBox checkBoxTrayIcon;
        private System.Windows.Forms.CheckBox checkBoxAutorun;
        private System.Windows.Forms.CheckBox checkBoxSmartSelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.Button buttonGithub;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSwitchHotkey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxConvertHotkey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSwitchLayoutHotkey;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
