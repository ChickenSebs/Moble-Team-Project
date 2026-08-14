using calendar4.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml.Linq;

namespace calendar4
{
    public partial class DiaryControl : UserControl
    {
        private DataGridView dgv;

        private readonly int loggedInUserId;
        private readonly DiaryDbRepository diaryDbRepository = new();
        private readonly HolidayService holidayService = new();

        private Dictionary<string, DiaryEntry> diaryMap =
            new Dictionary<string, DiaryEntry>();

        private Dictionary<DateTime, string> holidayMap =
            new Dictionary<DateTime, string>();

        private DateTime currentDate = DateTime.Now;

        private CalendarControl.CalendarViewMode viewMode =
            CalendarControl.CalendarViewMode.Month;

        public event EventHandler DataChanged;
        public event EventHandler DateOrScheduleChanged;


        public DiaryControl(int userId)
        {
            loggedInUserId = userId;

            InitializeUserControl();
            LoadDiaries();
            UpdateView();
        }


        // =========================================================
        // 기본 UI 생성
        // =========================================================
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

            dgv.DefaultCellStyle.Font =
                new Font(
                    "맑은 고딕",
                    9,
                    FontStyle.Regular);

            dgv.DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            dgv.Resize +=
                (s, ev) => AdjustRowHeights();

            dgv.CellDoubleClick +=
                DgvDiary_CellDoubleClick;

            dgv.MouseEnter +=
                (s, e) => dgv.Focus();

            dgv.MouseWheel +=
                DgvDiary_MouseWheel;

            Controls.Add(dgv);

            Load += DiaryControl_Load;
        }


        // =========================================================
        // 마우스 휠
        // =========================================================
        private void DgvDiary_MouseWheel(
            object? sender,
            MouseEventArgs e)
        {
            int moveDirection =
                e.Delta > 0 ? -1 : 1;

            switch (viewMode)
            {
                case CalendarControl.CalendarViewMode.Month:

                    currentDate =
                        currentDate.AddMonths(moveDirection);

                    break;


                case CalendarControl.CalendarViewMode.Week:

                    currentDate =
                        currentDate.AddDays(
                            moveDirection * 7);

                    break;


                case CalendarControl.CalendarViewMode.Day:

                    currentDate =
                        currentDate.AddDays(
                            moveDirection);

                    break;
            }

            UpdateView();

            _ = LoadHolidaysAsync(
                currentDate.Year,
                currentDate.Month);

            DateOrScheduleChanged?.Invoke(
                this,
                EventArgs.Empty);
        }


        private async void DiaryControl_Load(
            object sender,
            EventArgs e)
        {
            await LoadHolidaysAsync(
                currentDate.Year,
                currentDate.Month);
        }


        // =========================================================
        // 날짜 / 보기 설정
        // =========================================================
        public void SetTargetDate(DateTime date)
        {
            currentDate = date;

            UpdateView();

            _ = LoadHolidaysAsync(
                currentDate.Year,
                currentDate.Month);
        }


        public DateTime GetTargetDate()
        {
            return currentDate;
        }


        public void SetViewMode(
            CalendarControl.CalendarViewMode newMode)
        {
            viewMode = newMode;

            UpdateView();
        }


        public CalendarControl.CalendarViewMode GetViewMode()
        {
            return viewMode;
        }


        public Dictionary<string, DiaryEntry> GetDiaryMap()
        {
            return diaryMap;
        }


        public void UpdateGrid()
        {
            UpdateView();
        }


        public void SetHolidayMap(
            Dictionary<DateTime, string> holidays)
        {
            holidayMap =
                holidays != null
                    ? new Dictionary<DateTime, string>(holidays)
                    : new Dictionary<DateTime, string>();

            UpdateView();
        }


        // =========================================================
        // 공휴일
        // =========================================================
        private async Task LoadHolidaysAsync(
            int year,
            int month)
        {
            try
            {
                holidayMap =
                    await holidayService.GetHolidaysAsync(
                        year,
                        month);

                UpdateView();
            }
            catch
            {
                holidayMap.Clear();

                UpdateView();
            }
        }


        // =========================================================
        // 화면 갱신
        // =========================================================
        public void UpdateView()
        {
            switch (viewMode)
            {
                case CalendarControl.CalendarViewMode.Week:

                    UpdateWeekView();

                    break;


                case CalendarControl.CalendarViewMode.Day:

                    UpdateDayView();

                    break;


                default:

                    UpdateMonthView();

                    break;
            }

            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }


