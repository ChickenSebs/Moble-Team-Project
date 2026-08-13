using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using calendar4.Services;

namespace calendar4
{
    public partial class PlannerControl : UserControl
    {
        private DateTime currentDate = DateTime.Today;
        private Dictionary<string, PlannerData> plannerMap = new Dictionary<string, PlannerData>();
        private readonly int loggedInUserId;
        private readonly PlannerDbRepository plannerDbRepository = new();
        private bool isLoading = false;

        public PlannerControl(int userId)
        {
            loggedInUserId = userId;
            InitializeComponent();
        }

        private void PlannerControl_Load(object sender, EventArgs e)
        {
            // 체크리스트 파란색 선택 하이라이트 방지 설정
            if (dgvTodoList != null)
            {
                dgvTodoList.CellPainting -= DgvTodoList_CellPainting;
                dgvTodoList.CellPainting += DgvTodoList_CellPainting;

                dgvTodoList.DefaultCellStyle.SelectionBackColor =
                    dgvTodoList.DefaultCellStyle.BackColor;

                dgvTodoList.DefaultCellStyle.SelectionForeColor =
                    dgvTodoList.DefaultCellStyle.ForeColor;
            }

            InitTimeTableGrid();

            try
            {
                // 로그인한 사용자의 플래너 전체 조회
                plannerMap =
                    plannerDbRepository.Load(loggedInUserId);

                // 오늘 날짜 데이터 표시
                LoadPlannerDate(currentDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"플래너를 DB에서 불러오지 못했습니다.\n\n{ex.Message}",
                    "DB 불러오기 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                plannerMap =
                    new Dictionary<string, PlannerData>();

                ClearPlannerScreen();
            }
        }

        public void SetDate(DateTime date)
        {
            date = date.Date;
            if (currentDate == date && !isLoading) return;

            if (!isLoading) SaveCurrentPlanner();

            currentDate = date;
            LoadPlannerDate(currentDate);
        }

        // ============================================================
        // 체크리스트 셀 렌더링 (파란색 하이라이트 완전히 제거)
        // ============================================================
        private void DgvTodoList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // 선택 스타일을 제거하고 기본 배경색으로 그리기
            DataGridViewPaintParts paintParts = e.PaintParts & ~DataGridViewPaintParts.SelectionBackground;

            using (SolidBrush bgBrush = new SolidBrush(dgvTodoList.DefaultCellStyle.BackColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.CellBounds);
            }

            e.Paint(e.CellBounds, paintParts);
            e.Handled = true;
        }

        // ============================================================
        // 스터디 플래너 그리드 레이아웃 (219 x 490 맞춤 조절)
        // ============================================================
        private void InitTimeTableGrid()
        {
            if (dgvTimeTable == null) return;

            // 크기 219 x 490 맞춤 및 스크롤바 제거
            dgvTimeTable.Size = new Size(219, 490);
            dgvTimeTable.ScrollBars = ScrollBars.None;
            dgvTimeTable.BorderStyle = BorderStyle.FixedSingle;

            dgvTimeTable.Columns.Clear();
            dgvTimeTable.Rows.Clear();
            dgvTimeTable.AllowUserToAddRows = false;
            dgvTimeTable.RowHeadersVisible = false;
            dgvTimeTable.ColumnHeadersVisible = false;

            // 너비 계산
            int timeColWidth = 33;
            int minuteColWidth = (dgvTimeTable.Width - timeColWidth) / 6;

            var timeCol = new DataGridViewTextBoxColumn { Width = timeColWidth, ReadOnly = true };
            dgvTimeTable.Columns.Add(timeCol);

            for (int i = 0; i < 6; i++)
            {
                var minCol = new DataGridViewTextBoxColumn { Width = minuteColWidth, ReadOnly = true };
                dgvTimeTable.Columns.Add(minCol);
            }

            // 시간 표기 (7시 ~ 24시, 1시 ~ 2시 : 총 20개 행)
            int[] displayHours = new int[] { 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2 };
            int rowHeight = dgvTimeTable.Height / displayHours.Length;

            for (int idx = 0; idx < displayHours.Length; idx++)
            {
                int rIdx = dgvTimeTable.Rows.Add();
                dgvTimeTable.Rows[rIdx].Height = rowHeight;
                dgvTimeTable.Rows[rIdx].Cells[0].Value = displayHours[idx].ToString();

                int realHour = (idx + 7) % 24;
                dgvTimeTable.Rows[rIdx].Tag = new RowTimeInfo { RealHour = realHour, Blocks = new List<TimeBlock>() };
            }

            dgvTimeTable.CellPainting -= DgvTimeTable_CellPainting;
            dgvTimeTable.CellPainting += DgvTimeTable_CellPainting;

            // 자동 선택 하이라이트 제거
            dgvTimeTable.ClearSelection();
        }

