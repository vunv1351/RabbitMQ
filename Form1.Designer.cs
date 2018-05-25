namespace SendUDP
{
    partial class Form1
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
            this.txtsend = new System.Windows.Forms.TextBox();
            this.btnsend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtsend
            // 
            this.txtsend.Location = new System.Drawing.Point(85, 81);
            this.txtsend.Name = "txtsend";
            this.txtsend.Size = new System.Drawing.Size(114, 20);
            this.txtsend.TabIndex = 3;
            this.txtsend.TextChanged += new System.EventHandler(this.txtsend_TextChanged);
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(99, 156);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(75, 23);
            this.btnsend.TabIndex = 2;
            this.btnsend.Text = "Send";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "codedeploy aws";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtsend);
            this.Controls.Add(this.btnsend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtsend;
        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.Label label1;
    }
}

