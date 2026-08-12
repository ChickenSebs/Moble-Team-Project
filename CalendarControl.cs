using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Linq;

namespace calendar4
{
    public partial class CalendarControl : UserControl
    {
        private Dictionary<DateTime, string> holidayMap =
            new Dictionary<DateTime, string>();

        private DataGridView dgv;
        private readonly CalendarScheduleRepository scheduleRepository = new();
        private readonly CalendarMonthCellRenderer monthCellRenderer =
            new(PersonalCategoryStores.Calendar);

        private Dictionary<DateTime, List<CalendarScheduleEntry>> scheduleMap =
            new Dictionary<DateTime, List<CalendarScheduleEntry>>();

        private DateTime currentDate = DateTime.Now;
        private CalendarViewMode viewMode =
            CalendarViewMode.Month;

        private const int CalendarStartHour = 8;
        private const int CalendarEndHour = 22;

        public event EventHandler DateOrScheduleChanged;

        public enum CalendarViewMode
        {
            Month,
            Week,
            Day
        }

        public CalendarControl()
        {
            InitializeUserControl();
            LoadSchedules();
        }

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

            // ✨ 마우스가 캘린더 위로 올라오면 포커스를 주어 휠 이벤트를 받을 수 있게 함
            dgv.MouseEnter += (s, e) => dgv.Focus();

            // ✨ 마우스 휠 이벤트 추가
            dgv.MouseWheel += DgvCalendar_MouseWheel;

            Controls.Add(dgv);
        }

        // ✨ 휠 스크롤 처리 메서드 추가
        private void DgvCalendar_MouseWheel(object? sender, MouseEventArgs e)
        {
            // 위로 굴리면 음수(-1)로 과거(이전 달/주/일) 이동, 아래로 굴리면 양수(1)로 미래 이동
            int moveDirection = e.Delta > 0 ? -1 : 1;

            switch (viewMode)
            {
                case CalendarViewMode.Month:
                    currentDate = currentDate.AddMonths(moveDirection);
                    break;

                case CalendarViewMode.Week:
                    currentDate = currentDate.AddDays(moveDirection * 7);
                    break;

                case CalendarViewMode.Day:
                    currentDate = currentDate.AddDays(moveDirection);
                    break;
            }

            UpdateView();
            DateOrScheduleChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetViewMode(CalendarViewMode newMode)
        {
            viewMode = newMode;
            UpdateView();
        }

        public void SetTargetDate(DateTime date)
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

            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

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

                dgv.Columns[idx].FillWeight = 1;
            }

            dgv.Rows.Add(rowCount);

