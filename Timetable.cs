using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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

        todayClassList.SizeChanged +=
            (_, _) => ResizeTodayClassCards();

        progressPanel.SizeChanged +=
            (_, _) => PositionProgressCount();

        ScheduleColorService.SettingsChanged +=
            ScheduleColorsChanged;

        Disposed +=
            (_, _) =>
                ScheduleColorService.SettingsChanged -=
                    ScheduleColorsChanged;

        LoadSchedules();

        ApplyCategoryColors();

        RenderSchedules();

        PositionProgressCount();

        ApplyCurrentTheme();
    }

    // =========================================================
    // 수업 추가
    // =========================================================
    private void BtnAddClass_Click(object? sender, EventArgs e)
    {
        using var dialog = new ClassScheduleDialog();

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        if (scheduleService.HasTimeConflict(
            schedules,
            dialog.Schedule))
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

            ApplyCurrentTheme();
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

    // =========================================================
    // 수업 수정 / 삭제
    // =========================================================
    private void EditSchedule(ClassSchedule schedule)
    {
        using var dialog =
            new ClassScheduleDialog(schedule);

        var result =
            dialog.ShowDialog(this);

        try
        {
            if (result == DialogResult.Yes)
            {
                timetableDbRepository.Delete(
                    loggedInUserId,
                    schedule);

                schedules.RemoveAll(
                    item =>
                        item.Id == schedule.Id);

                RenderSchedules();

                ApplyCurrentTheme();

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

            dialog.Schedule.TimetableId =
                schedule.TimetableId;

            timetableDbRepository.Update(
                loggedInUserId,
                dialog.Schedule);

            var index =
                schedules.FindIndex(
                    item =>
                        item.Id == schedule.Id);

            if (index >= 0)
            {
                schedules[index] =
                    dialog.Schedule;
            }

            RenderSchedules();

            ApplyCurrentTheme();
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

            ApplyCurrentTheme();
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

    // =========================================================
    // 시간표 수업 렌더링
    // =========================================================
    private void RenderSchedules()
    {
        RemoveRuntimeScheduleLabels();

        foreach (
            var schedule
            in schedules
                .OrderBy(item => item.Day)
                .ThenBy(item => item.StartHour))
        {
            var subjectLabel =
                CreateSubjectLabel(schedule);

            var column =
                GetDayColumn(schedule.Day);

            var row =
                schedule.StartHour - 8;

            var rowSpan =
                schedule.EndHour -
                schedule.StartHour;

            scheduleTable.Controls.Add(
                subjectLabel,
                column,
                row);

            scheduleTable.SetRowSpan(
                subjectLabel,
                rowSpan);
        }

        RenderTodayClasses();
    }

    // =========================================================
    // 시간표 안의 과목 Label
    // =========================================================
    private Label CreateSubjectLabel(
        ClassSchedule schedule)
    {
        var label =
            new Label
            {
                BackColor =
                    GetThemedScheduleBackgroundColor(schedule),

                Cursor =
                    Cursors.Hand,

                Dock =
                    DockStyle.Fill,

                Font =
                    new Font(
                        "맑은 고딕",
                        8.5F),

                ForeColor =
                    GetScheduleTextColor(),

                Margin =
                    new Padding(0),

                Tag =
                    schedule,

                Text =
                    string.IsNullOrWhiteSpace(
                        schedule.Classroom)
                        ? schedule.SubjectName
                        : $"{schedule.SubjectName}\r\n{schedule.Classroom}",

                TextAlign =
                    ContentAlignment.MiddleCenter
            };

        label.Click +=
            (_, _) =>
                EditSchedule(schedule);

        return label;
    }

    // =========================================================
    // 런타임 수업 Label 삭제
    // =========================================================
    private void RemoveRuntimeScheduleLabels()
    {
        var controls =
            scheduleTable.Controls
                .Cast<Control>()
                .Where(
                    control =>
                        control.Tag
                        is ClassSchedule)
                .ToArray();

        foreach (var control in controls)
        {
            scheduleTable.Controls.Remove(
                control);

            control.Dispose();
        }
    }

    // =========================================================
    // 오늘 수업
    // =========================================================
    private void RenderTodayClasses()
    {
        ClearTodayClassList();

        var todaySchedules =
            schedules
                .Where(
                    item =>
                        item.Day ==
                        DateTime.Today.DayOfWeek)
                .OrderBy(
                    item =>
                        item.StartHour)
                .ToList();

        lblTodayDate.Text =
            DateTime.Today.ToString(
                "M월 d일 dddd");

        if (todaySchedules.Count == 0)
        {
            todayClassList.Controls.Add(
                new Label
                {
                    AutoSize = false,

                    Font =
                        new Font(
                            "맑은 고딕",
                            9.5F),

                    ForeColor =
                        GetSecondaryTextColor(),

                    BackColor =
                        Color.Transparent,

                    Size =
                        new Size(
                            Math.Max(
                                180,
                                todayClassList
                                    .ClientSize
                                    .Width -
                                20),
                            80),

                    Text =
                        "오늘 등록된 수업이 없습니다.",

                    TextAlign =
                        ContentAlignment.MiddleCenter
                });
        }
        else
        {
            foreach (
                var schedule
                in todaySchedules)
            {
                todayClassList.Controls.Add(
                    CreateTodayClassCard(
                        schedule));
            }
        }

        UpdateProgress(todaySchedules);
    }

    // =========================================================
    // 오늘 수업 카드
    // =========================================================
    private Panel CreateTodayClassCard(
        ClassSchedule schedule)
    {
        var card =
            new Panel
            {
                BackColor =
                    GetCardColor(),

                Cursor =
                    Cursors.Hand,

                Margin =
                    new Padding(
                        0,
                        0,
                        0,
                        10),

                Size =
                    new Size(
                        GetTodayCardWidth(),
                        88),

                Tag =
                    schedule
            };

        var accent =
            new Panel
            {
                BackColor =
                    GetThemedScheduleAccentColor(
                        schedule),

                Dock =
                    DockStyle.Left,

                Width =
                    5
            };

        var timeLabel =
            new Label
            {
                AutoSize =
                    true,

                Font =
                    new Font(
                        "맑은 고딕",
                        8.5F),

                ForeColor =
                    GetSecondaryTextColor(),

                BackColor =
                    Color.Transparent,

                Location =
                    new Point(
                        17,
                        10),

                Text =
                    $"{schedule.StartHour:00}:00 - {schedule.EndHour:00}:00"
            };

        var nameLabel =
            new Label
            {
                AutoEllipsis =
                    true,

                Font =
                    new Font(
                        "맑은 고딕",
                        10.5F,
                        FontStyle.Bold),

                ForeColor =
                    UiThemeService.TextColor,

                BackColor =
                    Color.Transparent,

                Location =
                    new Point(
                        16,
                        32),

                Size =
                    new Size(
                        Math.Max(
                            100,
                            card.Width -
                            32),
                        22),

                Text =
                    schedule.SubjectName
            };

        var roomLabel =
            new Label
            {
                AutoEllipsis =
                    true,

                Font =
                    new Font(
                        "맑은 고딕",
                        8.5F),

                ForeColor =
                    GetSecondaryTextColor(),

                BackColor =
                    Color.Transparent,

                Location =
                    new Point(
                        17,
                        59),

                Size =
                    new Size(
                        Math.Max(
                            100,
                            card.Width -
                            32),
                        18),

                Text =
                    schedule.Classroom
            };

        card.Controls.Add(accent);
        card.Controls.Add(timeLabel);
        card.Controls.Add(nameLabel);
        card.Controls.Add(roomLabel);

        foreach (
            Control control
            in card.Controls)
        {
            control.Click +=
                (_, _) =>
                    EditSchedule(schedule);
        }

        card.Click +=
            (_, _) =>
                EditSchedule(schedule);

        return card;
    }

    // =========================================================
    // 오늘 진행률
    // =========================================================
    private void UpdateProgress(
        IReadOnlyCollection<ClassSchedule>
        todaySchedules)
    {
        var now =
            DateTime.Now;

        var completed =
            todaySchedules.Count(
                item =>
                    now >=
                    DateTime.Today
                        .AddHours(
                            item.EndHour));

        var total =
            todaySchedules.Count;

        todayProgressBar.Maximum =
            Math.Max(
                1,
                total);

        todayProgressBar.Value =
            completed;

        lblProgressCount.Text =
            $"{completed} / {total}";

        lblProgressMessage.Text =
            total switch
            {
                0 =>
                    "오늘은 등록된 수업이 없어요.",

                _ when completed == total =>
                    "오늘 수업을 모두 마쳤어요.",

                _ =>
                    $"오늘 수업 {total - completed}개가 남아 있어요."
            };
    }

    // =========================================================
    // 오늘 카드 크기
    // =========================================================
    private void ResizeTodayClassCards()
    {
        foreach (
            Control control
            in todayClassList.Controls)
        {
            if (control is Panel card)
            {
                card.Width =
                    GetTodayCardWidth();
            }
        }
    }

    private void PositionProgressCount()
    {
        lblProgressCount.Left =
            progressPanel
                .ClientSize
                .Width -
            lblProgressCount.Width -
            16;
    }

    private int GetTodayCardWidth()
    {
        return Math.Max(
            180,
            todayClassList
                .ClientSize
                .Width -
            SystemInformation
                .VerticalScrollBarWidth -
            2);
    }

    private void ClearTodayClassList()
    {
        while (
            todayClassList
                .Controls
                .Count > 0)
        {
            var control =
                todayClassList
                    .Controls[0];

            todayClassList
                .Controls
                .RemoveAt(0);

            control.Dispose();
        }
    }

    // =========================================================
    // 디자이너 미리보기 제거
    // =========================================================
    private void RemovePreviewData()
    {
        var previewSubjects =
            new Control[]
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

        foreach (
            var control
            in previewSubjects)
        {
            scheduleTable.Controls.Remove(
                control);

            control.Dispose();
        }

        ClearTodayClassList();
    }

    // =========================================================
    // DB 로드
    // =========================================================
    private void LoadSchedules()
    {
        try
        {
            schedules.Clear();

            schedules.AddRange(
                timetableDbRepository
                    .Load(loggedInUserId)
                    .Where(
                        scheduleService
                            .IsValid));
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

    // =========================================================
    // 요일 → TableLayoutPanel 열
    // =========================================================
    private static int GetDayColumn(
        DayOfWeek day) =>
        day switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,

            _ =>
                throw new ArgumentOutOfRangeException(
                    nameof(day))
        };

    // =========================================================
    // 수업 색상 설정 변경
    // =========================================================
    private void ScheduleColorsChanged(
        object? sender,
        EventArgs e)
    {
        ApplyCategoryColors();

        RenderSchedules();

        ApplyCurrentTheme();
    }

    private void ApplyCategoryColors()
    {
        legendBlue.BackColor =
            GetThemedLegendColor(
                ScheduleCategory.Major);

        legendGreen.BackColor =
            GetThemedLegendColor(
                ScheduleCategory.General);

        legendOrange.BackColor =
            GetThemedLegendColor(
                ScheduleCategory.Other);
    }

    private static Color GetScheduleBackgroundColor(
        ClassSchedule schedule) =>
        ScheduleColorService
            .GetScheduleBackgroundColor(
                schedule.Category,
                schedule.CustomColorArgb);

    private static Color GetScheduleAccentColor(
        ClassSchedule schedule) =>
        ScheduleColorService
            .GetScheduleAccentColor(
                schedule.Category,
                schedule.CustomColorArgb);

    // =========================================================
    // ★ 현재 테마 적용
    // =========================================================
    public void ApplyCurrentTheme()
    {
        SuspendLayout();

        try
        {
            // -------------------------------------------------
            // 전체
            // -------------------------------------------------
            BackColor =
                UiThemeService.BackgroundColor;

            ForeColor =
                UiThemeService.TextColor;

            UiThemeService.ApplyTheme(this);

            // -------------------------------------------------
            // 시간표 자체
            // -------------------------------------------------
            ApplyScheduleTableTheme();

            // -------------------------------------------------
            // 오늘 수업 영역
            // -------------------------------------------------
            if (todayClassList != null)
            {
                todayClassList.BackColor =
                    UiThemeService.BackgroundColor;

                todayClassList.ForeColor =
                    UiThemeService.TextColor;
            }

            // -------------------------------------------------
            // 진행률 패널
            // -------------------------------------------------
            if (progressPanel != null)
            {
                progressPanel.BackColor =
                    UiThemeService.SurfaceColor;

                progressPanel.ForeColor =
                    UiThemeService.TextColor;
            }

            // -------------------------------------------------
            // 일반 컨트롤
            // -------------------------------------------------
            ApplyTimetableControlTheme(this);

            // -------------------------------------------------
            // 시간표는 일반 테마 적용 이후
            // 다시 덮어써야 합니다.
            // -------------------------------------------------
            ApplyScheduleTableTheme();

            // -------------------------------------------------
            // 범례 색상
            // -------------------------------------------------
            ApplyCategoryColors();

            // -------------------------------------------------
            // 실제 등록된 과목 색상
            // -------------------------------------------------
            foreach (
                Control control
                in scheduleTable.Controls)
            {
                if (
                    control is Label label &&
                    label.Tag is ClassSchedule schedule)
                {
                    label.BackColor =
                        GetThemedScheduleBackgroundColor(
                            schedule);

                    label.ForeColor =
                        GetScheduleTextColor();
                }
            }

            // -------------------------------------------------
            // 오늘 수업 카드 다시 생성
            // -------------------------------------------------
            RenderTodayClasses();

            // RenderTodayClasses 후에도
            // 기본 텍스트 색상 보정
            lblTodayDate.ForeColor =
                UiThemeService.TextColor;

            lblProgressCount.ForeColor =
                UiThemeService.TextColor;

            lblProgressMessage.ForeColor =
                GetSecondaryTextColor();

            Invalidate(true);
        }
        finally
        {
            ResumeLayout(true);
        }
    }

    // =========================================================
    // ★ 시간표 전용 테마
    // =========================================================
    private void ApplyScheduleTableTheme()
    {
        if (scheduleTable == null)
            return;

        scheduleTable.BackColor =
            GetTimetableGridColor();

        scheduleTable.ForeColor =
            UiThemeService.TextColor;

        foreach (
            Control control
            in scheduleTable.Controls)
        {
            // 실제 과목
            if (
                control is Label subjectLabel &&
                subjectLabel.Tag is ClassSchedule schedule)
            {
                subjectLabel.BackColor =
                    GetThemedScheduleBackgroundColor(
                        schedule);

                subjectLabel.ForeColor =
                    GetScheduleTextColor();

                continue;
            }

            // 디자이너에서 만들어진 Label
            if (control is Label label)
            {
                var position =
                    scheduleTable
                        .GetPositionFromControl(label);

                int column =
                    position.Column;

                int row =
                    position.Row;

                // ---------------------------------------------
                // 첫 번째 행 = 요일 헤더
                // ---------------------------------------------
                if (row == 0)
                {
                    label.BackColor =
                        GetTimetableHeaderColor();

                    label.ForeColor =
                        GetTimetableHeaderTextColor();
                }

                // ---------------------------------------------
                // 첫 번째 열 = 시간
                // ---------------------------------------------
                else if (column == 0)
                {
                    label.BackColor =
                        GetTimetableTimeColor();

                    label.ForeColor =
                        GetTimetableTimeTextColor();
                }

                // ---------------------------------------------
                // 나머지 기본 Label
                // ---------------------------------------------
                else
                {
                    label.BackColor =
                        GetTimetableCellColor();

                    label.ForeColor =
                        UiThemeService.TextColor;
                }
            }
        }
    }

    // =========================================================
    // 하위 컨트롤 기본 테마
    // =========================================================
    private void ApplyTimetableControlTheme(
        Control parent)
    {
        foreach (
            Control control
            in parent.Controls)
        {
            // scheduleTable은
            // 전용 메서드에서 처리
            if (control == scheduleTable)
                continue;

            if (control is Label label)
            {
                // 범례 색상 Panel 안쪽 등이 아닌
                // 일반 Label
                label.ForeColor =
                    UiThemeService.TextColor;
            }

            else if (control is Button button)
            {
                button.BackColor =
                    UiThemeService.PrimaryColor;

                button.ForeColor =
                    GetButtonTextColor();

                button.FlatStyle =
                    FlatStyle.Flat;

                button.FlatAppearance.BorderSize =
                    0;

                button.UseVisualStyleBackColor =
                    false;
            }

            else if (control is Panel panel)
            {
                panel.BackColor =
                    UiThemeService.SurfaceColor;

                panel.ForeColor =
                    UiThemeService.TextColor;
            }

            if (control.HasChildren)
            {
                ApplyTimetableControlTheme(
                    control);
            }
        }
    }

    // =========================================================
    // ★ 테마별 시간표 헤더색
    // =========================================================
    private static Color GetTimetableHeaderColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(58, 55, 72),

            AppTheme.Blossom =>
                Color.FromArgb(248, 214, 225),

            AppTheme.Mint =>
                Color.FromArgb(209, 235, 225),

            AppTheme.Lavender =>
                Color.FromArgb(224, 215, 241),

            AppTheme.Cozy =>
                Color.FromArgb(232, 217, 196),

            _ =>
                Color.FromArgb(232, 237, 247)
        };
    }

    // =========================================================
    // ★ 테마별 시간표 헤더 글자색
    // =========================================================
    private static Color GetTimetableHeaderTextColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(
                    245,
                    242,
                    250),

            _ =>
                Color.FromArgb(
                    60,
                    60,
                    70)
        };
    }

    // =========================================================
    // ★ 시간 열 색상
    // =========================================================
    private static Color GetTimetableTimeColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(48, 48, 54),

            AppTheme.Blossom =>
                Color.FromArgb(252, 237, 242),

            AppTheme.Mint =>
                Color.FromArgb(233, 247, 241),

            AppTheme.Lavender =>
                Color.FromArgb(241, 236, 249),

            AppTheme.Cozy =>
                Color.FromArgb(245, 236, 223),

            _ =>
                Color.FromArgb(246, 248, 251)
        };
    }

    private static Color GetTimetableTimeTextColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(
                    210,
                    210,
                    218),

            _ =>
                Color.FromArgb(
                    90,
                    90,
                    100)
        };
    }

    // =========================================================
    // ★ 시간표 빈칸 색상
    // =========================================================
    private static Color GetTimetableCellColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(38, 38, 42),

            AppTheme.Blossom =>
                Color.FromArgb(255, 252, 253),

            AppTheme.Mint =>
                Color.FromArgb(252, 255, 253),

            AppTheme.Lavender =>
                Color.FromArgb(253, 251, 255),

            AppTheme.Cozy =>
                Color.FromArgb(255, 252, 247),

            _ =>
                Color.White
        };
    }

    // =========================================================
    // ★ 시간표 선 색상
    // TableLayoutPanel의 BackColor가 셀 사이 선처럼 보입니다.
    // =========================================================
    private static Color GetTimetableGridColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(72, 72, 78),

            AppTheme.Blossom =>
                Color.FromArgb(231, 199, 210),

            AppTheme.Mint =>
                Color.FromArgb(195, 220, 210),

            AppTheme.Lavender =>
                Color.FromArgb(207, 199, 224),

            AppTheme.Cozy =>
                Color.FromArgb(216, 202, 184),

            _ =>
                Color.FromArgb(211, 216, 224)
        };
    }

    // =========================================================
    // ★ 과목 배경색
    // 기존 과목 색상을 가져온 뒤
    // 현재 테마에 맞게 살짝 보정
    // =========================================================
    private static Color GetThemedScheduleBackgroundColor(
        ClassSchedule schedule)
    {
        Color original =
            GetScheduleBackgroundColor(
                schedule);

        return UiThemeService.CurrentTheme switch
        {
            // 다크에서는 과목색을 조금 어둡게
            AppTheme.Dark =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        38,
                        38,
                        42),
                    0.45),

            // Blossom은 아주 약하게 핑크 쪽으로
            AppTheme.Blossom =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        255,
                        225,
                        235),
                    0.18),

            // Mint
            AppTheme.Mint =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        220,
                        245,
                        235),
                    0.15),

            // Lavender
            AppTheme.Lavender =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        235,
                        225,
                        250),
                    0.18),

            // Cozy
            AppTheme.Cozy =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        242,
                        225,
                        205),
                    0.18),

            _ =>
                original
        };
    }

    // =========================================================
    // ★ 과목 강조색
    // =========================================================
    private static Color GetThemedScheduleAccentColor(
        ClassSchedule schedule)
    {
        Color original =
            GetScheduleAccentColor(
                schedule);

        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        90,
                        80,
                        125),
                    0.20),

            AppTheme.Blossom =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        225,
                        105,
                        145),
                    0.15),

            AppTheme.Mint =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        75,
                        160,
                        130),
                    0.15),

            AppTheme.Lavender =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        135,
                        105,
                        190),
                    0.15),

            AppTheme.Cozy =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        170,
                        125,
                        85),
                    0.15),

            _ =>
                original
        };
    }

    // =========================================================
    // ★ 범례 색상도 테마에 맞게
    // =========================================================
    private static Color GetThemedLegendColor(
        ScheduleCategory category)
    {
        Color original =
            ScheduleColorService
                .GetAccentColor(category);

        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        80,
                        75,
                        100),
                    0.20),

            AppTheme.Blossom =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        240,
                        140,
                        170),
                    0.12),

            AppTheme.Mint =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        100,
                        185,
                        155),
                    0.12),

            AppTheme.Lavender =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        155,
                        130,
                        205),
                    0.12),

            AppTheme.Cozy =>
                BlendColor(
                    original,
                    Color.FromArgb(
                        185,
                        145,
                        105),
                    0.12),

            _ =>
                original
        };
    }

    // =========================================================
    // 색상 혼합
    // amount = 두 번째 색상의 비율
    // =========================================================
    private static Color BlendColor(
        Color first,
        Color second,
        double amount)
    {
        amount =
            Math.Max(
                0,
                Math.Min(
                    1,
                    amount));

        int r =
            (int)(
                first.R * (1 - amount) +
                second.R * amount);

        int g =
            (int)(
                first.G * (1 - amount) +
                second.G * amount);

        int b =
            (int)(
                first.B * (1 - amount) +
                second.B * amount);

        return Color.FromArgb(
            r,
            g,
            b);
    }

    // =========================================================
    // 시간표 과목 글씨
    // =========================================================
    private static Color GetScheduleTextColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(
                    245,
                    245,
                    248),

            _ =>
                Color.FromArgb(
                    55,
                    55,
                    65)
        };
    }

    // =========================================================
    // 버튼 글씨
    // =========================================================
    private static Color GetButtonTextColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.White,

            _ =>
                Color.FromArgb(
                    45,
                    45,
                    55)
        };
    }

    // =========================================================
    // 오늘 수업 카드
    // =========================================================
    private static Color GetCardColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(
                    52,
                    52,
                    58),

            AppTheme.Blossom =>
                Color.FromArgb(
                    255,
                    250,
                    252),

            AppTheme.Mint =>
                Color.FromArgb(
                    248,
                    255,
                    252),

            AppTheme.Lavender =>
                Color.FromArgb(
                    252,
                    249,
                    255),

            AppTheme.Cozy =>
                Color.FromArgb(
                    255,
                    251,
                    245),

            _ =>
                Color.FromArgb(
                    247,
                    249,
                    252)
        };
    }

    // =========================================================
    // 보조 글씨
    // =========================================================
    private static Color GetSecondaryTextColor()
    {
        return UiThemeService.CurrentTheme switch
        {
            AppTheme.Dark =>
                Color.FromArgb(
                    190,
                    190,
                    200),

            AppTheme.Blossom =>
                Color.FromArgb(
                    135,
                    100,
                    110),

            AppTheme.Mint =>
                Color.FromArgb(
                    90,
                    120,
                    110),

            AppTheme.Lavender =>
                Color.FromArgb(
                    110,
                    100,
                    130),

            AppTheme.Cozy =>
                Color.FromArgb(
                    125,
                    105,
                    85),

            _ =>
                Color.FromArgb(
                    107,
                    114,
                    128)
        };
    }
}