        // ============================================================
        // CellPainting (타임테이블)
        // ============================================================
        private void DgvTimeTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // 0번 시간 열
            if (e.ColumnIndex == 0)
            {
                e.PaintBackground(e.CellBounds, true);

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Value?.ToString() ?? "",
                    dgvTimeTable.Font,
                    e.CellBounds,
                    Color.DimGray,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                using (Pen pen = new Pen(Color.LightGray))
                {
                    e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                }
                e.Handled = true;
                return;
            }

            // 1~6번 10분 단위 타임테이블 열
            if (e.ColumnIndex >= 1 && e.ColumnIndex <= 6)
            {
                using (SolidBrush bgBrush = new SolidBrush(dgvTimeTable.DefaultCellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(bgBrush, e.CellBounds);
                }

                var rowInfo = dgvTimeTable.Rows[e.RowIndex].Tag as RowTimeInfo;
                int slotIndex = e.ColumnIndex - 1;

                if (rowInfo != null && rowInfo.Blocks != null && rowInfo.Blocks.Count > 0)
                {
                    foreach (var block in rowInfo.Blocks)
                    {
                        int slotStartMin = slotIndex * 10;
                        int slotEndMin = (slotIndex + 1) * 10;

                        if (block.StartMinute < slotEndMin && block.EndMinute > slotStartMin)
                        {
                            Rectangle fillRect = new Rectangle(e.CellBounds.X, e.CellBounds.Y + 1, e.CellBounds.Width, e.CellBounds.Height - 2);

                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, block.R, block.G, block.B)))
                            {
                                e.Graphics.FillRectangle(brush, fillRect);
                            }

                            if (slotIndex == block.StartMinute / 10 && !string.IsNullOrEmpty(block.TaskName))
                            {
                                TextRenderer.DrawText(
                                    e.Graphics,
                                    block.TaskName,
                                    dgvTimeTable.Font,
                                    new Rectangle(fillRect.X + 2, fillRect.Y, 100, fillRect.Height),
                                    Color.FromArgb(50, 50, 50),
                                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                                );
                            }
                        }
                    }
                }

