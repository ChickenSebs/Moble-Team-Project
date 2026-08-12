namespace calendar4;

partial class ClassScheduleDialog
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblDialogTitle = new Label();
        lblSubjectName = new Label();
        txtSubjectName = new TextBox();
        lblClassroom = new Label();
        txtClassroom = new TextBox();
        lblDay = new Label();
        cboDay = new ComboBox();
        lblStartTime = new Label();
        cboStartTime = new ComboBox();
        lblEndTime = new Label();
        cboEndTime = new ComboBox();
        lblCategory = new Label();
        cboCategory = new ComboBox();
        btnSave = new Button();
        btnDelete = new Button();
        btnCancel = new Button();
        SuspendLayout();
        // 
        // lblDialogTitle
        // 
        lblDialogTitle.AutoSize = true;
        lblDialogTitle.Font = new Font("맑은 고딕", 16F, FontStyle.Bold);
        lblDialogTitle.ForeColor = Color.FromArgb(31, 42, 68);
        lblDialogTitle.Location = new Point(28, 22);
        lblDialogTitle.Name = "lblDialogTitle";
        lblDialogTitle.Size = new Size(97, 30);
        lblDialogTitle.TabIndex = 0;
        lblDialogTitle.Text = "수업 추가";
        // 
        // lblSubjectName
        // 
        lblSubjectName.AutoSize = true;
        lblSubjectName.Location = new Point(30, 79);
        lblSubjectName.Name = "lblSubjectName";
        lblSubjectName.Size = new Size(43, 15);
        lblSubjectName.TabIndex = 1;
        lblSubjectName.Text = "과목명";
        // 
        // txtSubjectName
        // 
        txtSubjectName.Location = new Point(122, 75);
        txtSubjectName.Name = "txtSubjectName";
        txtSubjectName.PlaceholderText = "예: C# 프로그래밍";
        txtSubjectName.Size = new Size(254, 23);
        txtSubjectName.TabIndex = 2;
        // 
        // lblClassroom
        // 
        lblClassroom.AutoSize = true;
        lblClassroom.Location = new Point(30, 119);
        lblClassroom.Name = "lblClassroom";
        lblClassroom.Size = new Size(43, 15);
        lblClassroom.TabIndex = 3;
        lblClassroom.Text = "강의실";
        // 
        // txtClassroom
        // 
        txtClassroom.Location = new Point(122, 115);
        txtClassroom.Name = "txtClassroom";
        txtClassroom.PlaceholderText = "예: 공학관 301호";
        txtClassroom.Size = new Size(254, 23);
        txtClassroom.TabIndex = 4;
        // 
        // lblDay
        // 
        lblDay.AutoSize = true;
        lblDay.Location = new Point(30, 159);
        lblDay.Name = "lblDay";
        lblDay.Size = new Size(31, 15);
        lblDay.TabIndex = 5;
        lblDay.Text = "요일";
        // 
        // cboDay
        // 
        cboDay.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDay.FormattingEnabled = true;
        cboDay.Location = new Point(122, 155);
        cboDay.Name = "cboDay";
        cboDay.Size = new Size(254, 23);
        cboDay.TabIndex = 6;
        // 
        // lblStartTime
        // 
        lblStartTime.AutoSize = true;
        lblStartTime.Location = new Point(30, 199);
        lblStartTime.Name = "lblStartTime";
        lblStartTime.Size = new Size(55, 15);
        lblStartTime.TabIndex = 7;
        lblStartTime.Text = "시작 시간";
        // 
        // cboStartTime
        // 
        cboStartTime.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStartTime.FormattingEnabled = true;
        cboStartTime.Location = new Point(122, 195);
        cboStartTime.Name = "cboStartTime";
        cboStartTime.Size = new Size(108, 23);
        cboStartTime.TabIndex = 8;
        // 
        // lblEndTime
        // 
        lblEndTime.AutoSize = true;
        lblEndTime.Location = new Point(247, 199);
        lblEndTime.Name = "lblEndTime";
        lblEndTime.Size = new Size(31, 15);
        lblEndTime.TabIndex = 9;
        lblEndTime.Text = "종료";
        // 
        // cboEndTime
        // 
        cboEndTime.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEndTime.FormattingEnabled = true;
        cboEndTime.Location = new Point(284, 195);
        cboEndTime.Name = "cboEndTime";
        cboEndTime.Size = new Size(92, 23);
        cboEndTime.TabIndex = 10;
        // 
        // lblCategory
        // 
        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(30, 239);
        lblCategory.Name = "lblCategory";
        lblCategory.Size = new Size(55, 15);
        lblCategory.TabIndex = 11;
        lblCategory.Text = "과목 구분";
        // 
        // cboCategory
        // 
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCategory.FormattingEnabled = true;
        cboCategory.Location = new Point(122, 235);
        cboCategory.Name = "cboCategory";
        cboCategory.Size = new Size(254, 23);
        cboCategory.TabIndex = 12;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(79, 107, 237);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(166, 294);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(100, 36);
        btnSave.TabIndex = 13;
        btnSave.Text = "저장";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += BtnSave_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.White;
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(220, 80, 80);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.ForeColor = Color.FromArgb(190, 55, 55);
        btnDelete.Location = new Point(28, 294);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(86, 36);
        btnDelete.TabIndex = 14;
        btnDelete.Text = "삭제";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += BtnDelete_Click;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(276, 294);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 36);
        btnCancel.TabIndex = 15;
        btnCancel.Text = "취소";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // ClassScheduleDialog
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        CancelButton = btnCancel;
        ClientSize = new Size(410, 355);
        Controls.Add(btnCancel);
        Controls.Add(btnDelete);
        Controls.Add(btnSave);
        Controls.Add(cboCategory);
        Controls.Add(lblCategory);
        Controls.Add(cboEndTime);
        Controls.Add(lblEndTime);
        Controls.Add(cboStartTime);
        Controls.Add(lblStartTime);
        Controls.Add(cboDay);
        Controls.Add(lblDay);
        Controls.Add(txtClassroom);
        Controls.Add(lblClassroom);
        Controls.Add(txtSubjectName);
        Controls.Add(lblSubjectName);
        Controls.Add(lblDialogTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ClassScheduleDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "수업 추가";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblDialogTitle;
    private Label lblSubjectName;
    private TextBox txtSubjectName;
    private Label lblClassroom;
    private TextBox txtClassroom;
    private Label lblDay;
    private ComboBox cboDay;
    private Label lblStartTime;
    private ComboBox cboStartTime;
    private Label lblEndTime;
    private ComboBox cboEndTime;
    private Label lblCategory;
    private ComboBox cboCategory;
    private Button btnSave;
    private Button btnDelete;
    private Button btnCancel;
}
