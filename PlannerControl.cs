using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace calendar4
{
    public partial class PlannerControl : UserControl
    {
        // ============================================================
        // 기본 변수
        // ============================================================

        private DateTime currentDate = DateTime.Today;

        private Dictionary<string, PlannerData> plannerMap =
            new Dictionary<string, PlannerData>();

        // ============================================================
        // [수정] 프로그램이 종료되어도 유지되는 저장 파일
        // ============================================================

        private readonly string plannerSaveFilePath =
            Path.Combine(Application.StartupPath, "saved_planners.json");

        private bool isLoading = false;

        // ============================================================
        // 생성자
        // ============================================================

        public PlannerControl()
        {
            InitializeComponent();
        }

        // ============================================================
        // Load
        // ============================================================

        private void PlannerControl_Load(object sender, EventArgs e)
        {
            // ========================================================
            // [중요]
            // 기존 코드의 File.Delete()를 완전히 제거함.
            //
            // 프로그램을 다시 열어도 기존 형광펜 데이터를 유지하기 위해
            // 저장된 JSON을 먼저 불러온다.
            // ========================================================

            LoadPlanners();

            // ========================================================
            // 체크리스트 파란색 선택 하이라이트 방지
            // ========================================================

            if (dgvTodoList != null)
            {
                dgvTodoList.CellPainting -= DgvTodoList_CellPainting;
                dgvTodoList.CellPainting += DgvTodoList_CellPainting;

                dgvTodoList.DefaultCellStyle.SelectionBackColor =
                    dgvTodoList.DefaultCellStyle.BackColor;

                dgvTodoList.DefaultCellStyle.SelectionForeColor =
                    dgvTodoList.DefaultCellStyle.ForeColor;

                // 체크 상태 변경 시 자동 저장
                dgvTodoList.CellValueChanged -= DgvTodoList_CellValueChanged;
                dgvTodoList.CellValueChanged += DgvTodoList_CellValueChanged;

                dgvTodoList.CurrentCellDirtyStateChanged -=
                    DgvTodoList_CurrentCellDirtyStateChanged;

                dgvTodoList.CurrentCellDirtyStateChanged +=
                    DgvTodoList_CurrentCellDirtyStateChanged;
            }

            // ========================================================
            // 타임테이블 생성
            // ========================================================

            InitTimeTableGrid();

            // ========================================================
            // 현재 날짜의 플래너 불러오기
            // ========================================================

            LoadPlannerDate(currentDate);
        }

        // ============================================================
        // [추가]
        // 체크박스 바로 저장
        // ============================================================

        private void DgvTodoList_CurrentCellDirtyStateChanged(
            object sender,
            EventArgs e)
        {
            if (dgvTodoList == null)
                return;

            if (dgvTodoList.IsCurrentCellDirty)
            {
                dgvTodoList.CommitEdit(
                    DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvTodoList_CellValueChanged(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (isLoading)
                return;

            if (e.RowIndex >= 0)
            {
                SaveCurrentPlanner();
            }
        }

        // ============================================================
        // 날짜 변경
        // ============================================================

        public void SetDate(DateTime date)
        {
            date = date.Date;

            if (currentDate == date && !isLoading)
            {
                // 같은 날짜라도 화면이 비어 있다면 다시 불러오기
                if (dgvTimeTable != null)
                {
                    LoadPlannerDate(currentDate);
                }

                return;
            }

            // ========================================================
            // [중요]
            // 기존 날짜 데이터를 먼저 저장한다.
            // ========================================================

            if (!isLoading)
            {
                SaveCurrentPlanner();
            }

            currentDate = date;

            // ========================================================
            // 새 날짜 데이터를 불러온다.
            // ========================================================

            LoadPlannerDate(currentDate);
        }

        // ============================================================
        // 현재 플래너 날짜 가져오기
        // ============================================================

        public DateTime GetDate()
        {
            return currentDate;
        }

        // ============================================================
        // 체크리스트 셀 렌더링
        // ============================================================

        private void DgvTodoList_CellPainting(
            object sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewPaintParts paintParts =
                e.PaintParts &
                ~DataGridViewPaintParts.SelectionBackground;

            using (SolidBrush bgBrush =
                   new SolidBrush(dgvTodoList.DefaultCellStyle.BackColor))
            {
                e.Graphics.FillRectangle(
                    bgBrush,
                    e.CellBounds);
            }

            e.Paint(e.CellBounds, paintParts);

            e.Handled = true;
        }

        // ============================================================
        // 타임테이블 초기화
        // ============================================================

        private void InitTimeTableGrid()
        {
            if (dgvTimeTable == null)
                return;

            dgvTimeTable.Size = new Size(219, 490);

            dgvTimeTable.ScrollBars = ScrollBars.None;

            dgvTimeTable.BorderStyle =
                BorderStyle.FixedSingle;

            dgvTimeTable.Columns.Clear();
            dgvTimeTable.Rows.Clear();

            dgvTimeTable.AllowUserToAddRows = false;
            dgvTimeTable.RowHeadersVisible = false;
            dgvTimeTable.ColumnHeadersVisible = false;

            // ========================================================
            // 시간 열
            // ========================================================

            int timeColWidth = 33;

            int minuteColWidth =
                (dgvTimeTable.Width - timeColWidth) / 6;

            var timeCol =
                new DataGridViewTextBoxColumn
                {
                    Width = timeColWidth,
                    ReadOnly = true
                };

            dgvTimeTable.Columns.Add(timeCol);

            // ========================================================
            // 10분 단위 6칸
            // ========================================================

            for (int i = 0; i < 6; i++)
            {
                var minCol =
                    new DataGridViewTextBoxColumn
                    {
                        Width = minuteColWidth,
                        ReadOnly = true
                    };

                dgvTimeTable.Columns.Add(minCol);
            }

            // ========================================================
            // 7시 ~ 다음날 2시
            //
            // 실제 시간:
            //
            // 7
            // 8
            // ...
            // 23
            // 0
            // 1
            // 2
            //
            // 총 20개
            // ========================================================

            int[] displayHours =
            {
                7, 8, 9, 10, 11, 12,
                1, 2, 3, 4, 5, 6,
                7, 8, 9, 10, 11, 12,
                1, 2
            };

            int rowHeight =
                dgvTimeTable.Height /
                displayHours.Length;

            for (int idx = 0;
                 idx < displayHours.Length;
                 idx++)
            {
                int rIdx =
                    dgvTimeTable.Rows.Add();

                dgvTimeTable.Rows[rIdx].Height =
                    rowHeight;

                dgvTimeTable.Rows[rIdx]
                    .Cells[0]
                    .Value =
                    displayHours[idx].ToString();

                // ====================================================
                // 실제 24시간 기준
                // 7,8,9...23,0,1,2
                // ====================================================

                int realHour =
                    (idx + 7) % 24;

                dgvTimeTable.Rows[rIdx].Tag =
                    new RowTimeInfo
                    {
                        RealHour = realHour,
                        Blocks = new List<TimeBlock>()
                    };
            }

            dgvTimeTable.CellPainting -=
                DgvTimeTable_CellPainting;

            dgvTimeTable.CellPainting +=
                DgvTimeTable_CellPainting;

            dgvTimeTable.ClearSelection();
        }

        // ============================================================
        // 타임테이블 그리기
        // ============================================================

        private void DgvTimeTable_CellPainting(
            object sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // ========================================================
            // 시간 표시 열
            // ========================================================

            if (e.ColumnIndex == 0)
            {
                e.PaintBackground(
                    e.CellBounds,
                    true);

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Value?.ToString() ?? "",
                    dgvTimeTable.Font,
                    e.CellBounds,
                    Color.DimGray,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter
                );

                using (Pen pen =
                       new Pen(Color.LightGray))
                {
                    e.Graphics.DrawLine(
                        pen,
                        e.CellBounds.Right - 1,
                        e.CellBounds.Top,
                        e.CellBounds.Right - 1,
                        e.CellBounds.Bottom);

                    e.Graphics.DrawLine(
                        pen,
                        e.CellBounds.Left,
                        e.CellBounds.Bottom - 1,
                        e.CellBounds.Right,
                        e.CellBounds.Bottom - 1);
                }

                e.Handled = true;

                return;
            }

            // ========================================================
            // 10분 단위 열
            // ========================================================

            if (e.ColumnIndex >= 1 &&
                e.ColumnIndex <= 6)
            {
                using (SolidBrush bgBrush =
                       new SolidBrush(
                           dgvTimeTable.DefaultCellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(
                        bgBrush,
                        e.CellBounds);
                }

                var rowInfo =
                    dgvTimeTable.Rows[e.RowIndex].Tag
                    as RowTimeInfo;

                int slotIndex =
                    e.ColumnIndex - 1;

                if (rowInfo != null &&
                    rowInfo.Blocks != null &&
                    rowInfo.Blocks.Count > 0)
                {
                    foreach (var block in rowInfo.Blocks)
                    {
                        int slotStartMin =
                            slotIndex * 10;

                        int slotEndMin =
                            (slotIndex + 1) * 10;

                        if (block.StartMinute < slotEndMin &&
                            block.EndMinute > slotStartMin)
                        {
                            Rectangle fillRect =
                                new Rectangle(
                                    e.CellBounds.X,
                                    e.CellBounds.Y + 1,
                                    e.CellBounds.Width,
                                    e.CellBounds.Height - 2);

                            using (SolidBrush brush =
                                   new SolidBrush(
                                       Color.FromArgb(
                                           200,
                                           block.R,
                                           block.G,
                                           block.B)))
                            {
                                e.Graphics.FillRectangle(
                                    brush,
                                    fillRect);
                            }

                            // =================================================
                            // 시작 칸에만 할 일 이름 표시
                            // =================================================

                            if (slotIndex ==
                                block.StartMinute / 10)
                            {
                                if (!string.IsNullOrEmpty(
                                    block.TaskName))
                                {
                                    TextRenderer.DrawText(
                                        e.Graphics,
                                        block.TaskName,
                                        dgvTimeTable.Font,
                                        new Rectangle(
                                            fillRect.X + 2,
                                            fillRect.Y,
                                            100,
                                            fillRect.Height),
                                        Color.FromArgb(
                                            50,
                                            50,
                                            50),
                                        TextFormatFlags.VerticalCenter |
                                        TextFormatFlags.Left
                                    );
                                }
                            }
                        }
                    }
                }

                // ====================================================
                // 격자
                // ====================================================

                using (Pen gridPen =
                       new Pen(
                           Color.FromArgb(
                               230,
                               230,
                               230)))
                {
                    e.Graphics.DrawRectangle(
                        gridPen,
                        e.CellBounds.X,
                        e.CellBounds.Y,
                        e.CellBounds.Width - 1,
                        e.CellBounds.Height - 1);
                }

                e.Handled = true;
            }
        }

        // ============================================================
        // 총 공부시간
        // ============================================================

        private void UpdateTotalStudyTime()
        {
            if (lblStudyTimeValue == null ||
                dgvTimeTable == null)
                return;

            int totalMinutes = 0;

            for (int i = 0;
                 i < dgvTimeTable.Rows.Count;
                 i++)
            {
                var rowInfo =
                    dgvTimeTable.Rows[i].Tag
                    as RowTimeInfo;

                if (rowInfo == null ||
                    rowInfo.Blocks == null)
                    continue;

                foreach (var block in rowInfo.Blocks)
                {
                    int duration =
                        block.EndMinute -
                        block.StartMinute;

                    if (duration > 0)
                    {
                        totalMinutes += duration;
                    }
                }
            }

            int hours =
                totalMinutes / 60;

            int minutes =
                totalMinutes % 60;

            lblStudyTimeValue.Text =
                $"{hours}H {minutes}M";
        }

        // ============================================================
        // 화면 초기화
        // ============================================================

        public void ClearPlannerScreen()
        {
            if (dgvTodoList != null)
            {
                dgvTodoList.Rows.Clear();
                dgvTodoList.ClearSelection();
            }

            if (cbTaskList != null)
            {
                cbTaskList.Items.Clear();
            }

            if (dgvTimeTable != null)
            {
                for (int i = 0;
                     i < dgvTimeTable.Rows.Count;
                     i++)
                {
                    var rowInfo =
                        dgvTimeTable.Rows[i].Tag
                        as RowTimeInfo;

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
        // 형광펜 칠하기
        // ============================================================

        private void btnFillTime_Click(
            object sender,
            EventArgs e)
        {
            if (dtpStart == null ||
                dtpEnd == null ||
                dgvTimeTable == null)
                return;

            DateTime start =
                dtpStart.Value;

            DateTime end =
                dtpEnd.Value;

            if (start >= end)
            {
                MessageBox.Show(
                    "종료 시간이 시작 시간보다 늦어야 합니다!",
                    "안내",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // ========================================================
            // 기본 핑크
            // ========================================================

            Color highlightColor =
                Color.FromArgb(
                    244,
                    180,
                    190);

            string selectedText =
                cbColorPicker != null
                    ? (
                        cbColorPicker.SelectedItem?.ToString()
                        ??
                        cbColorPicker.Text
                      )
                    : "";

            selectedText =
                selectedText
                    .Replace(" ", "")
                    .Trim();

            if (selectedText.Contains("핑크") ||
                selectedText.Contains("분홍"))
            {
                highlightColor =
                    Color.FromArgb(
                        244,
                        180,
                        190);
            }
            else if (
                selectedText.Contains("노랑") ||
                selectedText.Contains("노란") ||
                selectedText.Contains("옐로우"))
            {
                highlightColor =
                    Color.FromArgb(
                        250,
                        220,
                        150);
            }
            else if (
                selectedText.Contains("연두") ||
                selectedText.Contains("초록") ||
                selectedText.Contains("그린"))
            {
                highlightColor =
                    Color.FromArgb(
                        170,
                        215,
                        175);
            }
            else if (
                selectedText.Contains("하늘") ||
                selectedText.Contains("파랑") ||
                selectedText.Contains("블루"))
            {
                highlightColor =
                    Color.FromArgb(
                        165,
                        205,
                        235);
            }
            else if (
                selectedText.Contains("보라") ||
                selectedText.Contains("퍼플"))
            {
                highlightColor =
                    Color.FromArgb(
                        215,
                        185,
                        225);
            }

            // ========================================================
            // 선택된 할 일
            // ========================================================

            string selectedTask =
                (
                    cbTaskList != null &&
                    cbTaskList.SelectedItem != null
                )
                ?
                cbTaskList.SelectedItem.ToString()
                :
                "";

            // ========================================================
            // 타임테이블에 형광펜 추가
            // ========================================================

            for (int i = 0;
                 i < dgvTimeTable.Rows.Count;
                 i++)
            {
                var rowInfo =
                    dgvTimeTable.Rows[i].Tag
                    as RowTimeInfo;

                if (rowInfo == null)
                    continue;

                int h =
                    rowInfo.RealHour;

                bool isInRange;

                // ====================================================
                // 일반적인 시간 범위
                // ====================================================

                if (start.Hour <= end.Hour)
                {
                    isInRange =
                        h >= start.Hour &&
                        h <= end.Hour;
                }
                else
                {
                    // =================================================
                    // 자정을 넘어가는 경우
                    // 예: 22:00 ~ 02:00
                    // =================================================

                    isInRange =
                        h >= start.Hour ||
                        h <= end.Hour;
                }

                if (!isInRange)
                    continue;

                int segStartMin =
                    h == start.Hour
                        ? start.Minute
                        : 0;

                int segEndMin =
                    h == end.Hour
                        ? end.Minute
                        : 60;

                if (segEndMin <= segStartMin)
                    continue;

                // ====================================================
                // 해당 영역의 기존 형광펜 제거
                // ====================================================

                rowInfo.Blocks.RemoveAll(
                    b =>
                        b.StartMinute < segEndMin &&
                        b.EndMinute > segStartMin);

                // ====================================================
                // 새 형광펜 추가
                // ====================================================

                rowInfo.Blocks.Add(
                    new TimeBlock
                    {
                        StartMinute = segStartMin,
                        EndMinute = segEndMin,
                        TaskName = selectedTask,

                        R = highlightColor.R,
                        G = highlightColor.G,
                        B = highlightColor.B
                    });
            }

            dgvTimeTable.ClearSelection();

            dgvTimeTable.Invalidate();

            // ========================================================
            // [중요] 형광펜 즉시 저장
            // ========================================================

            SaveCurrentPlanner();

            UpdateTotalStudyTime();
        }

        // ============================================================
        // 선택 영역 지우기
        // ============================================================

        private void btnClearTime_Click_Click(
            object sender,
            EventArgs e)
        {
            if (dtpStart == null ||
                dtpEnd == null ||
                dgvTimeTable == null)
                return;

            DateTime start =
                dtpStart.Value;

            DateTime end =
                dtpEnd.Value;

            if (start >= end)
            {
                MessageBox.Show(
                    "종료 시간이 시작 시간보다 늦어야 합니다!",
                    "안내",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            for (int i = 0;
                 i < dgvTimeTable.Rows.Count;
                 i++)
            {
                var rowInfo =
                    dgvTimeTable.Rows[i].Tag
                    as RowTimeInfo;

                if (rowInfo == null)
                    continue;

                int h =
                    rowInfo.RealHour;

                bool isInRange;

                if (start.Hour <= end.Hour)
                {
                    isInRange =
                        h >= start.Hour &&
                        h <= end.Hour;
                }
                else
                {
                    isInRange =
                        h >= start.Hour ||
                        h <= end.Hour;
                }

                if (!isInRange)
                    continue;

                int segStartMin =
                    h == start.Hour
                        ? start.Minute
                        : 0;

                int segEndMin =
                    h == end.Hour
                        ? end.Minute
                        : 60;

                rowInfo.Blocks.RemoveAll(
                    b =>
                        b.StartMinute < segEndMin &&
                        b.EndMinute > segStartMin);
            }

            dgvTimeTable.ClearSelection();

            dgvTimeTable.Invalidate();

            SaveCurrentPlanner();

            UpdateTotalStudyTime();
        }

        // ============================================================
        // 할 일 추가
        // ============================================================

        private void btnAddTask_Click(
            object sender,
            EventArgs e)
        {
            if (txtTaskInput == null ||
                dgvTodoList == null)
                return;

            string taskName =
                txtTaskInput.Text.Trim();

            if (!string.IsNullOrWhiteSpace(taskName))
            {
                dgvTodoList.Rows.Add(
                    false,
                    taskName);

                if (cbTaskList != null)
                {
                    if (!cbTaskList.Items.Contains(taskName))
                    {
                        cbTaskList.Items.Add(taskName);
                    }

                    cbTaskList.SelectedItem =
                        taskName;
                }

                txtTaskInput.Clear();

                txtTaskInput.Focus();

                dgvTodoList.ClearSelection();

                SaveCurrentPlanner();
            }
        }

        // ============================================================
        // 할 일 삭제
        // ============================================================

        private void btnDeleteTask_Click_Click(
            object sender,
            EventArgs e)
        {
            if (dgvTodoList == null)
                return;

            List<DataGridViewRow> toDelete =
                new List<DataGridViewRow>();

            foreach (
                DataGridViewCell cell
                in dgvTodoList.SelectedCells)
            {
                if (cell.RowIndex >= 0 &&
                    !dgvTodoList.Rows[cell.RowIndex].IsNewRow)
                {
                    var row =
                        dgvTodoList.Rows[cell.RowIndex];

                    if (!toDelete.Contains(row))
                    {
                        toDelete.Add(row);
                    }
                }
            }

            foreach (var row in toDelete)
            {
                string name =
                    row.Cells[1]
                        .Value?
                        .ToString();

                if (!string.IsNullOrEmpty(name) &&
                    cbTaskList != null)
                {
                    cbTaskList.Items.Remove(name);
                }

                dgvTodoList.Rows.Remove(row);
            }

            dgvTodoList.ClearSelection();

            SaveCurrentPlanner();
        }

        // ============================================================
        // [중요]
        // 저장된 전체 플래너 데이터를 JSON에서 불러온다.
        // ============================================================

        private void LoadPlanners()
        {
            try
            {
                if (!File.Exists(plannerSaveFilePath))
                    return;

                string json =
                    File.ReadAllText(
                        plannerSaveFilePath);

                if (string.IsNullOrWhiteSpace(json))
                    return;

                var loadedData =
                    JsonSerializer.Deserialize<
                        Dictionary<string, PlannerData>
                    >(json);

                if (loadedData != null)
                {
                    plannerMap =
                        loadedData;
                }
            }
            catch
            {
                plannerMap =
                    new Dictionary<string, PlannerData>();
            }
        }

        // ============================================================
        // 특정 날짜 플래너 불러오기
        // ============================================================

        private void LoadPlannerDate(
            DateTime date)
        {
            isLoading = true;

            try
            {
                ClearPlannerScreen();

                string key =
                    date.ToString(
                        "yyyy-MM-dd");

                // ====================================================
                // 해당 날짜에 저장된 데이터가 없으면 빈 화면
                // ====================================================

                if (!plannerMap.ContainsKey(key))
                    return;

                PlannerData data =
                    plannerMap[key];

                // ====================================================
                // 할 일 불러오기
                // ====================================================

                if (dgvTodoList != null)
                {
                    foreach (
                        var task
                        in data.Tasks)
                    {
                        dgvTodoList.Rows.Add(
                            task.Completed,
                            task.Name);
                    }

                    dgvTodoList.ClearSelection();
                }

                // ====================================================
                // 콤보박스 할 일 불러오기
                // ====================================================

                if (cbTaskList != null)
                {
                    foreach (
                        var task
                        in data.Tasks)
                    {
                        if (
                            !string.IsNullOrWhiteSpace(
                                task.Name) &&
                            !cbTaskList.Items.Contains(
                                task.Name))
                        {
                            cbTaskList.Items.Add(
                                task.Name);
                        }
                    }

                    if (cbTaskList.Items.Count > 0)
                    {
                        cbTaskList.SelectedIndex = 0;
                    }
                }

                // ====================================================
                // 형광펜 데이터 불러오기
                // ====================================================

                if (dgvTimeTable != null)
                {
                    foreach (
                        var slot
                        in data.TimeSlots)
                    {
                        for (
                            int i = 0;
                            i < dgvTimeTable.Rows.Count;
                            i++)
                        {
                            var rowInfo =
                                dgvTimeTable.Rows[i].Tag
                                as RowTimeInfo;

                            if (rowInfo == null)
                                continue;

                            if (
                                rowInfo.RealHour !=
                                slot.Hour)
                                continue;

                            rowInfo.Blocks.Add(
                                new TimeBlock
                                {
                                    StartMinute =
                                        slot.StartMinute,

                                    EndMinute =
                                        slot.EndMinute,

                                    TaskName =
                                        slot.TaskName,

                                    R = slot.R,
                                    G = slot.G,
                                    B = slot.B
                                });
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

        // ============================================================
        // 현재 날짜 플래너 저장
        // ============================================================

        private void SaveCurrentPlanner()
        {
            if (isLoading)
                return;

            string key =
                currentDate.ToString(
                    "yyyy-MM-dd");

            PlannerData data =
                new PlannerData();

            // ========================================================
            // 할 일 저장
            // ========================================================

            if (dgvTodoList != null)
            {
                foreach (
                    DataGridViewRow row
                    in dgvTodoList.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string name =
                        row.Cells[1]
                            .Value?
                            .ToString()
                        ?? "";

                    bool completed = false;

                    if (row.Cells[0].Value != null)
                    {
                        bool.TryParse(
                            row.Cells[0]
                                .Value
                                .ToString(),
                            out completed);
                    }

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        data.Tasks.Add(
                            new PlannerTask
                            {
                                Name = name,
                                Completed = completed
                            });
                    }
                }
            }

            // ========================================================
            // 형광펜 저장
            // ========================================================

            if (dgvTimeTable != null)
            {
                for (
                    int i = 0;
                    i < dgvTimeTable.Rows.Count;
                    i++)
                {
                    var rowInfo =
                        dgvTimeTable.Rows[i].Tag
                        as RowTimeInfo;

                    if (rowInfo == null)
                        continue;

                    foreach (
                        var block
                        in rowInfo.Blocks)
                    {
                        data.TimeSlots.Add(
                            new PlannerTimeSlot
                            {
                                Hour =
                                    rowInfo.RealHour,

                                StartMinute =
                                    block.StartMinute,

                                EndMinute =
                                    block.EndMinute,

                                TaskName =
                                    block.TaskName,

                                R = block.R,
                                G = block.G,
                                B = block.B
                            });
                    }
                }
            }

            // ========================================================
            // 데이터가 있으면 날짜별로 저장
            // ========================================================

            if (
                data.Tasks.Count > 0 ||
                data.TimeSlots.Count > 0)
            {
                plannerMap[key] = data;
            }
            else
            {
                // 아무것도 없으면 해당 날짜만 삭제
                plannerMap.Remove(key);
            }

            // ========================================================
            // JSON 파일에 저장
            // ========================================================

            SavePlanners();
        }

        // ============================================================
        // JSON 저장
        // ============================================================

        private void SavePlanners()
        {
            try
            {
                string json =
                    JsonSerializer.Serialize(
                        plannerMap,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });

                File.WriteAllText(
                    plannerSaveFilePath,
                    json);
            }
            catch
            {
                // 저장 오류가 발생해도 프로그램이 종료되지 않도록 함
            }
        }

        // ============================================================
        // 컨트롤 종료
        // ============================================================

        protected override void OnHandleDestroyed(
            EventArgs e)
        {
            // ========================================================
            // [중요]
            // 컨트롤이 사라지기 전에 현재 날짜 데이터 저장
            // ========================================================

            SaveCurrentPlanner();

            base.OnHandleDestroyed(e);
        }
    }

    // ================================================================
    // 타임테이블 한 줄 정보
    // ================================================================

    public class RowTimeInfo
    {
        public int RealHour { get; set; }

        public List<TimeBlock> Blocks { get; set; } =
            new List<TimeBlock>();
    }

    // ================================================================
    // 화면에서 사용하는 형광펜 데이터
    // ================================================================

    public class TimeBlock
    {
        public int StartMinute { get; set; }

        public int EndMinute { get; set; }

        public string TaskName { get; set; } =
            string.Empty;

        public int R { get; set; }

        public int G { get; set; }

        public int B { get; set; }
    }
}