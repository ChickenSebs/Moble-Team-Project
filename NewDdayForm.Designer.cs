namespace calendar4
{
    partial class NewDdayForm
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
            lblTitle = new Label();
            lblDate = new Label();
            dtpDdayDate = new DateTimePicker();
            lblDdayTitle = new Label();
            txtDdayTitle = new TextBox();
            grpStartType = new GroupBox();
            rdoOne = new RadioButton();
            rdoZero = new RadioButton();
            btnSave = new Button();
            btnCancel = new Button();
            grpStartType.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(139, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "새 D-Day 설정";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(35, 75);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(31, 15);
            lblDate.TabIndex = 1;
            lblDate.Text = "날짜";
            // 
            // dtpDdayDate
            // 
            dtpDdayDate.Location = new Point(100, 72);
            dtpDdayDate.Name = "dtpDdayDate";
            dtpDdayDate.Size = new Size(240, 23);
            dtpDdayDate.TabIndex = 2;
            // 
            // lblDdayTitle
            // 
            lblDdayTitle.AutoSize = true;
            lblDdayTitle.Location = new Point(35, 104);
            lblDdayTitle.Name = "lblDdayTitle";
            lblDdayTitle.Size = new Size(31, 15);
            lblDdayTitle.TabIndex = 3;
            lblDdayTitle.Text = "제목";
            // 
            // txtDdayTitle
            // 
            txtDdayTitle.Location = new Point(100, 104);
            txtDdayTitle.Name = "txtDdayTitle";
            txtDdayTitle.Size = new Size(240, 23);
            txtDdayTitle.TabIndex = 4;
            // 
            // grpStartType
            // 
            grpStartType.Controls.Add(rdoOne);
            grpStartType.Controls.Add(rdoZero);
            grpStartType.Location = new Point(35, 150);
            grpStartType.Name = "grpStartType";
            grpStartType.Size = new Size(305, 55);
            grpStartType.TabIndex = 5;
            grpStartType.TabStop = false;
            grpStartType.Text = "시작방식";
            // 
            // rdoOne
            // 
            rdoOne.AutoSize = true;
            rdoOne.Location = new Point(155, 22);
            rdoOne.Name = "rdoOne";
            rdoOne.Size = new Size(96, 19);
            rdoOne.TabIndex = 1;
            rdoOne.Text = "1일부터 시작";
            rdoOne.UseVisualStyleBackColor = true;
            // 
            // rdoZero
            // 
            rdoZero.AutoSize = true;
            rdoZero.Checked = true;
            rdoZero.Location = new Point(20, 22);
            rdoZero.Name = "rdoZero";
            rdoZero.Size = new Size(96, 19);
            rdoZero.TabIndex = 0;
            rdoZero.TabStop = true;
            rdoZero.Text = "0일부터 시작";
            rdoZero.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(150, 225);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "저장";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(250, 225);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 30);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "취소";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // NewDdayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 261);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpStartType);
            Controls.Add(txtDdayTitle);
            Controls.Add(lblDdayTitle);
            Controls.Add(dtpDdayDate);
            Controls.Add(lblDate);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewDdayForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "NewDdayForm";
            grpStartType.ResumeLayout(false);
            grpStartType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblDate;
        private DateTimePicker dtpDdayDate;
        private Label lblDdayTitle;
        private TextBox txtDdayTitle;
        private GroupBox grpStartType;
        private RadioButton rdoOne;
        private RadioButton rdoZero;
        private Button btnSave;
        private Button btnCancel;
    }
}