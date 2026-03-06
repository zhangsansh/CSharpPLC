namespace 西门子PLC上位机通讯软件
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            button4 = new Button();
            btn_Read = new Button();
            btn_VarConfig = new Button();
            btn_Connect = new Button();
            txt_IP = new TextBox();
            label1 = new Label();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button4);
            panel1.Controls.Add(btn_Read);
            panel1.Controls.Add(btn_VarConfig);
            panel1.Controls.Add(btn_Connect);
            panel1.Controls.Add(txt_IP);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(907, 148);
            panel1.TabIndex = 0;
            // 
            // button4
            // 
            button4.Location = new Point(753, 52);
            button4.Name = "button4";
            button4.Size = new Size(112, 34);
            button4.TabIndex = 5;
            button4.Text = "导出配置";
            button4.UseVisualStyleBackColor = true;
            // 
            // btn_Read
            // 
            btn_Read.Location = new Point(595, 52);
            btn_Read.Name = "btn_Read";
            btn_Read.Size = new Size(112, 34);
            btn_Read.TabIndex = 4;
            btn_Read.Text = "开始读取";
            btn_Read.UseVisualStyleBackColor = true;
            btn_Read.Click += btn_Read_Click;
            // 
            // btn_VarConfig
            // 
            btn_VarConfig.Location = new Point(449, 52);
            btn_VarConfig.Name = "btn_VarConfig";
            btn_VarConfig.Size = new Size(112, 34);
            btn_VarConfig.TabIndex = 3;
            btn_VarConfig.Text = "变量配置";
            btn_VarConfig.UseVisualStyleBackColor = true;
            btn_VarConfig.Click += btn_VarConfig_Click;
            // 
            // btn_Connect
            // 
            btn_Connect.Location = new Point(297, 52);
            btn_Connect.Name = "btn_Connect";
            btn_Connect.Size = new Size(112, 34);
            btn_Connect.TabIndex = 2;
            btn_Connect.Text = "建立连接";
            btn_Connect.UseVisualStyleBackColor = true;
            btn_Connect.Click += btn_Connect_Click;
            // 
            // txt_IP
            // 
            txt_IP.Location = new Point(92, 54);
            txt_IP.Name = "txt_IP";
            txt_IP.Size = new Size(150, 30);
            txt_IP.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 57);
            label1.Name = "label1";
            label1.Size = new Size(80, 24);
            label1.TabIndex = 0;
            label1.Text = "IP地址：";
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 148);
            panel2.Name = "panel2";
            panel2.Size = new Size(907, 779);
            panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(907, 779);
            dataGridView1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(907, 927);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button4;
        private Button btn_Read;
        private Button btn_VarConfig;
        private Button btn_Connect;
        private TextBox txt_IP;
        private Label label1;
        private Panel panel2;
        private DataGridView dataGridView1;
    }
}
