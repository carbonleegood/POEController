namespace Controller
{
    partial class Login
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
            this.btnInject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.tbPWD = new System.Windows.Forms.TextBox();
            this.btnSaveUID = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(31, 59);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(75, 23);
            this.btnInject.TabIndex = 0;
            this.btnInject.Text = "注入启动";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "账号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码:";
            // 
            // tbUID
            // 
            this.tbUID.Location = new System.Drawing.Point(101, 132);
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(147, 21);
            this.tbUID.TabIndex = 4;
            // 
            // tbPWD
            // 
            this.tbPWD.Location = new System.Drawing.Point(101, 178);
            this.tbPWD.Name = "tbPWD";
            this.tbPWD.PasswordChar = '*';
            this.tbPWD.Size = new System.Drawing.Size(147, 21);
            this.tbPWD.TabIndex = 5;
            // 
            // btnSaveUID
            // 
            this.btnSaveUID.Location = new System.Drawing.Point(154, 227);
            this.btnSaveUID.Name = "btnSaveUID";
            this.btnSaveUID.Size = new System.Drawing.Size(94, 23);
            this.btnSaveUID.TabIndex = 6;
            this.btnSaveUID.Text = "保存账号密码";
            this.btnSaveUID.UseVisualStyleBackColor = true;
            this.btnSaveUID.Click += new System.EventHandler(this.btnSaveUID_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(170, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSaveUID);
            this.Controls.Add(this.tbPWD);
            this.Controls.Add(this.tbUID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInject);
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.TextBox tbPWD;
        private System.Windows.Forms.Button btnSaveUID;
        private System.Windows.Forms.Button button1;
    }
}