namespace calendar4
{
    partial class Mypage
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
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new GroupBox();
            button4 = new Button();
            btnRe = new Button();
            txtReemail = new TextBox();
            txtRename = new TextBox();
            txtPw = new TextBox();
            txtRepw = new TextBox();
            label6 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 36F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(374, 9);
            label1.Name = "label1";
            label1.Size = new Size(268, 65);
            label1.TabIndex = 0;
            label1.Text = "마이페이지";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 83);
            label3.Name = "label3";
            label3.Size = new Size(83, 15);
            label3.TabIndex = 1;
            label3.Text = "비밀번호 수정";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 135);
            label4.Name = "label4";
            label4.Size = new Size(59, 15);
            label4.TabIndex = 1;
            label4.Text = "이름 수정";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 187);
            label5.Name = "label5";
            label5.Size = new Size(69, 15);
            label5.TabIndex = 1;
            label5.Text = "E-mail 수정";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(btnRe);
            groupBox1.Controls.Add(txtReemail);
            groupBox1.Controls.Add(txtRename);
            groupBox1.Controls.Add(txtPw);
            groupBox1.Controls.Add(txtRepw);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Point(708, 150);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(342, 316);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "회원정보수정";
            // 
            // button4
            // 
            button4.Location = new Point(136, 279);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 3;
            button4.Text = "회원탈퇴";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // btnRe
            // 
            btnRe.Location = new Point(23, 250);
            btnRe.Name = "btnRe";
            btnRe.Size = new Size(301, 23);
            btnRe.TabIndex = 3;
            btnRe.Text = "수정하기";
            btnRe.UseVisualStyleBackColor = true;
            btnRe.Click += btnRe_Click;
            // 
            // txtReemail
            // 
            txtReemail.Location = new Point(23, 209);
            txtReemail.Name = "txtReemail";
            txtReemail.Size = new Size(301, 23);
            txtReemail.TabIndex = 2;
            // 
            // txtRename
            // 
            txtRename.Location = new Point(23, 157);
            txtRename.Name = "txtRename";
            txtRename.Size = new Size(301, 23);
            txtRename.TabIndex = 2;
            // 
            // txtPw
            // 
            txtPw.Location = new Point(23, 53);
            txtPw.Name = "txtPw";
            txtPw.Size = new Size(301, 23);
            txtPw.TabIndex = 2;
            // 
            // txtRepw
            // 
            txtRepw.Location = new Point(23, 105);
            txtRepw.Name = "txtRepw";
            txtRepw.Size = new Size(301, 23);
            txtRepw.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(23, 31);
            label6.Name = "label6";
            label6.Size = new Size(111, 15);
            label6.TabIndex = 1;
            label6.Text = "기존 비밀번호 입력";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(37, 98);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(605, 493);
            tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(597, 465);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(597, 465);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(597, 465);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // Mypage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1077, 603);
            Controls.Add(groupBox1);
            Controls.Add(tabControl1);
            Controls.Add(label1);
            Name = "Mypage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mypage";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label3;
        private Label label4;
        private Label label5;
        private GroupBox groupBox1;
        private Button button4;
        private Button btnReemail;
        private Button btnRename;
        private Button btnRepw;
        private TextBox txtReemail;
        private TextBox txtRename;
        private TextBox txtRepw;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Button btnRe;
        private TextBox txtPw;
        private Label label6;
    }
}