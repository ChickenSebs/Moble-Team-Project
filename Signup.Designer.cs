namespace tap
{
    partial class Signup
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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btnSignup = new Button();
            txtSignupId = new TextBox();
            txtSignupPassword = new TextBox();
            txtPasswordCheck = new TextBox();
            txtName = new TextBox();
            txtEmail = new TextBox();
            btnBack = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("한컴 말랑말랑 Bold", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(355, 45);
            label1.Name = "label1";
            label1.Size = new Size(97, 31);
            label1.TabIndex = 0;
            label1.Text = "회원가입";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("한컴산뜻돋움", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(46, 97);
            label2.Name = "label2";
            label2.Size = new Size(55, 21);
            label2.TabIndex = 1;
            label2.Text = "아이디";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("한컴산뜻돋움", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(46, 155);
            label3.Name = "label3";
            label3.Size = new Size(70, 21);
            label3.TabIndex = 2;
            label3.Text = "비밀번호";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("한컴산뜻돋움", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(46, 213);
            label4.Name = "label4";
            label4.Size = new Size(104, 21);
            label4.TabIndex = 3;
            label4.Text = "비밀번호 확인";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("한컴산뜻돋움", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(46, 271);
            label5.Name = "label5";
            label5.Size = new Size(40, 21);
            label5.TabIndex = 4;
            label5.Text = "이름";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("한컴산뜻돋움", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(46, 329);
            label6.Name = "label6";
            label6.Size = new Size(60, 21);
            label6.TabIndex = 5;
            label6.Text = "E-mail";
            // 
            // btnSignup
            // 
            btnSignup.Font = new Font("한컴산뜻돋움", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnSignup.Location = new Point(168, 393);
            btnSignup.Name = "btnSignup";
            btnSignup.Size = new Size(95, 26);
            btnSignup.TabIndex = 7;
            btnSignup.Text = "회원가입";
            btnSignup.UseVisualStyleBackColor = true;
            btnSignup.Click += btnSignup_Click;
            // 
            // txtSignupId
            // 
            txtSignupId.Location = new Point(168, 98);
            txtSignupId.Name = "txtSignupId";
            txtSignupId.Size = new Size(137, 23);
            txtSignupId.TabIndex = 8;
            // 
            // txtSignupPassword
            // 
            txtSignupPassword.Location = new Point(168, 156);
            txtSignupPassword.Name = "txtSignupPassword";
            txtSignupPassword.Size = new Size(137, 23);
            txtSignupPassword.TabIndex = 9;
            // 
            // txtPasswordCheck
            // 
            txtPasswordCheck.Location = new Point(168, 214);
            txtPasswordCheck.Name = "txtPasswordCheck";
            txtPasswordCheck.Size = new Size(137, 23);
            txtPasswordCheck.TabIndex = 10;
            // 
            // txtName
            // 
            txtName.Location = new Point(168, 272);
            txtName.Name = "txtName";
            txtName.Size = new Size(137, 23);
            txtName.TabIndex = 11;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(168, 330);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(137, 23);
            txtEmail.TabIndex = 12;
            // 
            // btnBack
            // 
            btnBack.Font = new Font("한컴산뜻돋움", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnBack.Location = new Point(286, 393);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(95, 26);
            btnBack.TabIndex = 7;
            btnBack.Text = "돌아가기";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // Signup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtEmail);
            Controls.Add(txtName);
            Controls.Add(txtPasswordCheck);
            Controls.Add(txtSignupPassword);
            Controls.Add(txtSignupId);
            Controls.Add(btnBack);
            Controls.Add(btnSignup);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Signup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Signup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button btnSignup;
        private TextBox txtSignupId;
        private TextBox txtSignupPassword;
        private TextBox txtPasswordCheck;
        private TextBox txtName;
        private TextBox txtEmail;
        private Button btnBack;
    }
}