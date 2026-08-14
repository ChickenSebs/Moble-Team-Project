using calendar4.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace calendar4
{
    public partial class CalendarControl : UserControl
    {
        private Dictionary<DateTime, string> holidayMap =
            new Dictionary<DateTime, string>();

        private DataGridView dgv;

        private readonly CalendarDbRepository calendarDbRepository = new();

        private readonly int loggedInUserId;
        private readonly int calendarId;

        private readonly CalendarMonthCellRenderer monthCellRenderer =
            new(PersonalCategoryStores.Calendar);

        private Dictionary<DateTime, List<CalendarScheduleEntry>> scheduleMap =
            new Dictionary<DateTime, List<CalendarScheduleEntry>>();

        private Dictionary<DateTime, string> ddayMap =
            new Dictionary<DateTime, string>();

        private DateTime currentDate =
            DateTime.Now;

        private CalendarViewMode viewMode =
            CalendarViewMode.Month;

        private const int CalendarStartHour = 8;
        private const int CalendarEndHour = 22;

        public event EventHandler DateOrScheduleChanged;


        // ============================================================
        // 보기 모드
        // ============================================================

        public enum CalendarViewMode
        {
            Month,
            Week,
            Day
        }


        // ============================================================
        // 생성자
        // ============================================================

        public CalendarControl(
            int userId,
            int calendarId)
        {
            loggedInUserId = userId;
            this.calendarId = calendarId;

            InitializeUserControl();
            LoadSchedules();
        }


        // ============================================================
        // 기본 UI 생성
        // ============================================================

        private void InitializeUserControl()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,

                ReadOnly = true,

                RowHeadersVisible = false,
                ScrollBars = ScrollBars.None,

                SelectionMode =
                    DataGridViewSelectionMode.CellSelect,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            dgv.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.TopLeft;


            // ========================================================
            // 현재 설정된 글꼴 사용
            // ========================================================

            dgv.DefaultCellStyle.Font =
                AppFontService.CreateFont(
                    9.5f,
                    FontStyle.Bold);


            dgv.DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;


            dgv.Resize +=
                (s, ev) => AdjustRowHeights();

            dgv.CellDoubleClick +=
                DgvCalendar_CellDoubleClick;

            dgv.CellPainting +=
                DgvCalendar_CellPainting;


            // 마우스 휠
            dgv.MouseEnter +=
                (s, e) => dgv.Focus();

            dgv.MouseWheel +=
                DgvCalendar_MouseWheel;


            Controls.Add(dgv);
        }


        // ============================================================
        // 마우스 휠
        // ============================================================

        private void DgvCalendar_MouseWheel(
            object? sender,
            MouseEventArgs e)
        {
            int moveDirection =
                e.Delta > 0 ? -1 : 1;

            switch (viewMode)
            {
                case CalendarViewMode.Month:

                    currentDate =
                        currentDate.AddMonths(
                            moveDirection);

                    break;


                case CalendarViewMode.Week:

                    currentDate =
                        currentDate.AddDays(
                            moveDirection * 7);

                    break;


                case CalendarViewMode.Day:

                    currentDate =
                        currentDate.AddDays(
                            moveDirection);

                    break;
            }

            UpdateView();

            DateOrScheduleChanged?.Invoke(
                this,
                EventArgs.Empty);
        }


        // ============================================================
        // 보기 모드 설정
        // ============================================================

        public void SetViewMode(
            CalendarViewMode newMode)
        {
            viewMode = newMode;

            UpdateView();
        }


        public void SetTargetDate(
            DateTime date)
        {
            currentDate = date;

            UpdateView();
        }


        public DateTime GetTargetDate()
        {
            return currentDate;
        }


        public CalendarViewMode GetViewMode()
        {
            return viewMode;
        }


        public Dictionary<DateTime, List<CalendarScheduleEntry>>
            GetScheduleMap()
        {
            return scheduleMap;
        }


        // ============================================================
        // 공휴일
        // ============================================================

        public void SetHolidayMap(
            Dictionary<DateTime, string> holidays)
        {
            holidayMap =
                holidays != null
                    ? new Dictionary<DateTime, string>(
                        holidays)
                    : new Dictionary<DateTime, string>();

            UpdateView();
        }


        // ============================================================
        // 전체 화면 업데이트
        // ============================================================

        public void UpdateView()
        {
            switch (viewMode)
            {
                case CalendarViewMode.Week:

                    UpdateWeekView();

                    break;


                case CalendarViewMode.Day:

                    UpdateDayView();

                    break;


                default:

                    UpdateMonthView();

                    break;
            }

            dgv.ClearSelection();

            dgv.CurrentCell = null;
        }


        // ============================================================
        // Grid 기본 설정
        // ============================================================

        private void ConfigureGrid(
            string[] headers,
            int rowCount,
            int headerHeight)
        {
            dgv.SuspendLayout();

            dgv.Columns.Clear();
            dgv.Rows.Clear();

            dgv.ColumnHeadersHeight =
                headerHeight;

            dgv.EnableHeadersVisualStyles =
                false;

            dgv.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;


            // 현재 폰트 적용
            dgv.ColumnHeadersDefaultCellStyle.Font =
                AppFontService.CreateFont(
                    9F,
                    FontStyle.Bold);


            for (int i = 0;
                 i < headers.Length;
                 i++)
            {
                int idx =
                    dgv.Columns.Add(
                        $"col{i}",
                        headers[i]);

                dgv.Columns[idx].SortMode =
                    DataGridViewColumnSortMode.NotSortable;

                dgv.Columns[idx].FillWeight =
                    1;
            }

            dgv.Rows.Add(
                rowCount);

            dgv.ResumeLayout();
        }


        // ============================================================
        // 월간 보기
        // ============================================================

        private void UpdateMonthView()
        {
            string[] dayNames =
            {
                "일",
                "월",
                "화",
                "수",
                "목",
                "금",
                "토"
            };

            ConfigureGrid(
                dayNames,
                6,
                30);


            dgv.RowHeadersVisible =
                false;

            dgv.ScrollBars =
                ScrollBars.None;


            // 일요일
            dgv.Columns[0]
                .HeaderCell
                .Style
                .ForeColor =
                Color.Red;


            // 토요일
            dgv.Columns[6]
                .HeaderCell
                .Style
                .ForeColor =
                GetSaturdayColor();


            DateTime firstDay =
                new DateTime(
                    currentDate.Year,
                    currentDate.Month,
                    1);


            int totalDays =
                DateTime.DaysInMonth(
                    currentDate.Year,
                    currentDate.Month);


            int startDayOfWeek =
                (int)firstDay.DayOfWeek;


            for (int day = 1;
                 day <= totalDays;
                 day++)
            {
                int cellIndex =
                    startDayOfWeek +
                    day -
                    1;

                int row =
                    cellIndex / 7;

                int col =
                    cellIndex % 7;


                DateTime date =
                    new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        day);


                DataGridViewCell cell =
                    dgv[col, row];


                cell.Tag =
                    date;


                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);


                // ====================================================
                // 날짜 숫자 색상
                //
                // 공휴일 = 빨강
                // 일요일 = 빨강
                // 토요일 = 테마에 맞는 파랑
                // 일반 = 테마 글자색
                // ====================================================

                cell.Style.ForeColor =
                    isHoliday
                        ? Color.Red
                        : col switch
                        {
                            0 =>
                                Color.Red,

                            6 =>
                                GetSaturdayColor(),

                            _ =>
                                UiThemeService.TextColor
                        };


                string cellText =
                    day.ToString();


                if (isHoliday)
                {
                    cellText +=
                        $"\n{holidayName}";
                }


                cell.Value =
                    cellText;


                ApplyDateCellBackground(
                    cell,
                    date,
                    null,
                    isHoliday);
            }


            AdjustRowHeights();
        }


        // ============================================================
        // 주간 보기
        // ============================================================

        private void UpdateWeekView()
        {
            DateTime startOfWeek =
                currentDate.Date.AddDays(
                    -(int)currentDate.DayOfWeek);


            string[] dayNames =
            {
                "일",
                "월",
                "화",
                "수",
                "목",
                "금",
                "토"
            };


            string[] headers =
                new string[7];


            for (int i = 0;
                 i < 7;
                 i++)
            {
                DateTime date =
                    startOfWeek.AddDays(i);

                headers[i] =
                    $"{dayNames[i]}\n{date:M/d}";
            }


            ConfigureGrid(
                headers,
                1,
                48);


            dgv.RowHeadersVisible =
                false;

            dgv.ScrollBars =
                ScrollBars.None;


            // 일요일
            dgv.Columns[0]
                .HeaderCell
                .Style
                .ForeColor =
                Color.Red;


            // 토요일
            dgv.Columns[6]
                .HeaderCell
                .Style
                .ForeColor =
                GetSaturdayColor();


            for (int col = 0;
                 col < 7;
                 col++)
            {
                DateTime date =
                    startOfWeek.AddDays(
                        col);


                bool hasSchedule =
                    scheduleMap.TryGetValue(
                        date,
                        out List<CalendarScheduleEntry>? schedules)
                    &&
                    schedules.Count > 0;


                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);


                DataGridViewCell cell =
                    dgv[col, 0];


                cell.Tag =
                    date;


                string cellText =
                    hasSchedule
                        ? string.Join(
                            "\n",
                            schedules!
                                .OrderBy(
                                    item =>
                                        item.StartHour)
                                .Select(
                                    item =>
                                        $"{item.StartHour:00}:00 " +
                                        $"[{PersonalCategoryStores.Calendar.Get(item.CategoryId).Name}] " +
                                        $"{item.Text}"))
                        : "일정 없음";


                if (isHoliday)
                {
                    cellText =
                        holidayName +
                        (
                            hasSchedule
                                ? $"\n{cellText}"
                                : ""
                        );
                }


                cell.Value =
                    cellText;


                cell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;


                // ====================================================
                // 글꼴
                // ====================================================

                cell.Style.Font =
                    hasSchedule
                        ? AppFontService.CreateFont(
                            9.5F,
                            FontStyle.Bold)
                        : AppFontService.CreateFont(
                            9F,
                            FontStyle.Regular);


                // ====================================================
                // 글자색
                // ====================================================

                cell.Style.ForeColor =
                    isHoliday
                        ? Color.Red
                        : col switch
                        {
                            0 =>
                                Color.Red,

                            6 =>
                                GetSaturdayColor(),

                            _ =>
                                !hasSchedule
                                    ? Color.Gray
                                    : UiThemeService.TextColor
                        };


                ApplyDateCellBackground(
                    cell,
                    date,
                    hasSchedule
                        ? schedules![0]
                        : null,
                    isHoliday);
            }


            AdjustRowHeights();
        }


        // ============================================================
        // 일간 보기
        // ============================================================

        private void UpdateDayView()
        {
            DateTime date =
                currentDate.Date;


            bool isHoliday =
                holidayMap.TryGetValue(
                    date,
                    out string holidayName);


            string headerText =
                date.ToString(
                    "yyyy년 M월 d일 dddd");


            if (isHoliday)
            {
                headerText +=
                    $" - {holidayName}";
            }


            string[] headers =
            {
                headerText
            };


            ConfigureGrid(
                headers,
                CalendarEndHour -
                CalendarStartHour,
                48);


            dgv.RowHeadersVisible =
                true;

            dgv.RowHeadersWidth =
                62;

            dgv.ScrollBars =
                ScrollBars.Vertical;


            dgv.RowHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.TopCenter;


            dgv.RowHeadersDefaultCellStyle.ForeColor =
                UiThemeService.TextColor;


            // 시간 헤더 글꼴
            dgv.RowHeadersDefaultCellStyle.Font =
                AppFontService.CreateFont(
                    8.5F,
                    FontStyle.Regular);


            // 공휴일이면 상단 날짜 제목도 빨간색
            dgv.Columns[0]
                .HeaderCell
                .Style
                .ForeColor =
                isHoliday
                    ? Color.Red
                    : UiThemeService.TextColor;


            for (int row = 0;
                 row < dgv.Rows.Count;
                 row++)
            {
                dgv.Rows[row]
                    .HeaderCell
                    .Value =
                    $"{CalendarStartHour + row:00}:00";


                dgv.Rows[row].Height =
                    48;


                DataGridViewCell cell =
                    dgv[0, row];


                cell.Tag =
                    date.AddHours(
                        CalendarStartHour +
                        row);


                cell.Style.Alignment =
                    DataGridViewContentAlignment.TopLeft;


                cell.Style.Font =
                    AppFontService.CreateFont(
                        9F,
                        FontStyle.Regular);


                // 기본 날짜 배경
                if (date.Date ==
                    DateTime.Today)
                {
                    cell.Style.BackColor =
                        GetTodayColor();
                }
                else if (isHoliday)
                {
                    cell.Style.BackColor =
                        GetHolidayColor();
                }
                else
                {
                    cell.Style.BackColor =
                        UiThemeService.InputColor;
                }


                cell.Style.ForeColor =
                    isHoliday
                        ? Color.Red
                        : UiThemeService.TextColor;
            }


            // ========================================================
            // 일정 표시
            // ========================================================

            if (scheduleMap.TryGetValue(
                    date,
                    out List<CalendarScheduleEntry>? schedules))
            {
                foreach (var schedule
                    in schedules
                        .OrderBy(
                            item =>
                                item.StartHour)
                        .ThenBy(
                            item =>
                                item.EndHour))
                {
                    int startHour =
                        Math.Clamp(
                            schedule.StartHour,
                            CalendarStartHour,
                            CalendarEndHour -
                            1);


                    int endHour =
                        Math.Clamp(
                            schedule.EndHour,
                            startHour + 1,
                            CalendarEndHour);


                    int startRow =
                        startHour -
                        CalendarStartHour;


                    int endRow =
                        endHour -
                        CalendarStartHour;


                    for (int r = startRow;
                         r < endRow;
                         r++)
                    {
                        DataGridViewCell cell =
                            dgv[0, r];


                        cell.Style.BackColor =
                            PersonalCategoryStores.Calendar
                                .GetScheduleBackgroundColor(
                                    schedule.CategoryId,
                                    schedule.CustomColorArgb);


                        cell.Style.ForeColor =
                            PersonalCategoryStores.Calendar
                                .GetScheduleAccentColor(
                                    schedule.CategoryId,
                                    schedule.CustomColorArgb);


                        if (r == startRow)
                        {
                            string categoryName =
                                PersonalCategoryStores.Calendar
                                    .Get(
                                        schedule.CategoryId)
                                    .Name;


                            string scheduleText =
                                $"{startHour:00}:00~{endHour:00}:00 " +
                                $"[{categoryName}] " +
                                $"{schedule.Text}";


                            cell.Value =
                                string.IsNullOrWhiteSpace(
                                    cell.Value?.ToString())
                                    ? scheduleText
                                    : $"{cell.Value}\n{scheduleText}";


                            cell.Style.Font =
                                AppFontService.CreateFont(
                                    9F,
                                    FontStyle.Bold);
                        }
                    }
                }
            }


            AdjustRowHeights();
        }


        // ============================================================
        // 행 높이
        // ============================================================

        private void AdjustRowHeights()
        {
            if (dgv.Rows.Count == 0)
                return;


            if (viewMode ==
                CalendarViewMode.Day)
            {
                foreach (
                    DataGridViewRow row
                    in dgv.Rows)
                {
                    row.Height =
                        48;
                }

                return;
            }


            int availableHeight =
                dgv.Height -
                dgv.ColumnHeadersHeight -
                2;


            if (availableHeight <= 0)
                return;


            int rowHeight =
                availableHeight /
                dgv.Rows.Count;


            foreach (
                DataGridViewRow row
                in dgv.Rows)
            {
                row.Height =
                    rowHeight;
            }
        }


        // ============================================================
        // 날짜 셀 배경
        // ============================================================

        private void ApplyDateCellBackground(
            DataGridViewCell cell,
            DateTime date,
            CalendarScheduleEntry? schedule,
            bool isHoliday)
        {
            // 오늘
            if (date.Date ==
                DateTime.Today)
            {
                cell.Style.BackColor =
                    GetTodayColor();
            }

            // 공휴일
            else if (isHoliday)
            {
                cell.Style.BackColor =
                    GetHolidayColor();
            }

            // 일정
            else if (schedule is not null)
            {
                cell.Style.BackColor =
                    PersonalCategoryStores.Calendar
                        .GetScheduleBackgroundColor(
                            schedule.CategoryId,
                            schedule.CustomColorArgb);
            }

            // 기본
            else
            {
                cell.Style.BackColor =
                    UiThemeService.InputColor;
            }
        }


        // ============================================================
        // 테마별 오늘 색상
        // ============================================================

        private static Color GetTodayColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        85,
                        75,
                        135),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        255,
                        218,
                        230),

                AppTheme.Mint =>
                    Color.FromArgb(
                        205,
                        238,
                        225),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        225,
                        215,
                        245),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        235,
                        215,
                        190),

                _ =>
                    Color.FromArgb(
                        220,
                        235,
                        250)
            };
        }


        // ============================================================
        // 테마별 공휴일 색상
        // ============================================================

        private static Color GetHolidayColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        85,
                        55,
                        60),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        255,
                        225,
                        230),

                AppTheme.Mint =>
                    Color.FromArgb(
                        255,
                        235,
                        235),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        250,
                        228,
                        238),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        250,
                        225,
                        215),

                _ =>
                    Color.MistyRose
            };
        }


        // ============================================================
        // 테마별 토요일 색상
        // ============================================================

        private static Color GetSaturdayColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        100,
                        180,
                        255),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        70,
                        140,
                        220),

                AppTheme.Mint =>
                    Color.FromArgb(
                        60,
                        140,
                        210),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        80,
                        145,
                        230),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        65,
                        135,
                        210),

                _ =>
                    Color.DeepSkyBlue
            };
        }


        // ============================================================
        // 월간 달력 셀 직접 그리기
        // ============================================================

        private void DgvCalendar_CellPainting(
            object? sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (viewMode !=
                    CalendarViewMode.Month
                ||
                e.RowIndex < 0
                ||
                e.ColumnIndex < 0)
            {
                return;
            }


            DataGridViewCell cell =
                dgv[
                    e.ColumnIndex,
                    e.RowIndex];


            if (cell.Tag
                is not DateTime date)
            {
                return;
            }


            e.PaintBackground(
                e.CellBounds,
                false);


            e.Paint(
                e.ClipBounds,
                DataGridViewPaintParts.Border);


            holidayMap.TryGetValue(
                date.Date,
                out var holidayName);


            scheduleMap.TryGetValue(
                date.Date,
                out var schedules);


            ddayMap.TryGetValue(
                date.Date,
                out var ddayText);


            var displaySchedules =
                schedules is null
                    ? new List<CalendarScheduleEntry>()
                    : schedules.ToList();


            monthCellRenderer.Draw(
                e.Graphics,
                e.CellBounds,
                date,
                holidayName,
                displaySchedules,
                e.CellStyle.Font ??
                dgv.Font,
                e.CellStyle.ForeColor,
                (
                    e.State &
                    DataGridViewElementStates.Selected
                ) != 0);


            e.Handled =
                true;
        }


        // ============================================================
        // 날짜 더블클릭
        // ============================================================

        private void DgvCalendar_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }


            DataGridViewCell cell =
                dgv[
                    e.ColumnIndex,
                    e.RowIndex];


            if (cell.Tag
                is not DateTime selectedDate)
            {
                return;
            }


            DateTime keyDate =
                selectedDate.Date;


            currentDate =
                keyDate;


            scheduleMap.TryGetValue(
                keyDate,
                out var currentSchedules);


            // 기존 일정
            var originalSchedules =
                (
                    currentSchedules
                    ??
                    new List<CalendarScheduleEntry>()
                )
                .ToList();


            using var dialog =
                new CalenderScheduleListForm(
                    keyDate,
                    originalSchedules);


            if (dialog.ShowDialog(
                    FindForm())
                != DialogResult.OK)
            {
                return;
            }


            var updatedSchedules =
                dialog
                    .Schedules
                    .ToList();


            try
            {
                // ====================================================
                // 1. 삭제된 일정
                // ====================================================

                foreach (
                    var original
                    in originalSchedules)
                {
                    if (original.CalId is null)
                        continue;


                    bool stillExists =
                        updatedSchedules.Any(
                            updated =>
                                updated.CalId ==
                                original.CalId);


                    if (!stillExists)
                    {
                        calendarDbRepository.Delete(
                            loggedInUserId,
                            calendarId,
                            original);


                        // 현재 D-Day 일정이면
                        // D-Day도 같이 삭제
                        if (
                            ddayMap.TryGetValue(
                                keyDate,
                                out var ddayTitle)
                            &&
                            ddayTitle ==
                            original.Text)
                        {
                            calendarDbRepository.DeleteDday(
                                loggedInUserId,
                                keyDate);


                            ddayMap.Remove(
                                keyDate);
                        }
                    }
                }


                // ====================================================
                // 2. 새 일정 추가
                // ====================================================

                foreach (
                    var schedule
                    in updatedSchedules)
                {
                    if (schedule.CalId is null)
                    {
                        int newCalId =
                            calendarDbRepository.Add(
                                loggedInUserId,
                                calendarId,
                                schedule,
                                keyDate);


                        schedule.CalId =
                            newCalId;
                    }
                }


                // ====================================================
                // 3. 기존 일정 수정
                // ====================================================

                foreach (
                    var schedule
                    in updatedSchedules)
                {
                    if (schedule.CalId
                        is not null)
                    {
                        calendarDbRepository.Update(
                            loggedInUserId,
                            calendarId,
                            schedule,
                            keyDate);
                    }
                }


                // ====================================================
                // 4. 메모리 일정 갱신
                // ====================================================

                if (updatedSchedules.Count == 0)
                {
                    scheduleMap.Remove(
                        keyDate);
                }
                else
                {
                    scheduleMap[
                        keyDate] =
                        updatedSchedules;
                }


                UpdateView();


                DateOrScheduleChanged?.Invoke(
                    this,
                    EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"일정을 DB에 저장하는 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


                LoadSchedules();

                UpdateView();
            }
        }


        // ============================================================
        // DB 일정 불러오기
        // ============================================================

        public void LoadSchedules()
        {
            try
            {
                scheduleMap =
                    calendarDbRepository.Load(
                        loggedInUserId,
                        calendarId);


                ddayMap =
                    calendarDbRepository.LoadDdays(
                        loggedInUserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"일정을 불러오는 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 불러오기 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


                scheduleMap =
                    new Dictionary<
                        DateTime,
                        List<CalendarScheduleEntry>>();


                ddayMap =
                    new Dictionary<
                        DateTime,
                        string>();
            }
        }


        // ============================================================
        // 현재 테마 적용
        // ============================================================

        public void ApplyCurrentTheme()
        {
            // CalendarControl 자체
            BackColor =
                UiThemeService.BackgroundColor;

            ForeColor =
                UiThemeService.TextColor;


            // DataGridView
            dgv.BackgroundColor =
                UiThemeService.BackgroundColor;

            dgv.GridColor =
                GetThemeBorderColor();

            dgv.BorderStyle =
                BorderStyle.None;


            // 일반 셀
            dgv.DefaultCellStyle.BackColor =
                UiThemeService.InputColor;

            dgv.DefaultCellStyle.ForeColor =
                UiThemeService.TextColor;

            dgv.DefaultCellStyle.SelectionBackColor =
                UiThemeService.PrimaryColor;

            dgv.DefaultCellStyle.SelectionForeColor =
                UiThemeService.TextColor;


            // 요일 헤더
            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                UiThemeService.SurfaceColor;

            dgv.ColumnHeadersDefaultCellStyle.ForeColor =
                UiThemeService.TextColor;

            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                UiThemeService.SurfaceColor;

            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                UiThemeService.TextColor;


            // 시간 헤더
            dgv.RowHeadersDefaultCellStyle.BackColor =
                UiThemeService.SurfaceColor;

            dgv.RowHeadersDefaultCellStyle.ForeColor =
                UiThemeService.TextColor;


            // 다시 그림
            UpdateView();

            dgv.Invalidate();
        }


        // ============================================================
        // ★ 현재 글꼴 적용
        // ============================================================

        public void ApplyCurrentFont()
        {
            // 일반 셀
            dgv.DefaultCellStyle.Font =
                AppFontService.CreateFont(
                    9.5F,
                    FontStyle.Bold);


            // 요일/날짜 헤더
            dgv.ColumnHeadersDefaultCellStyle.Font =
                AppFontService.CreateFont(
                    9F,
                    FontStyle.Bold);


            // 시간 헤더
            dgv.RowHeadersDefaultCellStyle.Font =
                AppFontService.CreateFont(
                    8.5F,
                    FontStyle.Regular);


            // 현재 화면에 만들어진 각 셀의
            // 개별 폰트도 새로 적용하기 위해
            // View를 다시 생성
            UpdateView();


            dgv.Invalidate();
        }


        // ============================================================
        // 테마별 테두리 색상
        // ============================================================

        private Color GetThemeBorderColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        75,
                        75,
                        75),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        243,
                        198,
                        212),

                AppTheme.Mint =>
                    Color.FromArgb(
                        190,
                        225,
                        213),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        210,
                        198,
                        235),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        220,
                        202,
                        180),

                _ =>
                    Color.FromArgb(
                        210,
                        210,
                        210)
            };
        }
    }
}