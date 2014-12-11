namespace SMART.AI.Engine
{
    partial class QLearningControlPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.barLR = new System.Windows.Forms.TrackBar();
            this.barDF = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.barEF = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLR = new System.Windows.Forms.Label();
            this.lblDF = new System.Windows.Forms.Label();
            this.lblEF = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barLR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barEF)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Learning rate:";
            // 
            // barLR
            // 
            this.barLR.Location = new System.Drawing.Point(12, 49);
            this.barLR.Maximum = 100;
            this.barLR.Name = "barLR";
            this.barLR.Size = new System.Drawing.Size(263, 42);
            this.barLR.TabIndex = 1;
            this.barLR.Scroll += new System.EventHandler(this.barLR_Scroll);
            // 
            // barDF
            // 
            this.barDF.Location = new System.Drawing.Point(12, 123);
            this.barDF.Maximum = 100;
            this.barDF.Name = "barDF";
            this.barDF.Size = new System.Drawing.Size(263, 42);
            this.barDF.TabIndex = 3;
            this.barDF.Scroll += new System.EventHandler(this.barDF_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Discount factor:";
            // 
            // barEF
            // 
            this.barEF.Location = new System.Drawing.Point(17, 207);
            this.barEF.Maximum = 100;
            this.barEF.Name = "barEF";
            this.barEF.Size = new System.Drawing.Size(263, 42);
            this.barEF.TabIndex = 5;
            this.barEF.Scroll += new System.EventHandler(this.barEF_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Exploration factor:";
            // 
            // lblLR
            // 
            this.lblLR.AutoSize = true;
            this.lblLR.Location = new System.Drawing.Point(118, 33);
            this.lblLR.Name = "lblLR";
            this.lblLR.Size = new System.Drawing.Size(35, 13);
            this.lblLR.TabIndex = 6;
            this.lblLR.Text = "label4";
            // 
            // lblDF
            // 
            this.lblDF.AutoSize = true;
            this.lblDF.Location = new System.Drawing.Point(118, 107);
            this.lblDF.Name = "lblDF";
            this.lblDF.Size = new System.Drawing.Size(35, 13);
            this.lblDF.TabIndex = 7;
            this.lblDF.Text = "label4";
            // 
            // lblEF
            // 
            this.lblEF.AutoSize = true;
            this.lblEF.Location = new System.Drawing.Point(118, 191);
            this.lblEF.Name = "lblEF";
            this.lblEF.Size = new System.Drawing.Size(35, 13);
            this.lblEF.TabIndex = 8;
            this.lblEF.Text = "label4";
            // 
            // QLearningControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.lblEF);
            this.Controls.Add(this.lblDF);
            this.Controls.Add(this.lblLR);
            this.Controls.Add(this.barEF);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.barDF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.barLR);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QLearningControlPanel";
            this.ShowIcon = false;
            this.Text = "QLearningControlPanel";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QLearningControlPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barLR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barEF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar barLR;
        private System.Windows.Forms.TrackBar barDF;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar barEF;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLR;
        private System.Windows.Forms.Label lblDF;
        private System.Windows.Forms.Label lblEF;
    }
}