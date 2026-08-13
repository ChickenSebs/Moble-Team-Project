namespace tap
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtLoginId = new TextBox();
            txtPassword = new TextBox();
            chkRememberId = new CheckBox();
            btnLogin = new Button();
            btnSignup = new Button();
            btnHello = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("한컴 말랑말랑 Bold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(340, 30);
            label1.Name = "label1";
            label1.Size = new Size(102, 31);
            label1.TabIndex = 0;
            label1.Text = "제목 미정";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("한컴산뜻돋움", 14.2499981F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(29, 109);
            label2.Name = "label2";
            label2.Size = new Size(66, 25);
            label2.TabIndex = 1;
            label2.Text = "아이디";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("한컴산뜻돋움", 14.2499981F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(29, 171);
            label3.Name = "label3";
            label3.Size = new Size(84, 25);
            label3.TabIndex = 2;
            label3.Text = "비밀번호";
            // 
            // txtLoginId
            // 
            txtLoginId.Location = new Point(139, 109);
            txtLoginId.Name = "txtLoginId";
            txtLoginId.Size = new Size(203, 23);
            txtLoginId.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(139, 171);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(203, 23);
            txtPassword.TabIndex = 5;
            // 
            // chkRememberId
            // 
            chkRememberId.AutoSize = true;
            chkRememberId.Font = new Font("한컴산뜻돋움", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            chkRememberId.Location = new Point(39, 233);
            chkRememberId.Name = "chkRememberId";
            chkRememberId.Size = new Size(144, 21);
            chkRememberId.TabIndex = 6;
            chkRememberId.Text = "아이디/비밀번호 저장";
            chkRememberId.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("한컴산뜻돋움", 14.2499981F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogin.Location = new Point(86, 322);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(112, 34);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "로그인";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSignup
            // 
            btnSignup.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnSignup.Location = new Point(293, 322);
            btnSignup.Name = "btnSignup";
            btnSignup.Size = new Size(112, 32);
            btnSignup.TabIndex = 8;
            btnSignup.Text = "회원가입";
            btnSignup.UseVisualStyleBackColor = true;
            btnSignup.Click += btnSignup_Click;
            // 
            // btnHello
            // 
            btnHello.Location = new Point(560, 132);
            btnHello.Name = "btnHello";
            btnHello.Size = new Size(75, 23);
            btnHello.TabIndex = 9;
            btnHello.Text = "Hello";
            btnHello.UseVisualStyleBackColor = true;
            btnHello.Click += btnHello_Click;
            // 
            // Login
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnHello);
            Controls.Add(btnSignup);
            Controls.Add(btnLogin);
            Controls.Add(chkRememberId);
            Controls.Add(txtPassword);
            Controls.Add(txtLoginId);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            FormClosing += Login_FormClosing;
            Load += Login_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtLoginId;
        private TextBox txtPassword;
        private CheckBox chkRememberId;
        private Button btnLogin;
        private Button btnSignup;
        private Button btnHello;
    }
}