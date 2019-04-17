namespace TCPTunnel_Client
{
    partial class TCP_Tunnel
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
            this.nameData = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.userList = new System.Windows.Forms.ListBox();
            this.Sending_settings = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Sending = new System.Windows.Forms.Button();
            this.File_Show = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Sending_settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameData
            // 
            this.nameData.AutoSize = true;
            this.nameData.Location = new System.Drawing.Point(12, 9);
            this.nameData.Name = "nameData";
            this.nameData.Size = new System.Drawing.Size(35, 13);
            this.nameData.TabIndex = 20;
            this.nameData.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(399, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Список пользователей: ";
            // 
            // userList
            // 
            this.userList.Enabled = false;
            this.userList.FormattingEnabled = true;
            this.userList.Location = new System.Drawing.Point(396, 23);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(156, 108);
            this.userList.TabIndex = 18;
            // 
            // Sending_settings
            // 
            this.Sending_settings.Controls.Add(this.label1);
            this.Sending_settings.Controls.Add(this.comboBox1);
            this.Sending_settings.Controls.Add(this.label6);
            this.Sending_settings.Controls.Add(this.label5);
            this.Sending_settings.Controls.Add(this.label2);
            this.Sending_settings.Controls.Add(this.Sending);
            this.Sending_settings.Controls.Add(this.File_Show);
            this.Sending_settings.Location = new System.Drawing.Point(12, 25);
            this.Sending_settings.Name = "Sending_settings";
            this.Sending_settings.Size = new System.Drawing.Size(378, 152);
            this.Sending_settings.TabIndex = 21;
            this.Sending_settings.TabStop = false;
            this.Sending_settings.Text = "Трансфер";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "RSA",
            "DES",
            "AES"});
            this.comboBox1.Location = new System.Drawing.Point(60, 85);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(162, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(203, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "label5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Выбранный пользователь:";
            // 
            // Sending
            // 
            this.Sending.Location = new System.Drawing.Point(297, 123);
            this.Sending.Name = "Sending";
            this.Sending.Size = new System.Drawing.Size(75, 23);
            this.Sending.TabIndex = 28;
            this.Sending.Text = "Отправить";
            this.Sending.UseVisualStyleBackColor = true;
            // 
            // File_Show
            // 
            this.File_Show.Location = new System.Drawing.Point(60, 41);
            this.File_Show.Name = "File_Show";
            this.File_Show.Size = new System.Drawing.Size(96, 23);
            this.File_Show.TabIndex = 27;
            this.File_Show.Text = "Выбрать файл";
            this.File_Show.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Метод кодирования";
            // 
            // TCP_Tunnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 189);
            this.Controls.Add(this.Sending_settings);
            this.Controls.Add(this.nameData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.userList);
            this.Name = "TCP_Tunnel";
            this.Text = "Шифрованный теннель";
            this.Load += new System.EventHandler(this.TCP_Tunnel_Load);
            this.Sending_settings.ResumeLayout(false);
            this.Sending_settings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label nameData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.GroupBox Sending_settings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Sending;
        private System.Windows.Forms.Button File_Show;
    }
}