namespace calendar4
{
    partial class DdaySettingForm
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
            lblFormTitle = new Label();
            lblSchedule = new Label();
            lstSchedules = new ListBox();
            lblSelected = new Label();
            btnNewDday = new Button();
            grpCountMode = new GroupBox();
            rdoOne = new RadioButton();
            rdoZero = new RadioButton();
            btnSave = new Button();
            btnCancel = new Button();
            grpCountMode.SuspendLayout();
            SuspendLayout();
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblFormTitle.Location = new Point(20, 20);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(142, 32);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "D-Day 설정";
            // 
            // lblSchedule
            // 
            lblSchedule.AutoSize = true;
            lblSchedule.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblSchedule.Location = new Point(20, 66);
            lblSchedule.Name = "lblSchedule";
            lblSchedule.Size = new Size(188, 21);
            lblSchedule.TabIndex = 1;
            lblSchedule.Text = "내 캘린더 일정에서 선택";
            // 
            // lstSchedules
            // 
            lstSchedules.FormattingEnabled = true;
            lstSchedules.ItemHeight = 15;
            lstSchedules.Location = new Point(20, 90);
            lstSchedules.Name = "lstSchedules";
            lstSchedules.Size = new Size(440, 169);
            lstSchedules.TabIndex = 2;
            lstSchedules.SelectedIndexChanged += lstSchedules_SelectedIndexChanged;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Location = new Point(20, 285);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(138, 15);
            lblSelected.TabIndex = 3;
            lblSelected.Text = "선택된 일정이 없습니다.";
            // 
            // btnNewDday
            // 
            btnNewDday.Location = new Point(20, 320);
            btnNewDday.Name = "btnNewDday";
            btnNewDday.Size = new Size(112, 23);
            btnNewDday.TabIndex = 4;
            btnNewDday.Text = "새 D-Day 만들기";
            btnNewDday.UseVisualStyleBackColor = true;
            btnNewDday.Click += btnNewDday_Click;
            // 
            // grpCountMode
            // 
            grpCountMode.Controls.Add(rdoOne);
            grpCountMode.Controls.Add(rdoZero);
            grpCountMode.Location = new Point(20, 349);
            grpCountMode.Name = "grpCountMode";
            grpCountMode.Size = new Size(440, 55);
            grpCountMode.TabIndex = 5;
            grpCountMode.TabStop = false;
            grpCountMode.Text = "카운트 방식";
            // 
            // rdoOne
            // 
            rdoOne.AutoSize = true;
            rdoOne.Location = new Point(259, 22);
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
            rdoZero.Location = new Point(47, 22);
            rdoZero.Name = "rdoZero";
            rdoZero.Size = new Size(96, 19);
            rdoZero.TabIndex = 0;
            rdoZero.TabStop = true;
            rdoZero.Text = "0일부터 시작";
            rdoZero.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(262, 410);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(96, 32);
            btnSave.TabIndex = 6;
            btnSave.Text = "설정";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(364, 410);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(96, 32);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "취소";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // DdaySettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpCountMode);
            Controls.Add(btnNewDday);
            Controls.Add(lblSelected);
            Controls.Add(lstSchedules);
            Controls.Add(lblSchedule);
            Controls.Add(lblFormTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DdaySettingForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DdaySettingForm";
            grpCountMode.ResumeLayout(false);
            grpCountMode.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFormTitle;
        private Label lblSchedule;
        private ListBox lstSchedules;
        private Label lblSelected;
        private Button btnNewDday;
        private GroupBox grpCountMode;
        private RadioButton rdoOne;
        private RadioButton rdoZero;
        private Button btnSave;
        private Button btnCancel;
    }
}