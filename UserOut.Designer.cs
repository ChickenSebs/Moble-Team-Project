namespace calendar4
{
    partial class UserOut
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
            txtOutpw = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(112, 21);
            label1.Name = "label1";
            label1.Size = new Size(125, 37);
            label1.TabIndex = 0;
            label1.Text = "회원탈퇴";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 84);
            label2.Name = "label2";
            label2.Size = new Size(303, 15);
            label2.TabIndex = 1;
            label2.Text = "정말 회원탈퇴 하시려면 아래에 비밀번호를 적어주세요";
            // 
            // txtOutpw
            // 
            txtOutpw.Location = new Point(26, 115);
            txtOutpw.Name = "txtOutpw";
            txtOutpw.Size = new Size(303, 23);
            txtOutpw.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(112, 153);
            button1.Name = "button1";
            button1.Size = new Size(125, 23);
            button1.TabIndex = 3;
            button1.Text = "탈퇴하기";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // UserOut
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(352, 192);
            Controls.Add(button1);
            Controls.Add(txtOutpw);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "UserOut";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UserOut";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtOutpw;
        private Button button1;
    }
}