            dgv.ResumeLayout();
        }

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

            dgv.RowHeadersVisible = false;
            dgv.ScrollBars = ScrollBars.None;

            dgv.Columns[0].HeaderCell.Style.ForeColor =
                Color.Red;

            dgv.Columns[6].HeaderCell.Style.ForeColor =
                Color.Blue;

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
                    startDayOfWeek + day - 1;

                int row = cellIndex / 7;
                int col = cellIndex % 7;

                DateTime date =
                    new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        day);

                DataGridViewCell cell =
                    dgv[col, row];

                cell.Tag = date;

                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);

                cell.Style.ForeColor =
                    col switch
                    {
                        0 => Color.Red,
                        6 => Color.Blue,
                        _ => Color.Black
                    };

                string cellText = day.ToString();

                if (isHoliday)
                    cellText +=
                        $"\n{holidayName}";

                cell.Value = cellText;

                ApplyDateCellBackground(
                    cell,
                    date,
                    null,
                    isHoliday);
            }

            AdjustRowHeights();
        }

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

            for (int i = 0; i < 7; i++)
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

            dgv.RowHeadersVisible = false;
            dgv.ScrollBars = ScrollBars.None;

            dgv.Columns[0].HeaderCell.Style.ForeColor =
                Color.Red;

            dgv.Columns[6].HeaderCell.Style.ForeColor =
                Color.Blue;

            for (int col = 0; col < 7; col++)
            {
                DateTime date =
                    startOfWeek.AddDays(col);

                bool hasSchedule =
                    scheduleMap.TryGetValue(date, out List<CalendarScheduleEntry>? schedules) &&
                    schedules.Count > 0;

                bool isHoliday =
                    holidayMap.TryGetValue(
                        date.Date,
                        out string holidayName);

                DataGridViewCell cell =
                    dgv[col, 0];

                cell.Tag = date;

                string cellText = hasSchedule
                    ? string.Join("\n", schedules!
                        .OrderBy(item => item.StartHour)
                        .Select(item => $"{item.StartHour:00}:00 " +
                            $"[{PersonalCategoryStores.Calendar.Get(item.CategoryId).Name}] {item.Text}"))
                    : "일정 없음";

                if (isHoliday)
                {
                    cellText =
                        holidayName +
                        (hasSchedule
                            ? $"\n{cellText}"
                            : "");
                }

                cell.Value = cellText;

                cell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                cell.Style.Font =
                    hasSchedule
                    ? new Font(
                        "맑은 고딕",
                        9.5F,
                        FontStyle.Bold)
                    : new Font(
                        "맑은 고딕",
                        9F,
                        FontStyle.Regular);

                cell.Style.ForeColor =
                    isHoliday
                    ? Color.Red
                    : !hasSchedule
                        ? Color.Gray
                        : col switch
                        {
                            0 => Color.Red,
                            6 => Color.Blue,
                            _ => Color.Black
                        };

                ApplyDateCellBackground(
                    cell,
                    date,
                    hasSchedule ? schedules![0] : null,
                    isHoliday);
            }

            AdjustRowHeights();
        }

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
                headerText +=
                    $" - {holidayName}";

            string[] headers =
            {
                headerText
            };

            ConfigureGrid(
                headers,
                CalendarEndHour - CalendarStartHour,
                48);

            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 62;
            dgv.ScrollBars = ScrollBars.Vertical;

            dgv.RowHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.TopCenter;

            dgv.RowHeadersDefaultCellStyle.ForeColor =
                Color.DimGray;

            dgv.RowHeadersDefaultCellStyle.Font =
                new Font(
                    "맑은 고딕",
                    8.5F,
                    FontStyle.Regular);

            for (int row = 0;
                 row < dgv.Rows.Count;
                 row++)
            {
                dgv.Rows[row].HeaderCell.Value =
                    $"{CalendarStartHour + row:00}:00";

                dgv.Rows[row].Height = 48;

                DataGridViewCell cell =
                    dgv[0, row];

                cell.Tag =
                    date.AddHours(
                        CalendarStartHour + row);

                cell.Style.Alignment =
                    DataGridViewContentAlignment.TopLeft;

                cell.Style.Font =
                    new Font(
                        "맑은 고딕",
                        9F,
                        FontStyle.Regular);

                cell.Style.BackColor =
                    date.Date == DateTime.Today
                    ? Color.FromArgb(
                        242,
                        248,
                        255)
                    : Color.White;

                if (isHoliday)
                    cell.Style.BackColor =
                        Color.MistyRose;
            }

            if (scheduleMap.TryGetValue(date, out List<CalendarScheduleEntry>? schedules))
            {
                foreach (var schedule in schedules
                    .OrderBy(item => item.StartHour)
                    .ThenBy(item => item.EndHour))
                {
                    int startHour =
                        Math.Clamp(
                            schedule.StartHour,
                            CalendarStartHour,
                            CalendarEndHour - 1);

                    int endHour =
                        Math.Clamp(
                            schedule.EndHour,
                            startHour + 1,
                            CalendarEndHour);

                    int startRow =
                        startHour - CalendarStartHour;

                    int endRow =
                        endHour - CalendarStartHour;

                    for (int r = startRow;
                         r < endRow;
                         r++)
                    {
                        DataGridViewCell cell =
                            dgv[0, r];

                        cell.Style.BackColor =
                            PersonalCategoryStores.Calendar.GetScheduleBackgroundColor(
                                schedule.CategoryId,
                                schedule.CustomColorArgb);

                        cell.Style.ForeColor =
                            PersonalCategoryStores.Calendar.GetScheduleAccentColor(
                                schedule.CategoryId,
                                schedule.CustomColorArgb);

                        if (r == startRow)
                        {
                            var categoryName = PersonalCategoryStores.Calendar.Get(schedule.CategoryId).Name;
                            var scheduleText =
                                $"{startHour:00}:00~{endHour:00}:00 [{categoryName}] {schedule.Text}";
                            cell.Value = string.IsNullOrWhiteSpace(cell.Value?.ToString())
                                ? scheduleText
                                : $"{cell.Value}\n{scheduleText}";

                            cell.Style.Font =
                                new Font(
                                    "맑은 고딕",
                                    9F,
                                    FontStyle.Bold);
                        }
                    }
                }
            }

            AdjustRowHeights();
        }

        private void AdjustRowHeights()
        {
            if (dgv.Rows.Count == 0)
                return;

            if (viewMode ==
                CalendarViewMode.Day)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                    row.Height = 48;

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

            foreach (DataGridViewRow row in dgv.Rows)
                row.Height = rowHeight;
        }

        private void ApplyDateCellBackground(
            DataGridViewCell cell,
            DateTime date,
            CalendarScheduleEntry? schedule,
            bool isHoliday)
        {
            if (date.Date == DateTime.Today)
            {
                cell.Style.BackColor =
                    Color.LightSkyBlue;
            }
            else if (isHoliday)
            {
                cell.Style.BackColor =
                    Color.MistyRose;
            }
            else if (schedule is not null)
            {
                cell.Style.BackColor =
                    PersonalCategoryStores.Calendar.GetScheduleBackgroundColor(
                        schedule.CategoryId,
                        schedule.CustomColorArgb);
            }
            else
            {
                cell.Style.BackColor =
                    Color.White;
            }
        }

        private void DgvCalendar_CellPainting(
            object? sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (viewMode != CalendarViewMode.Month ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0)
                return;

            var cell = dgv[e.ColumnIndex, e.RowIndex];
            if (cell.Tag is not DateTime date)
                return;

            e.PaintBackground(e.CellBounds, false);
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);

            holidayMap.TryGetValue(date.Date, out var holidayName);
            scheduleMap.TryGetValue(date.Date, out var schedules);

            monthCellRenderer.Draw(
                e.Graphics,
                e.CellBounds,
                date,
                holidayName,
                schedules is null
                    ? Array.Empty<CalendarScheduleEntry>()
                    : schedules,
                e.CellStyle.Font ?? dgv.Font,
                e.CellStyle.ForeColor,
                (e.State & DataGridViewElementStates.Selected) != 0);

            e.Handled = true;
        }

        private void DgvCalendar_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var cell = dgv[e.ColumnIndex, e.RowIndex];
            if (cell.Tag is not DateTime selectedDate)
                return;

            var keyDate = selectedDate.Date;
            currentDate = keyDate;
            scheduleMap.TryGetValue(keyDate, out var currentSchedules);

            using var dialog = new CalendarScheduleListDialog(
                keyDate,
                currentSchedules ?? Enumerable.Empty<CalendarScheduleEntry>());
            if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                return;

            var updated = dialog.Schedules;
            if (updated.Count == 0)
                scheduleMap.Remove(keyDate);
            else
                scheduleMap[keyDate] = updated;

            SaveSchedules();
            UpdateView();
            DateOrScheduleChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SaveSchedules()
        {
            try
            {
                scheduleRepository.Save(scheduleMap);
            }
            catch
            {
            }
        }

        public void LoadSchedules()
        {
            scheduleMap = scheduleRepository.Load();
        }
    }
}