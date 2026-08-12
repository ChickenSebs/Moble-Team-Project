namespace calendar4
{
    partial class PlannerControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnDeleteTask_Click = new Button();
            dgvTodoList = new DataGridView();
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            btnAddTask = new Button();
            txtTaskInput = new TextBox();
            lblLeftTitle = new Label();
            panel2 = new Panel();
            btnClearTime_Click = new Button();
            dgvTimeTable = new DataGridView();
            colTime = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnFillTime = new Button();
            cbColorPicker = new ComboBox();
            dtpEnd = new DateTimePicker();
            dtpStart = new DateTimePicker();
            label3 = new Label();
            cbTaskList = new ComboBox();
            notice = new Label();
            lblRightTitle = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTodoList).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimeTable).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnDeleteTask_Click);
            panel1.Controls.Add(dgvTodoList);
            panel1.Controls.Add(btnAddTask);
            panel1.Controls.Add(txtTaskInput);
            panel1.Controls.Add(lblLeftTitle);
            panel1.Location = new Point(3, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(354, 499);
            panel1.TabIndex = 0;
            // 
            // btnDeleteTask_Click
            // 
            btnDeleteTask_Click.Location = new Point(280, 68);
            btnDeleteTask_Click.Name = "btnDeleteTask_Click";
            btnDeleteTask_Click.Size = new Size(55, 23);
            btnDeleteTask_Click.TabIndex = 10;
            btnDeleteTask_Click.Text = "삭제";
            btnDeleteTask_Click.UseVisualStyleBackColor = true;
            btnDeleteTask_Click.Click += btnDeleteTask_Click_Click;
            // 
            // dgvTodoList
            // 
            dgvTodoList.AllowUserToAddRows = false;
            dgvTodoList.BackgroundColor = SystemColors.Window;
            dgvTodoList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTodoList.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2 });
            dgvTodoList.Location = new Point(3, 96);
            dgvTodoList.MultiSelect = false;
            dgvTodoList.Name = "dgvTodoList";
            dgvTodoList.RowHeadersVisible = false;
            dgvTodoList.RowTemplate.Height = 25;
            dgvTodoList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTodoList.Size = new Size(348, 398);
            dgvTodoList.TabIndex = 3;
            // 
            // Column1
            // 
            Column1.HeaderText = "체크";
            Column1.Name = "Column1";
            Column1.Width = 40;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column2.HeaderText = "할 일";
            Column2.Name = "Column2";
            // 
            // btnAddTask
            // 
            btnAddTask.Location = new Point(280, 40);
            btnAddTask.Name = "btnAddTask";
            btnAddTask.Size = new Size(55, 23);
            btnAddTask.TabIndex = 2;
            btnAddTask.Text = "추가";
            btnAddTask.UseVisualStyleBackColor = true;
            btnAddTask.Click += btnAddTask_Click;
            // 
            // txtTaskInput
            // 
            txtTaskInput.Location = new Point(15, 40);
            txtTaskInput.Name = "txtTaskInput";
            txtTaskInput.Size = new Size(244, 23);
            txtTaskInput.TabIndex = 1;
            // 
            // lblLeftTitle
            // 
            lblLeftTitle.AutoSize = true;
            lblLeftTitle.Font = new Font("한컴 말랑말랑 Regular", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblLeftTitle.Location = new Point(15, 16);
            lblLeftTitle.Name = "lblLeftTitle";
            lblLeftTitle.Size = new Size(144, 21);
            lblLeftTitle.TabIndex = 0;
            lblLeftTitle.Text = "📋오늘의 체크리스트";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(btnClearTime_Click);
            panel2.Controls.Add(dgvTimeTable);
            panel2.Controls.Add(btnFillTime);
            panel2.Controls.Add(cbColorPicker);
            panel2.Controls.Add(dtpEnd);
            panel2.Controls.Add(dtpStart);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(cbTaskList);
            panel2.Controls.Add(notice);
            panel2.Controls.Add(lblRightTitle);
            panel2.Location = new Point(363, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(435, 499);
            panel2.TabIndex = 1;
            // 
            // btnClearTime_Click
            // 
            btnClearTime_Click.Location = new Point(324, 84);
            btnClearTime_Click.Name = "btnClearTime_Click";
            btnClearTime_Click.Size = new Size(75, 23);
            btnClearTime_Click.TabIndex = 9;
            btnClearTime_Click.Text = "삭제";
            btnClearTime_Click.UseVisualStyleBackColor = true;
            btnClearTime_Click.Click += btnClearTime_Click_Click;
            // 
            // dgvTimeTable
            // 
            dgvTimeTable.AllowUserToAddRows = false;
            dgvTimeTable.BackgroundColor = Color.White;
            dgvTimeTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTimeTable.Columns.AddRange(new DataGridViewColumn[] { colTime, colStatus });
            dgvTimeTable.Location = new Point(3, 113);
            dgvTimeTable.Name = "dgvTimeTable";
            dgvTimeTable.ReadOnly = true;
            dgvTimeTable.RowHeadersVisible = false;
            dgvTimeTable.RowTemplate.Height = 25;
            dgvTimeTable.Size = new Size(427, 385);
            dgvTimeTable.TabIndex = 8;
            // 
            // colTime
            // 
            colTime.HeaderText = "시간";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStatus.HeaderText = "일정 진행 한 시간";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // btnFillTime
            // 
            btnFillTime.BackColor = Color.LightYellow;
            btnFillTime.Cursor = Cursors.Hand;
            btnFillTime.ForeColor = SystemColors.ActiveCaptionText;
            btnFillTime.Location = new Point(324, 55);
            btnFillTime.Name = "btnFillTime";
            btnFillTime.Size = new Size(75, 23);
            btnFillTime.TabIndex = 7;
            btnFillTime.Text = "🎨 칠하기";
            btnFillTime.UseVisualStyleBackColor = false;
            btnFillTime.Click += btnFillTime_Click;
            // 
            // cbColorPicker
            // 
            cbColorPicker.FormattingEnabled = true;
            cbColorPicker.Items.AddRange(new object[] { "\U0001f7e1 노랑", "", "\U0001f7e2 연두", "", "🔵 하늘", "", "\U0001f7e3 보라", "", "🔴 핑크" });
            cbColorPicker.Location = new Point(208, 59);
            cbColorPicker.Name = "cbColorPicker";
            cbColorPicker.Size = new Size(110, 23);
            cbColorPicker.TabIndex = 6;
            cbColorPicker.Text = "형광펜 색상";
            // 
            // dtpEnd
            // 
            dtpEnd.CustomFormat = "HH:mm";
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.Location = new Point(121, 59);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.ShowUpDown = true;
            dtpEnd.Size = new Size(68, 23);
            dtpEnd.TabIndex = 5;
            dtpEnd.UseWaitCursor = true;
            // 
            // dtpStart
            // 
            dtpStart.CustomFormat = "HH:mm";
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.Location = new Point(12, 59);
            dtpStart.Name = "dtpStart";
            dtpStart.ShowUpDown = true;
            dtpStart.Size = new Size(68, 23);
            dtpStart.TabIndex = 4;
            dtpStart.UseWaitCursor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(86, 57);
            label3.Name = "label3";
            label3.Size = new Size(26, 25);
            label3.TabIndex = 3;
            label3.Text = "~";
            // 
            // cbTaskList
            // 
            cbTaskList.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTaskList.FormattingEnabled = true;
            cbTaskList.Location = new Point(93, 26);
            cbTaskList.Name = "cbTaskList";
            cbTaskList.Size = new Size(330, 23);
            cbTaskList.TabIndex = 2;
            // 
            // notice
            // 
            notice.AutoSize = true;
            notice.Location = new Point(17, 29);
            notice.Name = "notice";
            notice.Size = new Size(63, 15);
            notice.TabIndex = 1;
            notice.Text = "할 일 선택";
            // 
            // lblRightTitle
            // 
            lblRightTitle.AutoSize = true;
            lblRightTitle.Font = new Font("한컴 말랑말랑 Regular", 11.2499981F, FontStyle.Regular, GraphicsUnit.Point);
            lblRightTitle.Location = new Point(17, 0);
            lblRightTitle.Name = "lblRightTitle";
            lblRightTitle.Size = new Size(142, 20);
            lblRightTitle.TabIndex = 0;
            lblRightTitle.Text = "⏱️ 스터디 타임테이블";
            // 
            // PlannerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "PlannerControl";
            Size = new Size(814, 502);
            Load += PlannerControl_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTodoList).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimeTable).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox txtTaskInput;
        private Label lblLeftTitle;
        private Panel panel2;
        private DataGridView dgvTodoList;
        private Button btnAddTask;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DateTimePicker dtpStart;
        private Label label3;
        private ComboBox cbTaskList;
        private Label notice;
        private Label lblRightTitle;
        private DateTimePicker dtpEnd;
        private DataGridView dgvTimeTable;
        private Button btnFillTime;
        private ComboBox cbColorPicker;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colStatus;
        private Button btnClearTime_Click;
        private Button btnDeleteTask_Click;
    }
}