                // 모눈 격자선
                using (Pen gridPen = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.DrawRectangle(gridPen, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
                }

                e.Handled = true;
            }
        }

        // ============================================================
        // 총 공부시간 업데이트
        // ============================================================
        private void UpdateTotalStudyTime()
        {
            if (lblStudyTimeValue == null || dgvTimeTable == null) return;

            int totalMinutes = 0;

            for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
            {
                var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                if (rowInfo != null && rowInfo.Blocks != null)
                {
                    foreach (var block in rowInfo.Blocks)
                    {
                        int duration = block.EndMinute - block.StartMinute;
                        if (duration > 0)
                        {
                            totalMinutes += duration;
                        }
                    }
                }
            }

            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            lblStudyTimeValue.Text = $"{hours}H {minutes}M";
        }

        // ============================================================
        // 화면 완전 초기화
        // ============================================================
        public void ClearPlannerScreen()
        {
            if (dgvTodoList != null)
            {
                dgvTodoList.Rows.Clear();
                dgvTodoList.ClearSelection();
            }
            if (cbTaskList != null) cbTaskList.Items.Clear();

            if (dgvTimeTable != null)
            {
                for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
                {
                    var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                    if (rowInfo != null)
                    {
                        rowInfo.Blocks.Clear();
                    }
                }
                dgvTimeTable.ClearSelection();
                dgvTimeTable.Invalidate();
            }

            UpdateTotalStudyTime();
        }

        // ============================================================
        // [형광펜 칠하기]
        // ============================================================
        private void btnFillTime_Click(object sender, EventArgs e)
        {
            if (dtpStart == null || dtpEnd == null || dgvTimeTable == null) return;

            DateTime start = dtpStart.Value;
            DateTime end = dtpEnd.Value;

            if (start >= end)
            {
                MessageBox.Show("종료 시간이 시작 시간보다 늦어야 합니다!", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Color highlightColor = Color.FromArgb(244, 180, 190);
            string selectedText = cbColorPicker != null ? (cbColorPicker.SelectedItem?.ToString() ?? cbColorPicker.Text) : "";
            selectedText = selectedText.Replace(" ", "").Trim();

            if (selectedText.Contains("핑크") || selectedText.Contains("분홍"))
            {
                highlightColor = Color.FromArgb(244, 180, 190);
            }
            else if (selectedText.Contains("노랑") || selectedText.Contains("노란") || selectedText.Contains("옐로우"))
            {
                highlightColor = Color.FromArgb(250, 220, 150);
            }
            else if (selectedText.Contains("연두") || selectedText.Contains("초록") || selectedText.Contains("그린"))
            {
                highlightColor = Color.FromArgb(170, 215, 175);
            }
            else if (selectedText.Contains("하늘") || selectedText.Contains("파랑") || selectedText.Contains("블루"))
            {
                highlightColor = Color.FromArgb(165, 205, 235);
            }
            else if (selectedText.Contains("보라") || selectedText.Contains("퍼플"))
            {
                highlightColor = Color.FromArgb(215, 185, 225);
            }

            string selectedTask = (cbTaskList != null && cbTaskList.SelectedItem != null)
                ? cbTaskList.SelectedItem.ToString()
                : "";

            for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
            {
                var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                if (rowInfo == null) continue;

                int h = rowInfo.RealHour;

                bool isInRange = false;
                if (start.Hour <= end.Hour)
                    isInRange = (h >= start.Hour && h <= end.Hour);
                else
                    isInRange = (h >= start.Hour || h <= end.Hour);

                if (isInRange)
                {
                    int segStartMin = (h == start.Hour) ? start.Minute : 0;
                    int segEndMin = (h == end.Hour) ? end.Minute : 60;

                    if (segStartMin >= segEndMin && h == end.Hour) continue;

                    rowInfo.Blocks.RemoveAll(b => b.StartMinute < segEndMin && b.EndMinute > segStartMin);

                    rowInfo.Blocks.Add(new TimeBlock
                    {
                        StartMinute = segStartMin,
                        EndMinute = segEndMin,
                        TaskName = selectedTask,
                        R = highlightColor.R,
                        G = highlightColor.G,
                        B = highlightColor.B
                    });
                }
            }

            dgvTimeTable.ClearSelection();
            dgvTimeTable.Invalidate();
            SaveCurrentPlanner();
            UpdateTotalStudyTime();
        }

        // ============================================================
        // [선택 영역 지우기]
        // ============================================================
        private void btnClearTime_Click_Click(object sender, EventArgs e)
        {
            if (dtpStart == null || dtpEnd == null || dgvTimeTable == null) return;

            DateTime start = dtpStart.Value;
            DateTime end = dtpEnd.Value;

            for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
            {
                var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                if (rowInfo == null) continue;

                int h = rowInfo.RealHour;

                bool isInRange = false;
                if (start.Hour <= end.Hour)
                    isInRange = (h >= start.Hour && h <= end.Hour);
                else
                    isInRange = (h >= start.Hour || h <= end.Hour);

                if (isInRange)
                {
                    int segStartMin = (h == start.Hour) ? start.Minute : 0;
                    int segEndMin = (h == end.Hour) ? end.Minute : 60;

                    rowInfo.Blocks.RemoveAll(b => b.StartMinute < segEndMin && b.EndMinute > segStartMin);
                }
            }

            dgvTimeTable.ClearSelection();
            dgvTimeTable.Invalidate();
            SaveCurrentPlanner();
            UpdateTotalStudyTime();
        }

        // ============================================================
        // 할 일 추가 및 삭제
        // ============================================================
        private void btnAddTask_Click(object sender, EventArgs e)
        {
            if (txtTaskInput == null || dgvTodoList == null) return;

            string taskName = txtTaskInput.Text.Trim();
            if (!string.IsNullOrWhiteSpace(taskName))
            {
                dgvTodoList.Rows.Add(false, taskName);

                if (cbTaskList != null)
                {
                    if (!cbTaskList.Items.Contains(taskName))
                        cbTaskList.Items.Add(taskName);
                    cbTaskList.SelectedItem = taskName;
                }

                txtTaskInput.Clear();
                txtTaskInput.Focus();

                dgvTodoList.ClearSelection();

                SaveCurrentPlanner();
            }
        }

        private void btnDeleteTask_Click_Click(object sender, EventArgs e)
        {
            if (dgvTodoList == null) return;

            List<DataGridViewRow> toDelete = new List<DataGridViewRow>();
            foreach (DataGridViewCell cell in dgvTodoList.SelectedCells)
            {
                if (cell.RowIndex >= 0 && !dgvTodoList.Rows[cell.RowIndex].IsNewRow)
                {
                    var row = dgvTodoList.Rows[cell.RowIndex];
                    if (!toDelete.Contains(row)) toDelete.Add(row);
                }
            }

            foreach (var row in toDelete)
            {
                string name = row.Cells[1].Value?.ToString();
                if (!string.IsNullOrEmpty(name) && cbTaskList != null)
                {
                    cbTaskList.Items.Remove(name);
                }
                dgvTodoList.Rows.Remove(row);
            }

            dgvTodoList.ClearSelection();
            SaveCurrentPlanner();
        }

        // ============================================================
        // 저장 / 불러오기
        // ============================================================
        private void LoadPlannerDate(DateTime date)
        {
            isLoading = true;
            try
            {
                ClearPlannerScreen();
                string key = date.ToString("yyyy-MM-dd");

                if (!plannerMap.ContainsKey(key)) return;

                PlannerData data = plannerMap[key];

                if (dgvTodoList != null)
                {
                    foreach (var task in data.Tasks)
                        dgvTodoList.Rows.Add(task.Completed, task.Name);

                    dgvTodoList.ClearSelection();
                }

                if (cbTaskList != null)
                {
                    foreach (var task in data.Tasks)
                    {
                        if (!string.IsNullOrWhiteSpace(task.Name) && !cbTaskList.Items.Contains(task.Name))
                            cbTaskList.Items.Add(task.Name);
                    }
                    if (cbTaskList.Items.Count > 0) cbTaskList.SelectedIndex = 0;
                }

                if (dgvTimeTable != null)
                {
                    foreach (var slot in data.TimeSlots)
                    {
                        for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
                        {
                            var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                            if (rowInfo != null && rowInfo.RealHour == slot.Hour)
                            {
                                int start = 0;
                                int end = 0;

                                try { start = Convert.ToInt32(slot.GetType().GetProperty("StartMinute")?.GetValue(slot)); } catch { }
                                try { end = Convert.ToInt32(slot.GetType().GetProperty("EndMinute")?.GetValue(slot)); } catch { }

                                if (start == 0 && end == 0)
                                {
                                    try { start = Convert.ToInt32(slot.GetType().GetProperty("StartMin")?.GetValue(slot)); } catch { }
                                    try { end = Convert.ToInt32(slot.GetType().GetProperty("EndMin")?.GetValue(slot)); } catch { }
                                }

                                rowInfo.Blocks.Add(new TimeBlock
                                {
                                    StartMinute = start,
                                    EndMinute = end,
                                    TaskName = slot.TaskName,
                                    R = slot.R,
                                    G = slot.G,
                                    B = slot.B
                                });
                            }
                        }
                    }
                    dgvTimeTable.ClearSelection();
                    dgvTimeTable.Invalidate();
                }

                UpdateTotalStudyTime();
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SaveCurrentPlanner()
        {
            if (isLoading) return;

            string key = currentDate.ToString("yyyy-MM-dd");
            PlannerData data = new PlannerData();

            if (dgvTodoList != null)
            {
                foreach (DataGridViewRow row in dgvTodoList.Rows)
                {
                    if (row.IsNewRow) continue;
                    string name = row.Cells[1].Value?.ToString() ?? "";
                    bool completed = false;
                    if (row.Cells[0].Value != null) bool.TryParse(row.Cells[0].Value.ToString(), out completed);

                    if (!string.IsNullOrWhiteSpace(name))
                        data.Tasks.Add(new PlannerTask { Name = name, Completed = completed });
                }
            }

            if (dgvTimeTable != null)
            {
                for (int i = 0; i < dgvTimeTable.Rows.Count; i++)
                {
                    var rowInfo = dgvTimeTable.Rows[i].Tag as RowTimeInfo;
                    if (rowInfo != null)
                    {
                        foreach (var b in rowInfo.Blocks)
                        {
                            var slot = new PlannerTimeSlot
                            {
                                Hour = rowInfo.RealHour,
                                TaskName = b.TaskName,
                                R = b.R,
                                G = b.G,
                                B = b.B
                            };

                            var propStart = typeof(PlannerTimeSlot).GetProperty("StartMinute") ?? typeof(PlannerTimeSlot).GetProperty("StartMin");
                            var propEnd = typeof(PlannerTimeSlot).GetProperty("EndMinute") ?? typeof(PlannerTimeSlot).GetProperty("EndMin");

                            if (propStart != null) propStart.SetValue(slot, b.StartMinute);
                            if (propEnd != null) propEnd.SetValue(slot, b.EndMinute);

                            data.TimeSlots.Add(slot);
                        }
                    }
                }
            }

            if (data.Tasks.Count > 0 || data.TimeSlots.Count > 0)
            {
                plannerMap[key] = data;
            }
            else
            {
                plannerMap.Remove(key);
            }

            try
            {
                plannerDbRepository.Save(
                    loggedInUserId,
                    currentDate,
                    data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"플래너를 DB에 저장하지 못했습니다.\n\n{ex.Message}",
                    "DB 저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            SaveCurrentPlanner();
            base.OnHandleDestroyed(e);
        }
    }

    public class RowTimeInfo
    {
        public int RealHour { get; set; }
        public List<TimeBlock> Blocks { get; set; } = new List<TimeBlock>();
    }

    public class TimeBlock
    {
        public int StartMinute { get; set; }
        public int EndMinute { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}