namespace SMART.AI.AITest
{
    partial class QLearningTestForm
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
            this.speedMeter = new System.Windows.Forms.TrackBar();
            this.panelUp = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelDown = new System.Windows.Forms.Panel();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.btnTick = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnWatch = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.imgDisplay = new SMART.AI.AITest.AITestPictureBox();
            this.checkRnd = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.speedMeter)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // speedMeter
            // 
            this.speedMeter.Location = new System.Drawing.Point(29, 532);
            this.speedMeter.Maximum = 80;
            this.speedMeter.Name = "speedMeter";
            this.speedMeter.Size = new System.Drawing.Size(233, 42);
            this.speedMeter.TabIndex = 1;
            this.speedMeter.Scroll += new System.EventHandler(this.speedMeter_Scroll);
            // 
            // panelUp
            // 
            this.panelUp.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelUp.Location = new System.Drawing.Point(569, 496);
            this.panelUp.Name = "panelUp";
            this.panelUp.Size = new System.Drawing.Size(32, 30);
            this.panelUp.TabIndex = 2;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelRight.Location = new System.Drawing.Point(607, 532);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(32, 30);
            this.panelRight.TabIndex = 3;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelLeft.Location = new System.Drawing.Point(531, 532);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(32, 30);
            this.panelLeft.TabIndex = 4;
            // 
            // panelDown
            // 
            this.panelDown.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelDown.Location = new System.Drawing.Point(569, 566);
            this.panelDown.Name = "panelDown";
            this.panelDown.Size = new System.Drawing.Size(32, 30);
            this.panelDown.TabIndex = 3;
            // 
            // txtDelay
            // 
            this.txtDelay.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDelay.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtDelay.Font = new System.Drawing.Font("Motorwerk", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDelay.ForeColor = System.Drawing.Color.Yellow;
            this.txtDelay.Location = new System.Drawing.Point(37, 496);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.ReadOnly = true;
            this.txtDelay.Size = new System.Drawing.Size(225, 19);
            this.txtDelay.TabIndex = 5;
            this.txtDelay.Text = "Update speed: Stopped";
            // 
            // btnTick
            // 
            this.btnTick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTick.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTick.Location = new System.Drawing.Point(277, 531);
            this.btnTick.Name = "btnTick";
            this.btnTick.Size = new System.Drawing.Size(83, 65);
            this.btnTick.TabIndex = 6;
            this.btnTick.Text = "Tick";
            this.btnTick.UseVisualStyleBackColor = true;
            this.btnTick.Click += new System.EventHandler(this.btnTick_Click);
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Motorwerk", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(375, 531);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(83, 65);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(731, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.saveAsToolStripMenuItem.Text = "Save as..";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.openToolStripMenuItem.Text = "Load..";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Motorwerk", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(37, 567);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(48, 29);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnWatch
            // 
            this.btnWatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatch.Font = new System.Drawing.Font("Motorwerk", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWatch.Location = new System.Drawing.Point(91, 567);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(85, 29);
            this.btnWatch.TabIndex = 10;
            this.btnWatch.Text = "Watch";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrain.Font = new System.Drawing.Font("Motorwerk", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrain.Location = new System.Drawing.Point(182, 567);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(80, 29);
            this.btnTrain.TabIndex = 11;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // imgDisplay
            // 
            this.imgDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgDisplay.Location = new System.Drawing.Point(29, 27);
            this.imgDisplay.Name = "imgDisplay";
            this.imgDisplay.Size = new System.Drawing.Size(659, 452);
            this.imgDisplay.TabIndex = 0;
            this.imgDisplay.TabStop = false;
            // 
            // checkRnd
            // 
            this.checkRnd.AutoSize = true;
            this.checkRnd.Checked = true;
            this.checkRnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkRnd.Font = new System.Drawing.Font("Motorwerk", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkRnd.Location = new System.Drawing.Point(277, 496);
            this.checkRnd.Name = "checkRnd";
            this.checkRnd.Size = new System.Drawing.Size(136, 16);
            this.checkRnd.TabIndex = 12;
            this.checkRnd.Text = "Randomize food";
            this.checkRnd.UseVisualStyleBackColor = true;
            // 
            // AITestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 610);
            this.Controls.Add(this.checkRnd);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnWatch);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnTick);
            this.Controls.Add(this.txtDelay);
            this.Controls.Add(this.panelDown);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelUp);
            this.Controls.Add(this.speedMeter);
            this.Controls.Add(this.imgDisplay);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AITestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AITestForm";
            this.Load += new System.EventHandler(this.AITestForm_Load);
            this.LocationChanged += new System.EventHandler(this.AITestForm_LocationChanged);
            ((System.ComponentModel.ISupportInitialize)(this.speedMeter)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar speedMeter;
        private System.Windows.Forms.Panel panelUp;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.TextBox txtDelay;
        private AITestPictureBox imgDisplay;
        private System.Windows.Forms.Button btnTick;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.CheckBox checkRnd;
    }
}