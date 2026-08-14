namespace calendar4
{
    partial class premium
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(premium));
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            cb_pay = new ComboBox();
            btn_pay = new Button();
            btn_out = new Button();
            cb_select = new ComboBox();
            label1 = new Label();
            label5 = new Label();
            label6 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            label7 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(46, 247);
            label2.Name = "label2";
            label2.Size = new Size(340, 17);
            label2.TabIndex = 1;
            label2.Text = "작업 흐름을 방해하는 모든 광고가 깔끔하게 제거됩니다.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(146, 210);
            label3.Name = "label3";
            label3.Size = new Size(140, 17);
            label3.TabIndex = 2;
            label3.Text = "광고 없이 쾌적한 환경";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(85, 410);
            label4.Name = "label4";
            label4.Size = new Size(80, 21);
            label4.TabIndex = 3;
            label4.Text = "결제 방법";
            // 
            // cb_pay
            // 
            cb_pay.FormattingEnabled = true;
            cb_pay.Location = new Point(85, 434);
            cb_pay.Name = "cb_pay";
            cb_pay.Size = new Size(121, 23);
            cb_pay.TabIndex = 4;
            cb_pay.SelectedIndexChanged += cb_pay_SelectedIndexChanged;
            // 
            // btn_pay
            // 
            btn_pay.BackColor = Color.Transparent;
            btn_pay.FlatAppearance.BorderSize = 0;
            btn_pay.FlatStyle = FlatStyle.Flat;
            btn_pay.Font = new Font("맑은 고딕", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btn_pay.Location = new Point(102, 490);
            btn_pay.Name = "btn_pay";
            btn_pay.Size = new Size(104, 33);
            btn_pay.TabIndex = 5;
            btn_pay.Text = "결제하기";
            btn_pay.UseVisualStyleBackColor = false;
            btn_pay.Click += btn_pay_Click;
            // 
            // btn_out
            // 
            btn_out.BackColor = Color.Transparent;
            btn_out.FlatAppearance.BorderSize = 0;
            btn_out.FlatStyle = FlatStyle.Flat;
            btn_out.Font = new Font("맑은 고딕", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btn_out.Location = new Point(212, 490);
            btn_out.Name = "btn_out";
            btn_out.Size = new Size(104, 33);
            btn_out.TabIndex = 6;
            btn_out.Text = "나가기";
            btn_out.UseVisualStyleBackColor = false;
            btn_out.Click += btn_out_Click;
            // 
            // cb_select
            // 
            cb_select.FormattingEnabled = true;
            cb_select.Location = new Point(212, 434);
            cb_select.Name = "cb_select";
            cb_select.Size = new Size(121, 23);
            cb_select.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(24, 160);
            label1.Name = "label1";
            label1.Size = new Size(392, 20);
            label1.TabIndex = 8;
            label1.Text = "제약 없는 완벽한 플래너 경험, Premium으로 시작하세요\r\n";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(146, 284);
            label5.Name = "label5";
            label5.Size = new Size(140, 17);
            label5.TabIndex = 9;
            label5.Text = "한계 없는 무제한 기능";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(16, 382);
            label6.Name = "label6";
            label6.Size = new Size(400, 15);
            label6.TabIndex = 10;
            label6.Text = "탭 생성, 일정·다이어리 기록 제한 없이 모든 기능을 자유롭게 이용하세요.";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(165, 323);
            label7.Name = "label7";
            label7.Size = new Size(99, 30);
            label7.TabIndex = 9;
            label7.Text = "29,900원";
            // 
            // premium
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(428, 576);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(cb_select);
            Controls.Add(btn_out);
            Controls.Add(btn_pay);
            Controls.Add(cb_pay);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            DoubleBuffered = true;
            Name = "premium";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "premium";
            Load += premium_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox cb_pay;
        private Button btn_pay;
        private Button btn_out;
        private ComboBox cb_select;
        private Label label1;
        private Label label5;
        private Label label6;
        private System.Windows.Forms.Timer timer1;
        private Label label7;
    }
}