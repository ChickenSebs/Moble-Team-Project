namespace calendar4;

internal partial class ClassScheduleDialog : Form
{
    private readonly ClassSchedule? existingSchedule;
    private readonly Panel colorPreview = new();
    private int? customColorArgb;

    public ClassScheduleDialog(ClassSchedule? schedule = null)
    {
        InitializeComponent();
        existingSchedule = schedule;

        InitializeOptions();
        InitializeColorControls();
        if (schedule is null)
        {
            cboDay.SelectedIndex = 0;
            cboStartTime.SelectedIndex = 0;
            cboEndTime.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            btnDelete.Visible = false;
        }
        else
        {
            Text = "수업 수정";
            lblDialogTitle.Text = "수업 수정";
            LoadSchedule(schedule);
        }
    }

    public ClassSchedule Schedule { get; private set; } = null!;

    private void InitializeOptions()
    {
        cboDay.Items.AddRange(new object[] { "월요일", "화요일", "수요일", "목요일", "금요일" });

        for (var hour = 9; hour <= 17; hour++)
            cboStartTime.Items.Add($"{hour:00}:00");

        for (var hour = 10; hour <= 18; hour++)
            cboEndTime.Items.Add($"{hour:00}:00");

        cboCategory.Items.AddRange(new object[] { "전공", "교양", "기타" });
    }

    private void LoadSchedule(ClassSchedule schedule)
    {
        txtSubjectName.Text = schedule.SubjectName;
        txtClassroom.Text = schedule.Classroom;
        cboDay.SelectedIndex = (int)schedule.Day - 1;
        cboStartTime.SelectedItem = $"{schedule.StartHour:00}:00";
        cboEndTime.SelectedItem = $"{schedule.EndHour:00}:00";
        cboCategory.SelectedIndex = (int)schedule.Category;
        customColorArgb = schedule.CustomColorArgb;
        RefreshColorPreview();
    }

    private void InitializeColorControls()
    {
        ClientSize = new Size(410, 420);
        btnDelete.Top = 359;
        btnSave.Top = 359;
        btnCancel.Top = 359;

        var colorLabel = new Label
        {
            Text = "일정 색상",
            Location = new Point(30, 279),
            AutoSize = true
        };

        colorPreview.Location = new Point(122, 273);
        colorPreview.Size = new Size(55, 30);
        colorPreview.BorderStyle = BorderStyle.FixedSingle;

        var chooseButton = new Button
        {
            Text = "개별색 선택",
            Location = new Point(186, 273),
            Size = new Size(91, 30)
        };
        chooseButton.Click += (_, _) =>
        {
            var category = (ScheduleCategory)Math.Max(cboCategory.SelectedIndex, 0);
            using var picker = new ColorDialog
            {
                Color = ScheduleColorService.GetScheduleAccentColor(category, customColorArgb),
                FullOpen = true
            };
            if (picker.ShowDialog(this) == DialogResult.OK)
            {
                customColorArgb = picker.Color.ToArgb();
                RefreshColorPreview();
            }
        };

        var useCategoryButton = new Button
        {
            Text = "기본색 사용",
            Location = new Point(285, 273),
            Size = new Size(91, 30)
        };
        useCategoryButton.Click += (_, _) =>
        {
            customColorArgb = null;
            RefreshColorPreview();
        };

        var settingsButton = new Button
        {
            Text = "카테고리 색상 설정",
            Location = new Point(122, 313),
            Size = new Size(145, 30)
        };
        settingsButton.Click += (_, _) =>
        {
            using var dialog = new ScheduleColorSettingsDialog();
            dialog.ShowDialog(this);
            RefreshColorPreview();
        };

        cboCategory.SelectedIndexChanged += (_, _) => RefreshColorPreview();
        Controls.AddRange(new Control[]
        {
            colorLabel, colorPreview, chooseButton, useCategoryButton, settingsButton
        });
        RefreshColorPreview();
    }

    private void RefreshColorPreview()
    {
        if (cboCategory.SelectedIndex < 0)
            return;

        colorPreview.BackColor = ScheduleColorService.GetScheduleAccentColor(
            (ScheduleCategory)cboCategory.SelectedIndex,
            customColorArgb);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var subjectName = txtSubjectName.Text.Trim();
        if (subjectName.Length == 0)
        {
            MessageBox.Show("과목명을 입력해 주세요.", "입력 확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSubjectName.Focus();
            return;
        }

        if (cboDay.SelectedIndex < 0 || cboStartTime.SelectedIndex < 0 ||
            cboEndTime.SelectedIndex < 0 || cboCategory.SelectedIndex < 0)
        {
            MessageBox.Show("요일, 시간, 구분을 모두 선택해 주세요.", "입력 확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var startHour = 9 + cboStartTime.SelectedIndex;
        var endHour = 10 + cboEndTime.SelectedIndex;
        if (endHour <= startHour)
        {
            MessageBox.Show("종료 시간은 시작 시간보다 늦어야 합니다.", "시간 확인",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        Schedule = new ClassSchedule
        {
            Id = existingSchedule?.Id ?? Guid.NewGuid(),
            SubjectName = subjectName,
            Classroom = txtClassroom.Text.Trim(),
            Day = (DayOfWeek)(cboDay.SelectedIndex + 1),
            StartHour = startHour,
            EndHour = endHour,
            Category = (ScheduleCategory)cboCategory.SelectedIndex,
            CustomColorArgb = customColorArgb
        };

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        var answer = MessageBox.Show(
            "이 수업을 시간표에서 삭제할까요?",
            "수업 삭제",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (answer == DialogResult.Yes)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
