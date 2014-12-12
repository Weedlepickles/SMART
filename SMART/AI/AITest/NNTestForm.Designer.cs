namespace SMART.AI.AITest
{
    partial class NNTestForm
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
            this.txtInput1 = new System.Windows.Forms.TextBox();
            this.txtInput2 = new System.Windows.Forms.TextBox();
            this.txtOutput1 = new System.Windows.Forms.TextBox();
            this.txtOutput2 = new System.Windows.Forms.TextBox();
            this.btnPropForward = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAutoTrain = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInput1
            // 
            this.txtInput1.Location = new System.Drawing.Point(40, 84);
            this.txtInput1.Name = "txtInput1";
            this.txtInput1.Size = new System.Drawing.Size(108, 20);
            this.txtInput1.TabIndex = 0;
            // 
            // txtInput2
            // 
            this.txtInput2.Location = new System.Drawing.Point(40, 134);
            this.txtInput2.Name = "txtInput2";
            this.txtInput2.Size = new System.Drawing.Size(108, 20);
            this.txtInput2.TabIndex = 1;
            // 
            // txtOutput1
            // 
            this.txtOutput1.Location = new System.Drawing.Point(254, 84);
            this.txtOutput1.Name = "txtOutput1";
            this.txtOutput1.Size = new System.Drawing.Size(108, 20);
            this.txtOutput1.TabIndex = 2;
            // 
            // txtOutput2
            // 
            this.txtOutput2.Location = new System.Drawing.Point(254, 134);
            this.txtOutput2.Name = "txtOutput2";
            this.txtOutput2.Size = new System.Drawing.Size(108, 20);
            this.txtOutput2.TabIndex = 3;
            // 
            // btnPropForward
            // 
            this.btnPropForward.Location = new System.Drawing.Point(40, 190);
            this.btnPropForward.Name = "btnPropForward";
            this.btnPropForward.Size = new System.Drawing.Size(108, 25);
            this.btnPropForward.TabIndex = 4;
            this.btnPropForward.Text = "Propagate ->";
            this.btnPropForward.UseVisualStyleBackColor = true;
            this.btnPropForward.Click += new System.EventHandler(this.btnPropForward_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(254, 190);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(108, 25);
            this.btnTrain.TabIndex = 5;
            this.btnTrain.Text = "<- Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(43, 285);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(318, 89);
            this.txtLog.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Errors:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "State";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Output";
            // 
            // btnAutoTrain
            // 
            this.btnAutoTrain.Location = new System.Drawing.Point(40, 233);
            this.btnAutoTrain.Name = "btnAutoTrain";
            this.btnAutoTrain.Size = new System.Drawing.Size(107, 23);
            this.btnAutoTrain.TabIndex = 10;
            this.btnAutoTrain.Text = "Auto train";
            this.btnAutoTrain.UseVisualStyleBackColor = true;
            this.btnAutoTrain.Click += new System.EventHandler(this.btnAutoTrain_Click);
            // 
            // NNTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 386);
            this.Controls.Add(this.btnAutoTrain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnPropForward);
            this.Controls.Add(this.txtOutput2);
            this.Controls.Add(this.txtOutput1);
            this.Controls.Add(this.txtInput2);
            this.Controls.Add(this.txtInput1);
            this.Name = "NNTestForm";
            this.Text = "NNTestForm";
            this.Load += new System.EventHandler(this.NNTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput1;
        private System.Windows.Forms.TextBox txtInput2;
        private System.Windows.Forms.TextBox txtOutput1;
        private System.Windows.Forms.TextBox txtOutput2;
        private System.Windows.Forms.Button btnPropForward;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAutoTrain;
    }
}