namespace calendar4;

internal sealed class CalendarScheduleListDialog : Form
{
    private readonly DateTime date;
    private readonly ListBox scheduleList;
    private readonly List<CalendarScheduleEntry> schedules;

    public CalendarScheduleListDialog(DateTime date, IEnumerable<CalendarScheduleEntry> existing)
    {
        this.date = date.Date;
        schedules = existing.Select(item => item.Copy()).ToList();

        Text = $"{date:yyyy-MM-dd} 일정 관리";
        ClientSize = new Size(540, 390);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        var title = new Label
        {
            Text = $"{date:M월 d일} 일정",
            Font = new Font("맑은 고딕", 15F, FontStyle.Bold),
            Location = new Point(22, 18),
            AutoSize = true
        };

        scheduleList = new ListBox
        {
            Location = new Point(22, 58),
            Size = new Size(496, 232),
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 36
        };
        scheduleList.DrawItem += DrawScheduleItem;
        scheduleList.DoubleClick += (_, _) => EditSelected();

        var addButton = CreateButton("새 일정", 22, AddSchedule);
        var editButton = CreateButton("수정", 122, EditSelected);
        var deleteButton = CreateButton("삭제", 222, DeleteSelected);
        var categoriesButton = CreateButton("카테고리 관리", 322, EditCategories, 118);

        var doneButton = new Button
        {
            Text = "완료",
            Location = new Point(440, 330),
            Size = new Size(78, 34),
            DialogResult = DialogResult.OK,
            BackColor = Color.FromArgb(79, 107, 237),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        Controls.AddRange(new Control[]
        {
            title, scheduleList, addButton, editButton, deleteButton, categoriesButton, doneButton
        });
        AcceptButton = doneButton;
        RefreshList();
    }

    public List<CalendarScheduleEntry> Schedules => schedules
        .OrderBy(item => item.StartHour)
        .ThenBy(item => item.EndHour)
        .Select(item => item.Copy())
        .ToList();

    private Button CreateButton(string text, int left, Action action, int width = 88)
    {
        var button = new Button
        {
            Text = text,
            Location = new Point(left, 330),
            Size = new Size(width, 34)
        };
        button.Click += (_, _) => action();
        return button;
    }

    private void AddSchedule()
    {
        using var editor = new CalendarScheduleEditorDialog(date);
        if (editor.ShowDialog(this) != DialogResult.OK)
            return;

        schedules.Add(editor.Schedule);
        RefreshList(editor.Schedule.Id);
    }

    private void EditSelected()
    {
        if (scheduleList.SelectedItem is not CalendarScheduleEntry selected)
            return;

        using var editor = new CalendarScheduleEditorDialog(date, selected);
        if (editor.ShowDialog(this) != DialogResult.OK)
            return;

        var index = schedules.FindIndex(item => item.Id == selected.Id);
        if (index >= 0)
            schedules[index] = editor.Schedule;
        RefreshList(editor.Schedule.Id);
    }

    private void DeleteSelected()
    {
        if (scheduleList.SelectedItem is not CalendarScheduleEntry selected)
            return;

        if (MessageBox.Show(
                $"'{selected.Text}' 일정을 삭제하시겠습니까?",
                "일정 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        schedules.RemoveAll(item => item.Id == selected.Id);
        RefreshList();
    }

    private void EditCategories()
    {
        using var dialog = new UserCategoryManagerDialog(
            "개인 캘린더 카테고리 관리",
            PersonalCategoryStores.Calendar);
        if (dialog.ShowDialog(this) == DialogResult.OK)
            scheduleList.Invalidate();
    }

    private void RefreshList(Guid? selectedId = null)
    {
        scheduleList.BeginUpdate();
        scheduleList.Items.Clear();
        foreach (var schedule in schedules.OrderBy(item => item.StartHour).ThenBy(item => item.EndHour))
            scheduleList.Items.Add(schedule);
        scheduleList.EndUpdate();

        if (!selectedId.HasValue)
            return;

        for (var i = 0; i < scheduleList.Items.Count; i++)
        {
            if (((CalendarScheduleEntry)scheduleList.Items[i]).Id != selectedId.Value)
                continue;

            scheduleList.SelectedIndex = i;
            break;
        }
    }

    private void DrawScheduleItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0)
            return;

        e.DrawBackground();
        var schedule = (CalendarScheduleEntry)scheduleList.Items[e.Index];
        var accent = PersonalCategoryStores.Calendar.GetScheduleAccentColor(
            schedule.CategoryId,
            schedule.CustomColorArgb);
        using var brush = new SolidBrush(accent);
        e.Graphics.FillRectangle(brush, e.Bounds.Left + 5, e.Bounds.Top + 5, 8, e.Bounds.Height - 10);

        var text = $"{schedule.StartHour:00}:00~{schedule.EndHour:00}:00   {schedule.Text}   " +
                   $"[{PersonalCategoryStores.Calendar.Get(schedule.CategoryId).Name}]";
        TextRenderer.DrawText(
            e.Graphics,
            text,
            scheduleList.Font,
            new Rectangle(e.Bounds.Left + 21, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height),
            e.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }
}
