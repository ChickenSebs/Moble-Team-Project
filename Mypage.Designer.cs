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
            btnBack = new Button();
            tabChange = new TabPage();
            groupBox1 = new GroupBox();
            btnOut = new Button();
            btnRe = new Button();
            txtReemail = new TextBox();
            txtRename = new TextBox();
            txtPw = new TextBox();
            txtRepw = new TextBox();
            label6 = new Label();
            label3 = new Label();
            label5 = new Label();
            label4 = new Label();
            tabMyinfo = new TabPage();
            groupBox3 = new GroupBox();
            lbEmail = new Label();
            lbId = new Label();
            lbName = new Label();
            label10 = new Label();
            label8 = new Label();
            label12 = new Label();
            label11 = new Label();
            label9 = new Label();
            label7 = new Label();
            groupBox2 = new GroupBox();
            label2 = new Label();
            lblPremiumStatus = new Label();
            btnPremium = new Button();
            tabMy = new TabControl();
            tabChange.SuspendLayout();
            groupBox1.SuspendLayout();
            tabMyinfo.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            tabMy.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(168, 22);
            label1.Name = "label1";
            label1.Size = new Size(180, 45);
            label1.TabIndex = 0;
            label1.Text = "마이페이지";
            // 
            // btnBack
            // 
            btnBack.Location = new Point(405, 53);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 26);
            btnBack.TabIndex = 4;
            btnBack.Text = "돌아가기";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // tabChange
            // 
            tabChange.Controls.Add(groupBox1);
            tabChange.Location = new Point(4, 24);
            tabChange.Name = "tabChange";
            tabChange.Padding = new Padding(3);
            tabChange.Size = new Size(438, 465);
            tabChange.TabIndex = 1;
            tabChange.Text = "정보수정";
            tabChange.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnOut);
            groupBox1.Controls.Add(btnRe);
            groupBox1.Controls.Add(txtReemail);
            groupBox1.Controls.Add(txtRename);
            groupBox1.Controls.Add(txtPw);
            groupBox1.Controls.Add(txtRepw);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Point(45, 56);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(342, 349);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "회원정보수정";
            // 
            // btnOut
            // 
            btnOut.Location = new Point(135, 305);
            btnOut.Name = "btnOut";
            btnOut.Size = new Size(75, 23);
            btnOut.TabIndex = 3;
            btnOut.Text = "회원탈퇴";
            btnOut.UseVisualStyleBackColor = true;
            btnOut.Click += btnOut_Click;
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
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 83);
            label3.Name = "label3";
            label3.Size = new Size(83, 15);
            label3.TabIndex = 1;
            label3.Text = "비밀번호 수정";
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
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 135);
            label4.Name = "label4";
            label4.Size = new Size(59, 15);
            label4.TabIndex = 1;
            label4.Text = "이름 수정";
            // 
            // tabMyinfo
            // 
            tabMyinfo.Controls.Add(groupBox3);
            tabMyinfo.Controls.Add(groupBox2);
            tabMyinfo.Location = new Point(4, 24);
            tabMyinfo.Name = "tabMyinfo";
            tabMyinfo.Padding = new Padding(3);
            tabMyinfo.Size = new Size(438, 465);
            tabMyinfo.TabIndex = 0;
            tabMyinfo.Text = "내정보";
            tabMyinfo.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lbEmail);
            groupBox3.Controls.Add(lbId);
            groupBox3.Controls.Add(lbName);
            groupBox3.Controls.Add(label10);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(label7);
            groupBox3.Location = new Point(58, 35);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(321, 211);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "내정보";
            // 
            // lbEmail
            // 
            lbEmail.AutoSize = true;
            lbEmail.Location = new Point(149, 160);
            lbEmail.Name = "lbEmail";
            lbEmail.Size = new Size(12, 15);
            lbEmail.TabIndex = 3;
            lbEmail.Text = "-";
            // 
            // lbId
            // 
            lbId.AutoSize = true;
            lbId.Location = new Point(149, 102);
            lbId.Name = "lbId";
            lbId.Size = new Size(12, 15);
            lbId.TabIndex = 3;
            lbId.Text = "-";
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Location = new Point(149, 44);
            lbName.Name = "lbName";
            lbName.Size = new Size(12, 15);
            lbName.TabIndex = 3;
            lbName.Text = "-";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(38, 160);
            label10.Name = "label10";
            label10.Size = new Size(36, 15);
            label10.TabIndex = 3;
            label10.Text = "Email";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(38, 102);
            label8.Name = "label8";
            label8.Size = new Size(43, 15);
            label8.TabIndex = 3;
            label8.Text = "아이디";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(89, 160);
            label12.Name = "label12";
            label12.Size = new Size(15, 15);
            label12.TabIndex = 3;
            label12.Text = "=";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(89, 102);
            label11.Name = "label11";
            label11.Size = new Size(15, 15);
            label11.TabIndex = 3;
            label11.Text = "=";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(89, 44);
            label9.Name = "label9";
            label9.Size = new Size(15, 15);
            label9.TabIndex = 3;
            label9.Text = "=";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(38, 44);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 3;
            label7.Text = "이름";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(lblPremiumStatus);
            groupBox2.Controls.Add(btnPremium);
            groupBox2.Location = new Point(58, 269);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(321, 155);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "프리미엄";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(89, 53);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 2;
            label2.Text = "요금제 :";
            // 
            // lblPremiumStatus
            // 
            lblPremiumStatus.AutoSize = true;
            lblPremiumStatus.Location = new Point(149, 53);
            lblPremiumStatus.Name = "lblPremiumStatus";
            lblPremiumStatus.Size = new Size(71, 15);
            lblPremiumStatus.TabIndex = 0;
            lblPremiumStatus.Text = "일반 사용자";
            // 
            // btnPremium
            // 
            btnPremium.Location = new Point(67, 96);
            btnPremium.Name = "btnPremium";
            btnPremium.Size = new Size(177, 38);
            btnPremium.TabIndex = 1;
            btnPremium.Text = "프리미엄 결제하기";
            btnPremium.UseVisualStyleBackColor = true;
            btnPremium.Click += btnPremium_Click;
            // 
            // tabMy
            // 
            tabMy.Controls.Add(tabMyinfo);
            tabMy.Controls.Add(tabChange);
            tabMy.Location = new Point(39, 80);
            tabMy.Name = "tabMy";
            tabMy.SelectedIndex = 0;
            tabMy.Size = new Size(446, 493);
            tabMy.TabIndex = 3;
            // 
            // Mypage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(523, 603);
            Controls.Add(btnBack);
            Controls.Add(tabMy);
            Controls.Add(label1);
            Name = "Mypage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mypage";
            FormClosed += Mypage_FormClosed;
            Load += Mypage_Load;
            tabChange.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabMyinfo.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabMy.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnReemail;
        private Button btnRename;
        private Button btnRepw;
        private Button btnBack;
        private TabPage tabChange;
        private GroupBox groupBox1;
        private Button btnOut;
        private Button btnRe;
        private TextBox txtReemail;
        private TextBox txtRename;
        private TextBox txtPw;
        private TextBox txtRepw;
        private Label label6;
        private Label label3;
        private Label label5;
        private Label label4;
        private TabPage tabMyinfo;
        private Button btnPremium;
        private Label lblPremiumStatus;
        private TabControl tabMy;
        private GroupBox groupBox3;
        private Label lbEmail;
        private Label lbId;
        private Label lbName;
        private Label label10;
        private Label label8;
        private Label label7;
        private GroupBox groupBox2;
        private Label label2;
        private Label label12;
        private Label label11;
        private Label label9;
    }
}