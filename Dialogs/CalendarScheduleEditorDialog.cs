namespace calendar4;

internal sealed class CalendarScheduleEditorDialog : Form
{
    private readonly CalendarScheduleEntry? existing;
    private readonly TextBox textBox;
    private readonly ComboBox startBox;
    private readonly ComboBox endBox;
    private readonly ComboBox categoryBox;
    private readonly Panel colorPreview;
    private readonly CheckBox highPriorityCheckBox;
    private readonly ComboBox alarmBox;
    private int? customColorArgb;

    private static readonly int[] AlarmOffsets = { 0, 5, 10, 30, 60, 120 };

    public CalendarScheduleEditorDialog(DateTime date, CalendarScheduleEntry? schedule = null)
    {
        existing = schedule;
        customColorArgb = schedule?.CustomColorArgb;

        Text = schedule is null ? "일정 추가" : "일정 수정";
        ClientSize = new Size(430, 408);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        Controls.Add(new Label
        {
            Text = $"{date:yyyy년 M월 d일}",
            Font = new Font("맑은 고딕", 14F, FontStyle.Bold),
            Location = new Point(24, 18),
            AutoSize = true
        });

        AddLabel("일정 내용", 74);
        textBox = new TextBox
        {
            Location = new Point(118, 70),
            Size = new Size(284, 23),
            Text = schedule?.Text ?? string.Empty
        };

        AddLabel("시간", 116);
        startBox = new ComboBox
        {
            Location = new Point(118, 112),
            Size = new Size(105, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        endBox = new ComboBox
        {
            Location = new Point(260, 112),
            Size = new Size(105, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        Controls.Add(new Label { Text = "~", Location = new Point(239, 116), AutoSize = true });
        for (var hour = 8; hour < 22; hour++)
            startBox.Items.Add($"{hour:00}:00");
        for (var hour = 9; hour <= 22; hour++)
            endBox.Items.Add($"{hour:00}:00");
        startBox.SelectedIndex = Math.Clamp((schedule?.StartHour ?? 9) - 8, 0, startBox.Items.Count - 1);
        endBox.SelectedIndex = Math.Clamp((schedule?.EndHour ?? 10) - 9, 0, endBox.Items.Count - 1);

        AddLabel("카테고리", 158);
        categoryBox = new ComboBox
        {
            Location = new Point(118, 154),
            Size = new Size(247, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        LoadCategories(schedule?.CategoryId);
        categoryBox.SelectedIndexChanged += (_, _) => RefreshColorPreview();

        AddLabel("일정 색상", 202);
        colorPreview = new Panel
        {
            Location = new Point(118, 196),
            Size = new Size(62, 30),
            BorderStyle = BorderStyle.FixedSingle
        };
        var chooseColorButton = new Button
        {
            Text = "직접 선택",
            Location = new Point(190, 196),
            Size = new Size(100, 30)
        };
        chooseColorButton.Click += (_, _) => ChooseCustomColor();
        var categoryColorButton = new Button
        {
            Text = "기본색 사용",
            Location = new Point(300, 196),
            Size = new Size(102, 30)
        };
        categoryColorButton.Click += (_, _) =>
        {
            customColorArgb = null;
            RefreshColorPreview();
        };

        var settingsButton = new Button
        {
            Text = "카테고리 관리",
            Location = new Point(118, 236),
            Size = new Size(145, 30)
        };
        settingsButton.Click += (_, _) => EditCategories();

        highPriorityCheckBox = new CheckBox
        {
            Text = "중요 일정",
            Location = new Point(282, 240),
            Size = new Size(120, 24),
            Checked = schedule?.IsHighPriority ?? false,
            AutoSize = false
        };

        AddLabel("알림", 284);
        alarmBox = new ComboBox
        {
            Location = new Point(118, 280),
            Size = new Size(247, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        alarmBox.Items.AddRange(new object[]
        {
            "알림 없음",
            "5분 전",
            "10분 전",
            "30분 전",
            "1시간 전",
            "2시간 전"
        });
        int alarmIndex = Array.IndexOf(AlarmOffsets, schedule?.NotificationOffset ?? 0);
        alarmBox.SelectedIndex = alarmIndex >= 0 ? alarmIndex : 0;

        var saveButton = new Button
        {
            Text = "저장",
            Location = new Point(214, 348),
            Size = new Size(88, 34),
            BackColor = Color.FromArgb(79, 107, 237),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        saveButton.Click += (_, _) => SaveSchedule();
        var cancelButton = new Button
        {
            Text = "취소",
            Location = new Point(314, 348),
            Size = new Size(88, 34),
            DialogResult = DialogResult.Cancel
        };

        Controls.AddRange(new Control[]
        {
            textBox, startBox, endBox, categoryBox, colorPreview,
            chooseColorButton, categoryColorButton, settingsButton, highPriorityCheckBox,
            alarmBox, saveButton, cancelButton
        });
        AcceptButton = saveButton;
        CancelButton = cancelButton;
        RefreshColorPreview();
    }

    public CalendarScheduleEntry Schedule { get; private set; } = null!;

    private void LoadCategories(string? selectedId)
    {
        categoryBox.BeginUpdate();
        categoryBox.Items.Clear();
        foreach (var category in PersonalCategoryStores.Calendar.Categories)
            categoryBox.Items.Add(category);
        categoryBox.EndUpdate();

        var targetId = selectedId ?? UserCategoryStore.HomeId;
        for (var i = 0; i < categoryBox.Items.Count; i++)
        {
            if (((UserCategory)categoryBox.Items[i]).Id != targetId)
                continue;

            categoryBox.SelectedIndex = i;
            return;
        }

        if (categoryBox.Items.Count > 0)
            categoryBox.SelectedIndex = 0;
    }

    private void AddLabel(string text, int top)
    {
        Controls.Add(new Label { Text = text, Location = new Point(24, top), AutoSize = true });
    }

    private void EditCategories()
    {
        var selectedId = (categoryBox.SelectedItem as UserCategory)?.Id;
        using var dialog = new UserCategoryManagerDialog(
            "개인 캘린더 카테고리 관리",
            PersonalCategoryStores.Calendar);
        if (dialog.ShowDialog(this) == DialogResult.OK)
            LoadCategories(selectedId);
    }

    private void ChooseCustomColor()
    {
        var categoryId = (categoryBox.SelectedItem as UserCategory)?.Id ?? UserCategoryStore.HomeId;
        using var picker = new ColorDialog
        {
            Color = PersonalCategoryStores.Calendar.GetScheduleAccentColor(categoryId, customColorArgb),
            FullOpen = true
        };
        if (picker.ShowDialog(this) != DialogResult.OK)
            return;

        customColorArgb = picker.Color.ToArgb();
        RefreshColorPreview();
    }

    private void RefreshColorPreview()
    {
        if (categoryBox.SelectedIndex < 0)
            return;

        colorPreview.BackColor = PersonalCategoryStores.Calendar.GetScheduleAccentColor(
            (categoryBox.SelectedItem as UserCategory)?.Id,
            customColorArgb);
    }

    private void SaveSchedule()
    {
        var text = textBox.Text.Trim();
        if (text.Length == 0)
        {
            MessageBox.Show(
                "일정 내용을 입력해 주세요.",
                "입력 확인",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            textBox.Focus();
            return;
        }

        var startHour = 8 + startBox.SelectedIndex;
        var endHour = 9 + endBox.SelectedIndex;
        if (endHour <= startHour)
        {
            MessageBox.Show(
                "종료 시간은 시작 시간보다 늦어야 합니다.",
                "시간 확인",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        Schedule = new CalendarScheduleEntry
        {
            Id = existing?.Id ?? Guid.NewGuid(),
            Text = text,
            StartHour = startHour,
            EndHour = endHour,
            CategoryId = (categoryBox.SelectedItem as UserCategory)?.Id ?? UserCategoryStore.HomeId,
            CustomColorArgb = customColorArgb,
            IsHighPriority = highPriorityCheckBox.Checked,
            NotificationOffset = AlarmOffsets[alarmBox.SelectedIndex]
        };
        DialogResult = DialogResult.OK;
        Close();
    }
}
