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
        // 현재 플래너가 표시하고 있는 날짜
        private DateTime currentDate = DateTime.Today;

        // 날짜별 플래너 데이터
        // key : yyyy-MM-dd
        private Dictionary<string, PlannerData> plannerMap =
            new Dictionary<string, PlannerData>();

        private readonly PlannerRepository plannerRepository = new();

        private bool isLoading = false;

        public PlannerControl()
        {
            InitializeComponent();
        }

        // ============================================================
        // 로드
        // ============================================================
        private void PlannerControl_Load(object sender, EventArgs e)
        {
            LoadPlanners();

            InitTimeTableGrid();
            InitDefaults();

            // 처음에는 오늘 날짜
            LoadPlannerDate(currentDate);
        }

        // ============================================================
        // mainForm의 monthCalendar1에서 날짜를 전달받는 메서드
        // ============================================================
        public void SetDate(DateTime date)
        {
            date = date.Date;

            // 같은 날짜면 다시 불러올 필요 없음
            if (currentDate == date && !isLoading)
            {
                return;
            }

            // 이전 날짜 데이터 저장
            if (!isLoading)
            {
                SaveCurrentPlanner();
            }

            // 날짜 변경
            currentDate = date;

            // 새 날짜 데이터 불러오기
            LoadPlannerDate(currentDate);
        }

        // ============================================================
        // 기본 설정
        // ============================================================
        private void InitDefaults()
        {
            if (cbTaskList != null)
            {
                if (cbTaskList.Items.Count == 0)
                {
                    cbTaskList.Items.Add("자율 학습");
                }

                if (cbTaskList.Items.Count > 0)
                {
                    cbTaskList.SelectedIndex = 0;
                }
            }

            if (cbColorPicker != null &&
                cbColorPicker.Items.Count > 0)
            {
                cbColorPicker.SelectedIndex = 0;
            }
        }

        // ============================================================
        // 24시간 시간표 생성
        // ============================================================
        private void InitTimeTableGrid()
        {
            if (dgvTimeTable == null)
                return;

            dgvTimeTable.Rows.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                dgvTimeTable.Rows.Add(
                    $"{hour:D2}:00 ~ {hour + 1:D2}:00",
                    ""
                );
            }
        }

        // ============================================================
        // 현재 플래너 화면 전체 초기화
        // ============================================================
        private void ClearPlannerScreen()
        {
            // 할 일 목록 삭제
            if (dgvTodoList != null)
            {
                dgvTodoList.Rows.Clear();
            }

            // 할 일 선택 ComboBox 초기화
            if (cbTaskList != null)
            {
                cbTaskList.Items.Clear();

                cbTaskList.Items.Add("자율 학습");

                if (cbTaskList.Items.Count > 0)
                {
                    cbTaskList.SelectedIndex = 0;
                }
            }

            // 시간표 초기화
            if (dgvTimeTable != null)
            {
                for (int i = 0;
                     i < dgvTimeTable.Rows.Count;
                     i++)
                {
                    dgvTimeTable.Rows[i]
                        .Cells[1]
                        .Value = "";

                    dgvTimeTable.Rows[i]
                        .Cells[1]
                        .Style
                        .BackColor = Color.White;

                    dgvTimeTable.Rows[i]
                        .Cells[1]
                        .Style
                        .SelectionBackColor =
                        SystemColors.Highlight;
                }
            }
        }

        // ============================================================
        // 선택된 날짜의 플래너 불러오기
        // ============================================================
        private void LoadPlannerDate(DateTime date)
        {
            isLoading = true;

            try
            {
                // 먼저 현재 화면을 깨끗하게 비움
                ClearPlannerScreen();

                string key =
                    date.ToString("yyyy-MM-dd");

                // 해당 날짜에 저장된 데이터가 없으면
                // 빈 플래너 상태 그대로 종료
                if (!plannerMap.ContainsKey(key))
                {
                    return;
                }

                PlannerData data =
                    plannerMap[key];

                // ----------------------------------------------------
                // 할 일 목록 복구
                // ----------------------------------------------------
                if (dgvTodoList != null)
                {
                    foreach (PlannerTask task
                             in data.Tasks)
                    {
                        dgvTodoList.Rows.Add(
                            task.Completed,
                            task.Name
                        );
                    }
                }

                // ----------------------------------------------------
                // 할 일 ComboBox 복구
                // ----------------------------------------------------
                if (cbTaskList != null)
                {
                    foreach (PlannerTask task
                             in data.Tasks)
                    {
                        if (!string.IsNullOrWhiteSpace(task.Name) &&
                            !cbTaskList.Items.Contains(task.Name))
                        {
                            cbTaskList.Items.Add(
                                task.Name
                            );
                        }
                    }

                    if (cbTaskList.Items.Count > 0)
                    {
                        cbTaskList.SelectedIndex = 0;
                    }
                }

                // ----------------------------------------------------
                // 시간표 복구
                // ----------------------------------------------------
                if (dgvTimeTable != null)
                {
                    foreach (PlannerTimeSlot slot
                             in data.TimeSlots)
                    {
                        if (slot.Hour >= 0 &&
                            slot.Hour < dgvTimeTable.Rows.Count)
                        {
                            DataGridViewCell cell =
                                dgvTimeTable.Rows[
                                    slot.Hour
                                ].Cells[1];

                            cell.Value =
                                slot.TaskName;

                            cell.Style.BackColor =
                                Color.FromArgb(
                                    slot.R,
                                    slot.G,
                                    slot.B
                                );
                        }
                    }
                }
            }
            finally
            {
                isLoading = false;
            }
        }

        // ============================================================
        // 현재 화면의 플래너 저장
        // ============================================================
        private void SaveCurrentPlanner()
        {
            if (isLoading)
                return;

            string key =
                currentDate.ToString("yyyy-MM-dd");

            PlannerData data =
                new PlannerData();

            // ----------------------------------------------------
            // 할 일 목록 저장
            // ----------------------------------------------------
            if (dgvTodoList != null)
            {
                foreach (DataGridViewRow row
                         in dgvTodoList.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string taskName =
                        row.Cells[1]
                           .Value?
                           .ToString() ?? "";

                    bool completed = false;

                    if (row.Cells[0].Value != null)
                    {
                        bool.TryParse(
                            row.Cells[0]
                                .Value
                                .ToString(),
                            out completed
                        );
                    }

                    if (!string.IsNullOrWhiteSpace(taskName))
                    {
                        data.Tasks.Add(
                            new PlannerTask
                            {
                                Name = taskName,
                                Completed = completed
                            }
                        );
                    }
                }
            }

            // ----------------------------------------------------
            // 시간표 저장
            // ----------------------------------------------------
            if (dgvTimeTable != null)
            {
                for (int hour = 0;
                     hour < dgvTimeTable.Rows.Count &&
                     hour < 24;
                     hour++)
                {
                    string taskName =
                        dgvTimeTable.Rows[hour]
                            .Cells[1]
                            .Value?
                            .ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(taskName))
                        continue;

                    Color color =
                        dgvTimeTable.Rows[hour]
                            .Cells[1]
                            .Style
                            .BackColor;

                    data.TimeSlots.Add(
                        new PlannerTimeSlot
                        {
                            Hour = hour,
                            TaskName = taskName,
                            R = color.R,
                            G = color.G,
                            B = color.B
                        }
                    );
                }
            }

            // ----------------------------------------------------
            // 저장할 데이터가 있는 경우
            // ----------------------------------------------------
            if (data.Tasks.Count > 0 ||
                data.TimeSlots.Count > 0)
            {
                plannerMap[key] = data;
            }
            else
            {
                // 아무것도 없으면 해당 날짜 데이터 삭제
                plannerMap.Remove(key);
            }

            SavePlanners();
        }

        // ============================================================
        // JSON 저장
        // ============================================================
        private void SavePlanners()
        {
            try
            {
                plannerRepository.Save(plannerMap);
            }
            catch
            {
                // 저장 오류 무시
            }
        }

        // ============================================================
        // JSON 불러오기
        // ============================================================
        private void LoadPlanners()
        {
            plannerMap = plannerRepository.Load();
        }

        // ============================================================
        // [할 일 추가]
        // ============================================================
        private void btnAddTask_Click(
            object sender,
            EventArgs e)
        {
            if (txtTaskInput == null ||
                dgvTodoList == null)
            {
                return;
            }

            string taskName =
                txtTaskInput.Text.Trim();

            if (!string.IsNullOrWhiteSpace(taskName))
            {
                // 할 일 목록에 추가
                dgvTodoList.Rows.Add(
                    false,
                    taskName
                );

                // ComboBox에 추가
                if (cbTaskList != null)
                {
                    if (!cbTaskList.Items.Contains(
                        taskName))
                    {
                        cbTaskList.Items.Add(
                            taskName
                        );
                    }

                    cbTaskList.SelectedItem =
                        taskName;
                }

                txtTaskInput.Clear();
                txtTaskInput.Focus();

                SaveCurrentPlanner();
            }
        }

        // ============================================================
        // [시간표 칠하기]
        // ============================================================
        private void btnFillTime_Click(
            object sender,
            EventArgs e)
        {
            if (dtpStart == null ||
                dtpEnd == null ||
                dgvTimeTable == null)
            {
                return;
            }

            int startHour =
                dtpStart.Value.Hour;

            int endHour =
                dtpEnd.Value.Hour;

            if (startHour > endHour ||
                (startHour == endHour &&
                 dtpEnd.Value.Minute == 0))
            {
                MessageBox.Show(
                    "종료 시간이 시작 시간보다 늦어야 합니다!",
                    "시간 설정 안내",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // ----------------------------------------------------
            // 색상
            // ----------------------------------------------------
            Color highlightColor =
                Color.Yellow;

            int colorIndex =
                cbColorPicker != null
                    ? cbColorPicker.SelectedIndex
                    : 0;

            switch (colorIndex)
            {
                case 0:
                    highlightColor =
                        Color.FromArgb(
                            255,
                            255,
                            170
                        );
                    break;

                case 1:
                    highlightColor =
                        Color.FromArgb(
                            190,
                            255,
                            190
                        );
                    break;

                case 2:
                    highlightColor =
                        Color.FromArgb(
                            190,
                            230,
                            255
                        );
                    break;

                case 3:
                    highlightColor =
                        Color.FromArgb(
                            230,
                            200,
                            255
                        );
                    break;

                case 4:
                    highlightColor =
                        Color.FromArgb(
                            255,
                            200,
                            220
                        );
                    break;
            }

            // ----------------------------------------------------
            // 선택된 할 일
            // ----------------------------------------------------
            string selectedTask =
                (
                    cbTaskList != null &&
                    cbTaskList.SelectedItem != null
                )
                ? cbTaskList.SelectedItem.ToString()
                : "공부";

            // ----------------------------------------------------
            // 시간 칠하기
            // ----------------------------------------------------
            for (int h = startHour;
                 h <= endHour;
                 h++)
            {
                if (h >= 0 && h < 24)
                {
                    dgvTimeTable.Rows[h]
                        .Cells[1]
                        .Style
                        .BackColor =
                        highlightColor;

                    dgvTimeTable.Rows[h]
                        .Cells[1]
                        .Value =
                        $"  [ {selectedTask} ]";
                }
            }

            SaveCurrentPlanner();
        }

        // ============================================================
        // [할 일 삭제]
        // ============================================================
        private void btnDeleteTask_Click_Click(
            object sender,
            EventArgs e)
        {
            if (dgvTodoList == null)
                return;

            if (dgvTodoList.SelectedRows.Count == 0 &&
                dgvTodoList.SelectedCells.Count == 0)
            {
                MessageBox.Show(
                    "삭제할 할 일 항목을 선택해주세요.",
                    "안내",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            HashSet<DataGridViewRow> rowsToDelete =
                new HashSet<DataGridViewRow>();

            // 선택된 행
            foreach (DataGridViewRow row
                     in dgvTodoList.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    rowsToDelete.Add(row);
                }
            }

            // 선택된 셀의 행
            foreach (DataGridViewCell cell
                     in dgvTodoList.SelectedCells)
            {
                if (cell.RowIndex >= 0 &&
                    !dgvTodoList
                        .Rows[cell.RowIndex]
                        .IsNewRow)
                {
                    rowsToDelete.Add(
                        dgvTodoList.Rows[
                            cell.RowIndex
                        ]
                    );
                }
            }

            // 실제 삭제
            foreach (DataGridViewRow row
                     in rowsToDelete)
            {
                string taskToDelete =
                    row.Cells[1]
                       .Value?
                       .ToString();

                if (!string.IsNullOrEmpty(
                    taskToDelete) &&
                    cbTaskList != null)
                {
                    cbTaskList.Items.Remove(
                        taskToDelete
                    );
                }

                dgvTodoList.Rows.Remove(row);
            }

            if (cbTaskList != null)
            {
                if (cbTaskList.Items.Count > 0)
                {
                    cbTaskList.SelectedIndex = 0;
                }
                else
                {
                    cbTaskList.Text = "";
                }
            }

            SaveCurrentPlanner();
        }

        // ============================================================
        // [시간표 지우기]
        // ============================================================
        private void btnClearTime_Click_Click(
            object sender,
            EventArgs e)
        {
            if (dtpStart == null ||
                dtpEnd == null ||
                dgvTimeTable == null)
            {
                return;
            }

            int startHour =
                dtpStart.Value.Hour;

            int endHour =
                dtpEnd.Value.Hour;

            int endMinute =
                dtpEnd.Value.Minute;

            int fillStartHour =
                startHour;

            int fillEndHour =
                endHour;

            if (endMinute == 0 &&
                endHour > startHour)
            {
                fillEndHour =
                    endHour - 1;
            }

            for (int h = fillStartHour;
                 h <= fillEndHour;
                 h++)
            {
                if (h >= 0 && h < 24)
                {
                    dgvTimeTable.Rows[h]
                        .Cells[1]
                        .Style
                        .BackColor =
                        Color.White;

                    dgvTimeTable.Rows[h]
                        .Cells[1]
                        .Style
                        .SelectionBackColor =
                        SystemColors.Highlight;

                    dgvTimeTable.Rows[h]
                        .Cells[1]
                        .Value = "";
                }
            }

            SaveCurrentPlanner();
        }

        // ============================================================
        // 컨트롤이 종료될 때 저장
        // ============================================================
        protected override void OnHandleDestroyed(
            EventArgs e)
        {
            SaveCurrentPlanner();

            base.OnHandleDestroyed(e);
        }
    }
}
