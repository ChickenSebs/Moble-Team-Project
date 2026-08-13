using System.ComponentModel;
using calendar4.Services;

namespace calendar4;

public partial class Timetable : UserControl
{
    private readonly List<ClassSchedule> schedules = new();
    private readonly int loggedInUserId;
    private readonly TimetableDbRepository timetableDbRepository = new();
    private readonly TimetableScheduleService scheduleService = new();

    public Timetable() : this(0)
    {
    }

    public Timetable(int userId)
    {
        InitializeComponent();

        loggedInUserId = userId;

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        RemovePreviewData();
        btnAddClass.Click += BtnAddClass_Click;
        todayClassList.SizeChanged += (_, _) => ResizeTodayClassCards();
        progressPanel.SizeChanged += (_, _) => PositionProgressCount();
        ScheduleColorService.SettingsChanged += ScheduleColorsChanged;
        Disposed += (_, _) => ScheduleColorService.SettingsChanged -= ScheduleColorsChanged;

        LoadSchedules();
        ApplyCategoryColors();
        RenderSchedules();
        PositionProgressCount();
    }

    private void BtnAddClass_Click(object? sender, EventArgs e)
    {
        using var dialog = new ClassScheduleDialog();

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        if (scheduleService.HasTimeConflict(schedules, dialog.Schedule))
        {
            ShowTimeConflictMessage();
            return;
        }

        try
        {
            int newId =
                timetableDbRepository.Add(
                    loggedInUserId,
                    dialog.Schedule);

            dialog.Schedule.TimetableId = newId;

            schedules.Add(dialog.Schedule);

            RenderSchedules();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"시간표를 DB에 저장하지 못했습니다.\n\n{ex.Message}",
                "DB 저장 오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void EditSchedule(ClassSchedule schedule)
    {
        using var dialog = new ClassScheduleDialog(schedule);

        var result = dialog.ShowDialog(this);

        try
        {
            // 삭제
            if (result == DialogResult.Yes)
            {
                timetableDbRepository.Delete(
                    loggedInUserId,
                    schedule);

                schedules.RemoveAll(
                    item => item.Id == schedule.Id);

                RenderSchedules();
                return;
            }

            if (result != DialogResult.OK)
                return;

            if (scheduleService.HasTimeConflict(
                schedules,
                dialog.Schedule,
                schedule.Id))
            {
                ShowTimeConflictMessage();
                return;
            }

            // 기존 DB ID 유지
            dialog.Schedule.TimetableId =
                schedule.TimetableId;

            timetableDbRepository.Update(
                loggedInUserId,
                dialog.Schedule);

            var index =
                schedules.FindIndex(
                    item => item.Id == schedule.Id);

            if (index >= 0)
            {
                schedules[index] =
                    dialog.Schedule;
            }

            RenderSchedules();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"시간표를 DB에 반영하지 못했습니다.\n\n{ex.Message}",
                "DB 저장 오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            LoadSchedules();
            RenderSchedules();
        }
    }

    private static void ShowTimeConflictMessage()
    {
        MessageBox.Show(
            "같은 요일과 시간에 이미 등록된 수업이 있습니다.",
            "시간 중복",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }

    private void RenderSchedules()
    {
        RemoveRuntimeScheduleLabels();

        foreach (var schedule in schedules.OrderBy(item => item.Day).ThenBy(item => item.StartHour))
        {
            var subjectLabel = CreateSubjectLabel(schedule);
            var column = GetDayColumn(schedule.Day);
            var row = schedule.StartHour - 8;
            var rowSpan = schedule.EndHour - schedule.StartHour;

            scheduleTable.Controls.Add(subjectLabel, column, row);
            scheduleTable.SetRowSpan(subjectLabel, rowSpan);
        }

        RenderTodayClasses();
    }

    private Label CreateSubjectLabel(ClassSchedule schedule)
    {
        var label = new Label
        {
            BackColor = GetScheduleBackgroundColor(schedule),
            Cursor = Cursors.Hand,
            Dock = DockStyle.Fill,
            Font = new Font("맑은 고딕", 8.5F),
            ForeColor = Color.FromArgb(31, 42, 68),
            Margin = new Padding(0),
            Tag = schedule,
            Text = string.IsNullOrWhiteSpace(schedule.Classroom)
                ? schedule.SubjectName
                : $"{schedule.SubjectName}\r\n{schedule.Classroom}",
            TextAlign = ContentAlignment.MiddleCenter
        };

        label.Click += (_, _) => EditSchedule(schedule);
        return label;
    }

    private void RemoveRuntimeScheduleLabels()
    {
        var controls = scheduleTable.Controls
            .Cast<Control>()
            .Where(control => control.Tag is ClassSchedule)
            .ToArray();

        foreach (var control in controls)
        {
            scheduleTable.Controls.Remove(control);
            control.Dispose();
        }
    }

    private void RenderTodayClasses()
    {
        ClearTodayClassList();

        var todaySchedules = schedules
            .Where(item => item.Day == DateTime.Today.DayOfWeek)
            .OrderBy(item => item.StartHour)
            .ToList();

        lblTodayDate.Text = DateTime.Today.ToString("M월 d일 dddd");

        if (todaySchedules.Count == 0)
        {
            todayClassList.Controls.Add(new Label
            {
                AutoSize = false,
                Font = new Font("맑은 고딕", 9.5F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Size = new Size(Math.Max(180, todayClassList.ClientSize.Width - 20), 80),
                Text = "오늘 등록된 수업이 없습니다.",
                TextAlign = ContentAlignment.MiddleCenter
            });
        }
        else
        {
            foreach (var schedule in todaySchedules)
                todayClassList.Controls.Add(CreateTodayClassCard(schedule));
        }

        UpdateProgress(todaySchedules);
    }

    private Panel CreateTodayClassCard(ClassSchedule schedule)
    {
        var card = new Panel
        {
            BackColor = Color.FromArgb(247, 249, 252),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 0, 10),
            Size = new Size(GetTodayCardWidth(), 88),
            Tag = schedule
        };

        var accent = new Panel
        {
            BackColor = GetScheduleAccentColor(schedule),
            Dock = DockStyle.Left,
            Width = 5
        };
        var timeLabel = new Label
        {
            AutoSize = true,
            Font = new Font("맑은 고딕", 8.5F),
            ForeColor = Color.FromArgb(107, 114, 128),
            Location = new Point(17, 10),
            Text = $"{schedule.StartHour:00}:00 - {schedule.EndHour:00}:00"
        };
        var nameLabel = new Label
        {
            AutoEllipsis = true,
            Font = new Font("맑은 고딕", 10.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 42, 68),
            Location = new Point(16, 32),
            Size = new Size(Math.Max(100, card.Width - 32), 22),
            Text = schedule.SubjectName
        };
        var roomLabel = new Label
        {
            AutoEllipsis = true,
            Font = new Font("맑은 고딕", 8.5F),
            ForeColor = Color.FromArgb(107, 114, 128),
            Location = new Point(17, 59),
            Size = new Size(Math.Max(100, card.Width - 32), 18),
            Text = schedule.Classroom
        };

        card.Controls.Add(accent);
        card.Controls.Add(timeLabel);
        card.Controls.Add(nameLabel);
        card.Controls.Add(roomLabel);

        foreach (Control control in card.Controls)
            control.Click += (_, _) => EditSchedule(schedule);
        card.Click += (_, _) => EditSchedule(schedule);

        return card;
    }

    private void UpdateProgress(IReadOnlyCollection<ClassSchedule> todaySchedules)
    {
        var now = DateTime.Now;
        var completed = todaySchedules.Count(item => now >= DateTime.Today.AddHours(item.EndHour));
        var total = todaySchedules.Count;

        todayProgressBar.Maximum = Math.Max(1, total);
        todayProgressBar.Value = completed;
        lblProgressCount.Text = $"{completed} / {total}";
        lblProgressMessage.Text = total switch
        {
            0 => "오늘은 등록된 수업이 없어요.",
            _ when completed == total => "오늘 수업을 모두 마쳤어요.",
            _ => $"오늘 수업 {total - completed}개가 남아 있어요."
        };
    }

    private void ResizeTodayClassCards()
    {
        foreach (Control control in todayClassList.Controls)
        {
            if (control is Panel card)
                card.Width = GetTodayCardWidth();
        }
    }

    private void PositionProgressCount()
    {
        lblProgressCount.Left = progressPanel.ClientSize.Width - lblProgressCount.Width - 16;
    }

    private int GetTodayCardWidth()
    {
        return Math.Max(180, todayClassList.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2);
    }

    private void ClearTodayClassList()
    {
        while (todayClassList.Controls.Count > 0)
        {
            var control = todayClassList.Controls[0];
            todayClassList.Controls.RemoveAt(0);
            control.Dispose();
        }
    }

    private void RemovePreviewData()
    {
        var previewSubjects = new Control[]
        {
            lblSubjectDataStructure,
            lblSubjectOperatingSystem,
            lblSubjectCSharp,
            lblSubjectWeb,
            lblSubjectDatabase,
            lblSubjectComputer,
            lblSubjectProject,
            lblSubjectEnglish
        };

        foreach (var control in previewSubjects)
        {
            scheduleTable.Controls.Remove(control);
            control.Dispose();
        }

        ClearTodayClassList();
    }

    private void LoadSchedules()
    {
        try
        {
            schedules.Clear();

            schedules.AddRange(
                timetableDbRepository
                    .Load(loggedInUserId)
                    .Where(scheduleService.IsValid));
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"시간표를 DB에서 불러오지 못했습니다.\n\n{ex.Message}",
                "DB 불러오기 오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private static int GetDayColumn(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => 1,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 3,
        DayOfWeek.Thursday => 4,
        DayOfWeek.Friday => 5,
        _ => throw new ArgumentOutOfRangeException(nameof(day))
    };

    private void ScheduleColorsChanged(object? sender, EventArgs e)
    {
        ApplyCategoryColors();
        RenderSchedules();
    }

    private void ApplyCategoryColors()
    {
        legendBlue.BackColor = ScheduleColorService.GetAccentColor(ScheduleCategory.Major);
        legendGreen.BackColor = ScheduleColorService.GetAccentColor(ScheduleCategory.General);
        legendOrange.BackColor = ScheduleColorService.GetAccentColor(ScheduleCategory.Other);
    }

    private static Color GetScheduleBackgroundColor(ClassSchedule schedule) =>
        ScheduleColorService.GetScheduleBackgroundColor(schedule.Category, schedule.CustomColorArgb);

    private static Color GetScheduleAccentColor(ClassSchedule schedule) =>
        ScheduleColorService.GetScheduleAccentColor(schedule.Category, schedule.CustomColorArgb);
}