        // =========================================================
        // DataGridView 기본 설정
        // =========================================================
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

            for (
                int index = 0;
                index < headers.Length;
                index++)
            {
                int columnIndex =
                    dgv.Columns.Add(
                        $"col{index}",
                        headers[index]);

                dgv.Columns[columnIndex].SortMode =
                    DataGridViewColumnSortMode.NotSortable;

                dgv.Columns[columnIndex].FillWeight =
                    1;
            }

            dgv.Rows.Add(rowCount);

            dgv.ResumeLayout();
        }


        // =========================================================
        // 월간 보기
        // =========================================================
        private void UpdateMonthView()
        {
            int year =
                currentDate.Year;

            int month =
                currentDate.Month;

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
                Color.Blue;


            DateTime firstDay =
                new DateTime(
                    year,
                    month,
                    1);

            int totalDays =
                DateTime.DaysInMonth(
                    year,
                    month);

            int startDayOfWeek =
                (int)firstDay.DayOfWeek;


            for (
                int day = 1;
                day <= totalDays;
                day++)
            {
                int cellIndex =
                    startDayOfWeek +
                    day -
                    1;

                int row =
                    cellIndex / 7;

                int column =
                    cellIndex % 7;


                DateTime date =
                    new DateTime(
                        year,
                        month,
                        day);

                string dateKey =
                    date.ToString(
                        "yyyy-MM-dd");


                DataGridViewCell cell =
                    dgv[column, row];

                cell.Tag =
                    date;


                // 요일별 글씨 색
                cell.Style.ForeColor =
                    column switch
                    {
                        0 => Color.Red,
                        6 => Color.Blue,

                        _ =>
                            UiThemeService.TextColor
                    };


                bool hasDiary =
                    diaryMap.TryGetValue(
                        dateKey,
                        out DiaryEntry diary) &&
                    diary != null;


                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);


                string text =
                    day.ToString();


                if (isHoliday)
                {
                    text +=
                        $"\n🎉 {holidayName}";
                }


                if (hasDiary)
                {
                    string title =
                        string.IsNullOrWhiteSpace(
                            diary.Title)
                            ? "[일기 작성됨]"
                            : diary.Title;

                    text +=
                        $"\n\n📝 {title}";
                }


                cell.Value =
                    text;


                ApplyDateCellBackground(
                    cell,
                    date,
                    hasDiary,
                    isHoliday);
            }

            AdjustRowHeights();
        }


        // =========================================================
        // 주간 보기
        // =========================================================
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


            for (
                int index = 0;
                index < 7;
                index++)
            {
                DateTime date =
                    startOfWeek.AddDays(
                        index);

                headers[index] =
                    $"{dayNames[index]}\n{date:M/d}";
            }


            ConfigureGrid(
                headers,
                1,
                48);


            dgv.RowHeadersVisible =
                false;

            dgv.ScrollBars =
                ScrollBars.None;


            dgv.Columns[0]
                .HeaderCell
                .Style
                .ForeColor =
                Color.Red;

            dgv.Columns[6]
                .HeaderCell
                .Style
                .ForeColor =
                Color.Blue;


            for (
                int column = 0;
                column < 7;
                column++)
            {
                DateTime date =
                    startOfWeek.AddDays(
                        column);


                DataGridViewCell cell =
                    dgv[column, 0];


                bool hasDiary =
                    diaryMap.TryGetValue(
                        date.ToString(
                            "yyyy-MM-dd"),
                        out DiaryEntry diary) &&
                    diary != null;


                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);


                cell.Tag =
                    date;


                string text =
                    "";


                if (isHoliday)
                {
                    text +=
                        $"🎉 {holidayName}\n\n";
                }


                if (hasDiary)
                {
                    text +=
                        $"📝 {DiaryTextFormatter.GetTitle(diary)}\n\n" +
                        DiaryTextFormatter.GetPreview(
                            diary.Content);
                }
                else
                {
                    text +=
                        "작성된 일기 없음";
                }


                cell.Value =
                    text;


                cell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;


                cell.Style.Font =
                    new Font(
                        "맑은 고딕",
                        9F,
                        FontStyle.Regular);


                if (isHoliday)
                {
                    cell.Style.ForeColor =
                        Color.Red;
                }
                else if (!hasDiary)
                {
                    cell.Style.ForeColor =
                        Color.Gray;
                }
                else
                {
                    cell.Style.ForeColor =
                        column switch
                        {
                            0 => Color.Red,

                            6 => Color.Blue,

                            _ =>
                                UiThemeService.TextColor
                        };
                }


                ApplyDateCellBackground(
                    cell,
                    date,
                    hasDiary,
                    isHoliday);
            }

            AdjustRowHeights();
        }


        // =========================================================
        // 일간 보기
        // =========================================================
        private void UpdateDayView()
        {
            DateTime date =
                currentDate.Date;


            ConfigureGrid(
                new[]
                {
                    date.ToString(
                        "yyyy년 M월 d일 dddd")
                },
                1,
                48);


            dgv.RowHeadersVisible =
                false;

            dgv.ScrollBars =
                ScrollBars.None;


            DataGridViewCell cell =
                dgv[0, 0];


            bool hasDiary =
                diaryMap.TryGetValue(
                    date.ToString(
                        "yyyy-MM-dd"),
                    out DiaryEntry diary) &&
                diary != null;


            bool isHoliday =
                holidayMap.TryGetValue(
                    date,
                    out string holidayName);


            cell.Tag =
                date;


            string text =
                "";


            if (isHoliday)
            {
                text +=
                    $"🎉 {holidayName}\n\n";
            }


            if (hasDiary)
            {
                text +=
                    $"📝 {DiaryTextFormatter.GetTitle(diary)}\n\n" +
                    diary.Content;
            }
            else
            {
                text +=
                    "작성된 일기가 없습니다.\n\n" +
                    "더블클릭하여 일기를 작성하세요.";
            }


            cell.Value =
                text;


            cell.Style.Alignment =
                DataGridViewContentAlignment.MiddleCenter;


            cell.Style.Font =
                new Font(
                    "맑은 고딕",
                    11F,
                    hasDiary
                        ? FontStyle.Regular
                        : FontStyle.Bold);


            cell.Style.ForeColor =
                isHoliday
                    ? Color.Red
                    : hasDiary
                        ? UiThemeService.TextColor
                        : Color.Gray;


            ApplyDateCellBackground(
                cell,
                date,
                hasDiary,
                isHoliday);


            AdjustRowHeights();
        }


        // =========================================================
        // 날짜 셀 배경
        // =========================================================
        private static void ApplyDateCellBackground(
            DataGridViewCell cell,
            DateTime date,
            bool hasDiary,
            bool isHoliday)
        {
            // 오늘
            if (date.Date == DateTime.Today)
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

            // 일기가 있는 날짜
            else if (hasDiary)
            {
                cell.Style.BackColor =
                    GetDiaryColor();
            }

            // 일반 날짜
            else
            {
                cell.Style.BackColor =
                    UiThemeService.InputColor;
            }
        }


        // =========================================================
        // 오늘 색상
        // =========================================================
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


        // =========================================================
        // 일기 작성된 날짜 색상
        // =========================================================
        private static Color GetDiaryColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        60,
                        55,
                        75),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        255,
                        239,
                        245),

                AppTheme.Mint =>
                    Color.FromArgb(
                        232,
                        248,
                        241),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        242,
                        236,
                        252),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        248,
                        238,
                        222),

                _ =>
                    Color.FromArgb(
                        255,
                        250,
                        220)
            };
        }


        // =========================================================
        // 공휴일 색상
        // =========================================================
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


        // =========================================================
        // 행 높이
        // =========================================================
        private void AdjustRowHeights()
        {
            if (dgv.Rows.Count == 0)
            {
                return;
            }


            int availableHeight =
                dgv.Height -
                dgv.ColumnHeadersHeight -
                2;


            if (availableHeight <= 0)
            {
                return;
            }


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


        // =========================================================
        // 일기 더블클릭
        // =========================================================
        private void DgvDiary_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (
                e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }


            var cell =
                dgv[
                    e.ColumnIndex,
                    e.RowIndex];


            if (
                cell.Tag
                is not DateTime selectedDate)
            {
                return;
            }


            var key =
                selectedDate.ToString(
                    "yyyy-MM-dd");


            diaryMap.TryGetValue(
                key,
                out var currentEntry);


            using var dialog =
                new DiaryEntryForm(
                    selectedDate,
                    currentEntry);


            var result =
                dialog.ShowDialog(
                    FindForm());


            try
            {
                // =====================================
                // 저장
                // =====================================
                if (result == DialogResult.OK)
                {
                    // 내용이 전부 비어 있으면 삭제 처리
                    if (dialog.IsEmpty)
                    {
                        if (currentEntry != null)
                        {
                            diaryDbRepository.Delete(
                                loggedInUserId,
                                currentEntry);
                        }

                        diaryMap.Remove(
                            key);
                    }

                    else
                    {
                        // ---------------------------------
                        // 새 일기
                        // ---------------------------------
                        if (currentEntry == null)
                        {
                            var newEntry =
                                new DiaryEntry
                                {
                                    DateStr =
                                        key,

                                    Title =
                                        dialog.DiaryTitle,

                                    Content =
                                        dialog.DiaryContent
                                };


                            int newDiaryId =
                                diaryDbRepository.Add(
                                    loggedInUserId,
                                    newEntry);


                            newEntry.DiaryId =
                                newDiaryId;


                            diaryMap[key] =
                                newEntry;
                        }

                        // ---------------------------------
                        // 기존 일기 수정
                        // ---------------------------------
                        else
                        {
                            currentEntry.Title =
                                dialog.DiaryTitle;


                            currentEntry.Content =
                                dialog.DiaryContent;


                            currentEntry.DateStr =
                                key;


                            diaryDbRepository.Update(
                                loggedInUserId,
                                currentEntry);


                            diaryMap[key] =
                                currentEntry;
                        }
                    }
                }

                // =====================================
                // 삭제
                // =====================================
                else if (
                    result ==
                    DialogResult.Yes)
                {
                    if (currentEntry != null)
                    {
                        diaryDbRepository.Delete(
                            loggedInUserId,
                            currentEntry);
                    }


                    diaryMap.Remove(
                        key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"일기를 DB에 저장하는 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


                // 문제가 생기면 DB 기준으로 복구
                LoadDiaries();
            }


            UpdateGrid();


            DataChanged?.Invoke(
                this,
                EventArgs.Empty);
        }


        // =========================================================
        // DB에서 일기 불러오기
        // =========================================================
        public void LoadDiaries()
        {
            try
            {
                diaryMap =
                    diaryDbRepository.Load(
                        loggedInUserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"일기를 불러오는 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 불러오기 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


                diaryMap =
                    new Dictionary<string, DiaryEntry>();
            }
        }


        // =========================================================
        // 현재 테마 적용
        // =========================================================
        public void ApplyCurrentTheme()
        {
            // DiaryControl 자체
            BackColor =
                UiThemeService.BackgroundColor;

            ForeColor =
                UiThemeService.TextColor;


            // DataGridView 전체 배경
            dgv.BackgroundColor =
                UiThemeService.BackgroundColor;

            dgv.GridColor =
                GetThemeBorderColor();

            dgv.BorderStyle =
                BorderStyle.None;


            // 기본 셀
            dgv.DefaultCellStyle.BackColor =
                UiThemeService.InputColor;

            dgv.DefaultCellStyle.ForeColor =
                UiThemeService.TextColor;


            // 선택 셀
            dgv.DefaultCellStyle.SelectionBackColor =
                GetSelectedColor();

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


            // 다시 테마 기준으로 화면 생성
            UpdateView();

            dgv.Invalidate();
        }


        // =========================================================
        // 달력 선 색상
        // =========================================================
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


        // =========================================================
        // 선택 셀 색상
        // =========================================================
        private Color GetSelectedColor()
        {
            return UiThemeService.CurrentTheme switch
            {
                AppTheme.Dark =>
                    Color.FromArgb(
                        90,
                        80,
                        145),

                AppTheme.Blossom =>
                    Color.FromArgb(
                        248,
                        190,
                        210),

                AppTheme.Mint =>
                    Color.FromArgb(
                        185,
                        230,
                        215),

                AppTheme.Lavender =>
                    Color.FromArgb(
                        215,
                        200,
                        240),

                AppTheme.Cozy =>
                    Color.FromArgb(
                        225,
                        205,
                        180),

                _ =>
                    Color.FromArgb(
                        210,
                        225,
                        250)
            };
        }
    }
}