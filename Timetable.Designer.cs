namespace calendar4
{
    partial class Timetable
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            rootLayout = new TableLayoutPanel();
            todayPanel = new Panel();
            todayLayout = new TableLayoutPanel();
            lblTodayTitle = new Label();
            lblTodayDate = new Label();
            progressPanel = new Panel();
            lblProgressTitle = new Label();
            lblProgressCount = new Label();
            todayProgressBar = new ProgressBar();
            lblProgressMessage = new Label();
            lblTodayClasses = new Label();
            todayClassList = new FlowLayoutPanel();
            classCard1 = new Panel();
            classAccent1 = new Panel();
            lblClassTime1 = new Label();
            lblClassName1 = new Label();
            lblClassRoom1 = new Label();
            lblClassStatus1 = new Label();
            classCard2 = new Panel();
            classAccent2 = new Panel();
            lblClassTime2 = new Label();
            lblClassName2 = new Label();
            lblClassRoom2 = new Label();
            lblClassStatus2 = new Label();
            classCard3 = new Panel();
            classAccent3 = new Panel();
            lblClassTime3 = new Label();
            lblClassName3 = new Label();
            lblClassRoom3 = new Label();
            lblClassStatus3 = new Label();
            schedulePanel = new Panel();
            scheduleLayout = new TableLayoutPanel();
            scheduleHeader = new Panel();
            lblScheduleTitle = new Label();
            lblSemester = new Label();
            btnAddClass = new Button();
            legendPanel = new FlowLayoutPanel();
            legendBlue = new Label();
            lblMajorLegend = new Label();
            legendGreen = new Label();
            lblStudyLegend = new Label();
            legendOrange = new Label();
            lblEtcLegend = new Label();
            scheduleScrollPanel = new Panel();
            scheduleTable = new TableLayoutPanel();
            lblHeaderTime = new Label();
            lblHeaderMonday = new Label();
            lblHeaderTuesday = new Label();
            lblHeaderWednesday = new Label();
            lblHeaderThursday = new Label();
            lblHeaderFriday = new Label();
            lblTime0900 = new Label();
            lblTime1000 = new Label();
            lblTime1100 = new Label();
            lblTime1200 = new Label();
            lblTime1300 = new Label();
            lblTime1400 = new Label();
            lblTime1500 = new Label();
            lblTime1600 = new Label();
            lblTime1700 = new Label();
            lblSubjectDataStructure = new Label();
            lblSubjectOperatingSystem = new Label();
            lblSubjectCSharp = new Label();
            lblSubjectWeb = new Label();
            lblSubjectDatabase = new Label();
            lblSubjectComputer = new Label();
            lblSubjectProject = new Label();
            lblSubjectEnglish = new Label();
            rootLayout.SuspendLayout();
            todayPanel.SuspendLayout();
            todayLayout.SuspendLayout();
            progressPanel.SuspendLayout();
            todayClassList.SuspendLayout();
            classCard1.SuspendLayout();
            classCard2.SuspendLayout();
            classCard3.SuspendLayout();
            schedulePanel.SuspendLayout();
            scheduleLayout.SuspendLayout();
            scheduleHeader.SuspendLayout();
            legendPanel.SuspendLayout();
            scheduleScrollPanel.SuspendLayout();
            scheduleTable.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.BackColor = Color.FromArgb(244, 246, 250);
            rootLayout.ColumnCount = 3;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(todayPanel, 0, 0);
            rootLayout.Controls.Add(schedulePanel, 2, 0);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(12);
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(808, 496);
            rootLayout.TabIndex = 0;
            // 
            // todayPanel
            // 
            todayPanel.BackColor = Color.White;
            todayPanel.Controls.Add(todayLayout);
            todayPanel.Dock = DockStyle.Fill;
            todayPanel.Location = new Point(15, 15);
            todayPanel.Name = "todayPanel";
            todayPanel.Padding = new Padding(22);
            todayPanel.Size = new Size(274, 466);
            todayPanel.TabIndex = 0;
            // 
            // todayLayout
            // 
            todayLayout.ColumnCount = 1;
            todayLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            todayLayout.Controls.Add(lblTodayTitle, 0, 0);
            todayLayout.Controls.Add(lblTodayDate, 0, 1);
            todayLayout.Controls.Add(progressPanel, 0, 3);
            todayLayout.Controls.Add(lblTodayClasses, 0, 5);
            todayLayout.Controls.Add(todayClassList, 0, 6);
            todayLayout.Dock = DockStyle.Fill;
            todayLayout.Location = new Point(22, 22);
            todayLayout.Name = "todayLayout";
            todayLayout.RowCount = 7;
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            todayLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            todayLayout.Size = new Size(230, 422);
            todayLayout.TabIndex = 0;
            // 
            // lblTodayTitle
            // 
            lblTodayTitle.AutoSize = true;
            lblTodayTitle.Dock = DockStyle.Fill;
            lblTodayTitle.Font = new Font("맑은 고딕", 17F, FontStyle.Bold, GraphicsUnit.Point);
            lblTodayTitle.ForeColor = Color.FromArgb(31, 42, 68);
            lblTodayTitle.Location = new Point(0, 0);
            lblTodayTitle.Margin = new Padding(0);
            lblTodayTitle.Name = "lblTodayTitle";
            lblTodayTitle.Size = new Size(230, 38);
            lblTodayTitle.TabIndex = 0;
            lblTodayTitle.Text = "오늘 수업";
            // 
            // lblTodayDate
            // 
            lblTodayDate.AutoSize = true;
            lblTodayDate.Dock = DockStyle.Fill;
            lblTodayDate.Font = new Font("맑은 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTodayDate.ForeColor = Color.FromArgb(107, 114, 128);
            lblTodayDate.Location = new Point(0, 38);
            lblTodayDate.Margin = new Padding(0);
            lblTodayDate.Name = "lblTodayDate";
            lblTodayDate.Size = new Size(230, 26);
            lblTodayDate.TabIndex = 1;
            lblTodayDate.Text = "8월 11일 화요일";
            // 
            // progressPanel
            // 
            progressPanel.BackColor = Color.FromArgb(245, 247, 255);
            progressPanel.Controls.Add(lblProgressTitle);
            progressPanel.Controls.Add(lblProgressCount);
            progressPanel.Controls.Add(todayProgressBar);
            progressPanel.Controls.Add(lblProgressMessage);
            progressPanel.Dock = DockStyle.Fill;
            progressPanel.Location = new Point(0, 84);
            progressPanel.Margin = new Padding(0);
            progressPanel.Name = "progressPanel";
            progressPanel.Size = new Size(230, 116);
            progressPanel.TabIndex = 2;
            // 
            // lblProgressTitle
            // 
            lblProgressTitle.AutoSize = true;
            lblProgressTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblProgressTitle.ForeColor = Color.FromArgb(55, 65, 81);
            lblProgressTitle.Location = new Point(16, 14);
            lblProgressTitle.Name = "lblProgressTitle";
            lblProgressTitle.Size = new Size(84, 19);
            lblProgressTitle.TabIndex = 0;
            lblProgressTitle.Text = "수업 진행도";
            // 
            // lblProgressCount
            // 
            lblProgressCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblProgressCount.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblProgressCount.ForeColor = Color.FromArgb(79, 107, 237);
            lblProgressCount.Location = new Point(148, 14);
            lblProgressCount.Name = "lblProgressCount";
            lblProgressCount.Size = new Size(66, 19);
            lblProgressCount.TabIndex = 1;
            lblProgressCount.Text = "2 / 4";
            lblProgressCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // todayProgressBar
            // 
            todayProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            todayProgressBar.Location = new Point(16, 44);
            todayProgressBar.Maximum = 4;
            todayProgressBar.Name = "todayProgressBar";
            todayProgressBar.Size = new Size(198, 12);
            todayProgressBar.Style = ProgressBarStyle.Continuous;
            todayProgressBar.TabIndex = 2;
            todayProgressBar.Value = 2;
            // 
            // lblProgressMessage
            // 
            lblProgressMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblProgressMessage.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblProgressMessage.ForeColor = Color.FromArgb(107, 114, 128);
            lblProgressMessage.Location = new Point(16, 69);
            lblProgressMessage.Name = "lblProgressMessage";
            lblProgressMessage.Size = new Size(198, 32);
            lblProgressMessage.TabIndex = 3;
            lblProgressMessage.Text = "오후 수업 2개가 남아 있어요.";
            // 
            // lblTodayClasses
            // 
            lblTodayClasses.AutoSize = true;
            lblTodayClasses.Dock = DockStyle.Fill;
            lblTodayClasses.Font = new Font("맑은 고딕", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblTodayClasses.ForeColor = Color.FromArgb(31, 42, 68);
            lblTodayClasses.Location = new Point(0, 224);
            lblTodayClasses.Margin = new Padding(0);
            lblTodayClasses.Name = "lblTodayClasses";
            lblTodayClasses.Size = new Size(230, 32);
            lblTodayClasses.TabIndex = 3;
            lblTodayClasses.Text = "수업 목록";
            lblTodayClasses.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // todayClassList
            // 
            todayClassList.AutoScroll = true;
            todayClassList.Controls.Add(classCard1);
            todayClassList.Controls.Add(classCard2);
            todayClassList.Controls.Add(classCard3);
            todayClassList.Dock = DockStyle.Fill;
            todayClassList.FlowDirection = FlowDirection.TopDown;
            todayClassList.Location = new Point(0, 256);
            todayClassList.Margin = new Padding(0);
            todayClassList.Name = "todayClassList";
            todayClassList.Size = new Size(230, 166);
            todayClassList.TabIndex = 4;
            todayClassList.WrapContents = false;
            // 
            // classCard1
            // 
            classCard1.BackColor = Color.FromArgb(247, 249, 252);
            classCard1.Controls.Add(classAccent1);
            classCard1.Controls.Add(lblClassTime1);
            classCard1.Controls.Add(lblClassName1);
            classCard1.Controls.Add(lblClassRoom1);
            classCard1.Controls.Add(lblClassStatus1);
            classCard1.Location = new Point(0, 0);
            classCard1.Margin = new Padding(0, 0, 0, 10);
            classCard1.Name = "classCard1";
            classCard1.Size = new Size(212, 88);
            classCard1.TabIndex = 0;
            // 
            // classAccent1
            // 
            classAccent1.BackColor = Color.FromArgb(79, 107, 237);
            classAccent1.Dock = DockStyle.Left;
            classAccent1.Location = new Point(0, 0);
            classAccent1.Name = "classAccent1";
            classAccent1.Size = new Size(5, 88);
            classAccent1.TabIndex = 0;
            // 
            // lblClassTime1
            // 
            lblClassTime1.AutoSize = true;
            lblClassTime1.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassTime1.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassTime1.Location = new Point(17, 11);
            lblClassTime1.Name = "lblClassTime1";
            lblClassTime1.Size = new Size(82, 15);
            lblClassTime1.TabIndex = 1;
            lblClassTime1.Text = "09:00 - 10:30";
            // 
            // lblClassName1
            // 
            lblClassName1.AutoSize = true;
            lblClassName1.Font = new Font("맑은 고딕", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassName1.ForeColor = Color.FromArgb(31, 42, 68);
            lblClassName1.Location = new Point(16, 33);
            lblClassName1.Name = "lblClassName1";
            lblClassName1.Size = new Size(65, 19);
            lblClassName1.TabIndex = 2;
            lblClassName1.Text = "자료구조";
            // 
            // lblClassRoom1
            // 
            lblClassRoom1.AutoSize = true;
            lblClassRoom1.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassRoom1.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassRoom1.Location = new Point(17, 59);
            lblClassRoom1.Name = "lblClassRoom1";
            lblClassRoom1.Size = new Size(80, 15);
            lblClassRoom1.TabIndex = 3;
            lblClassRoom1.Text = "공학관 301호";
            // 
            // lblClassStatus1
            // 
            lblClassStatus1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblClassStatus1.BackColor = Color.FromArgb(229, 247, 238);
            lblClassStatus1.Font = new Font("맑은 고딕", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassStatus1.ForeColor = Color.FromArgb(35, 134, 87);
            lblClassStatus1.Location = new Point(190, 12);
            lblClassStatus1.Name = "lblClassStatus1";
            lblClassStatus1.Size = new Size(50, 22);
            lblClassStatus1.TabIndex = 4;
            lblClassStatus1.Text = "완료";
            lblClassStatus1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // classCard2
            // 
            classCard2.BackColor = Color.FromArgb(247, 249, 252);
            classCard2.Controls.Add(classAccent2);
            classCard2.Controls.Add(lblClassTime2);
            classCard2.Controls.Add(lblClassName2);
            classCard2.Controls.Add(lblClassRoom2);
            classCard2.Controls.Add(lblClassStatus2);
            classCard2.Location = new Point(0, 98);
            classCard2.Margin = new Padding(0, 0, 0, 10);
            classCard2.Name = "classCard2";
            classCard2.Size = new Size(212, 88);
            classCard2.TabIndex = 1;
            // 
            // classAccent2
            // 
            classAccent2.BackColor = Color.FromArgb(46, 157, 103);
            classAccent2.Dock = DockStyle.Left;
            classAccent2.Location = new Point(0, 0);
            classAccent2.Name = "classAccent2";
            classAccent2.Size = new Size(5, 88);
            classAccent2.TabIndex = 0;
            // 
            // lblClassTime2
            // 
            lblClassTime2.AutoSize = true;
            lblClassTime2.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassTime2.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassTime2.Location = new Point(17, 11);
            lblClassTime2.Name = "lblClassTime2";
            lblClassTime2.Size = new Size(82, 15);
            lblClassTime2.TabIndex = 1;
            lblClassTime2.Text = "11:00 - 12:30";
            // 
            // lblClassName2
            // 
            lblClassName2.AutoSize = true;
            lblClassName2.Font = new Font("맑은 고딕", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassName2.ForeColor = Color.FromArgb(31, 42, 68);
            lblClassName2.Location = new Point(16, 33);
            lblClassName2.Name = "lblClassName2";
            lblClassName2.Size = new Size(102, 19);
            lblClassName2.TabIndex = 2;
            lblClassName2.Text = "C# 프로그래밍";
            // 
            // lblClassRoom2
            // 
            lblClassRoom2.AutoSize = true;
            lblClassRoom2.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassRoom2.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassRoom2.Location = new Point(17, 59);
            lblClassRoom2.Name = "lblClassRoom2";
            lblClassRoom2.Size = new Size(54, 15);
            lblClassRoom2.TabIndex = 3;
            lblClassRoom2.Text = "실습실 B";
            // 
            // lblClassStatus2
            // 
            lblClassStatus2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblClassStatus2.BackColor = Color.FromArgb(231, 237, 255);
            lblClassStatus2.Font = new Font("맑은 고딕", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassStatus2.ForeColor = Color.FromArgb(79, 107, 237);
            lblClassStatus2.Location = new Point(179, 12);
            lblClassStatus2.Name = "lblClassStatus2";
            lblClassStatus2.Size = new Size(61, 22);
            lblClassStatus2.TabIndex = 4;
            lblClassStatus2.Text = "진행 중";
            lblClassStatus2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // classCard3
            // 
            classCard3.BackColor = Color.FromArgb(247, 249, 252);
            classCard3.Controls.Add(classAccent3);
            classCard3.Controls.Add(lblClassTime3);
            classCard3.Controls.Add(lblClassName3);
            classCard3.Controls.Add(lblClassRoom3);
            classCard3.Controls.Add(lblClassStatus3);
            classCard3.Location = new Point(0, 196);
            classCard3.Margin = new Padding(0, 0, 0, 10);
            classCard3.Name = "classCard3";
            classCard3.Size = new Size(212, 88);
            classCard3.TabIndex = 2;
            // 
            // classAccent3
            // 
            classAccent3.BackColor = Color.FromArgb(244, 160, 72);
            classAccent3.Dock = DockStyle.Left;
            classAccent3.Location = new Point(0, 0);
            classAccent3.Name = "classAccent3";
            classAccent3.Size = new Size(5, 88);
            classAccent3.TabIndex = 0;
            // 
            // lblClassTime3
            // 
            lblClassTime3.AutoSize = true;
            lblClassTime3.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassTime3.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassTime3.Location = new Point(17, 11);
            lblClassTime3.Name = "lblClassTime3";
            lblClassTime3.Size = new Size(82, 15);
            lblClassTime3.TabIndex = 1;
            lblClassTime3.Text = "14:00 - 15:30";
            // 
            // lblClassName3
            // 
            lblClassName3.AutoSize = true;
            lblClassName3.Font = new Font("맑은 고딕", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassName3.ForeColor = Color.FromArgb(31, 42, 68);
            lblClassName3.Location = new Point(16, 33);
            lblClassName3.Name = "lblClassName3";
            lblClassName3.Size = new Size(93, 19);
            lblClassName3.TabIndex = 2;
            lblClassName3.Text = "데이터베이스";
            // 
            // lblClassRoom3
            // 
            lblClassRoom3.AutoSize = true;
            lblClassRoom3.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblClassRoom3.ForeColor = Color.FromArgb(107, 114, 128);
            lblClassRoom3.Location = new Point(17, 59);
            lblClassRoom3.Name = "lblClassRoom3";
            lblClassRoom3.Size = new Size(80, 15);
            lblClassRoom3.TabIndex = 3;
            lblClassRoom3.Text = "공학관 202호";
            // 
            // lblClassStatus3
            // 
            lblClassStatus3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblClassStatus3.BackColor = Color.FromArgb(255, 242, 225);
            lblClassStatus3.Font = new Font("맑은 고딕", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblClassStatus3.ForeColor = Color.FromArgb(190, 106, 28);
            lblClassStatus3.Location = new Point(190, 12);
            lblClassStatus3.Name = "lblClassStatus3";
            lblClassStatus3.Size = new Size(50, 22);
            lblClassStatus3.TabIndex = 4;
            lblClassStatus3.Text = "예정";
            lblClassStatus3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // schedulePanel
            // 
            schedulePanel.BackColor = Color.White;
            schedulePanel.Controls.Add(scheduleLayout);
            schedulePanel.Dock = DockStyle.Fill;
            schedulePanel.Location = new Point(307, 15);
            schedulePanel.Name = "schedulePanel";
            schedulePanel.Padding = new Padding(22);
            schedulePanel.Size = new Size(486, 466);
            schedulePanel.TabIndex = 1;
            // 
            // scheduleLayout
            // 
            scheduleLayout.ColumnCount = 1;
            scheduleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            scheduleLayout.Controls.Add(scheduleHeader, 0, 0);
            scheduleLayout.Controls.Add(legendPanel, 0, 1);
            scheduleLayout.Controls.Add(scheduleScrollPanel, 0, 3);
            scheduleLayout.Dock = DockStyle.Fill;
            scheduleLayout.Location = new Point(22, 22);
            scheduleLayout.Name = "scheduleLayout";
            scheduleLayout.RowCount = 4;
            scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 14F));
            scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            scheduleLayout.Size = new Size(442, 422);
            scheduleLayout.TabIndex = 0;
            // 
            // scheduleHeader
            // 
            scheduleHeader.Controls.Add(lblScheduleTitle);
            scheduleHeader.Controls.Add(lblSemester);
            scheduleHeader.Controls.Add(btnAddClass);
            scheduleHeader.Dock = DockStyle.Fill;
            scheduleHeader.Location = new Point(0, 0);
            scheduleHeader.Margin = new Padding(0);
            scheduleHeader.Name = "scheduleHeader";
            scheduleHeader.Size = new Size(442, 58);
            scheduleHeader.TabIndex = 0;
            // 
            // lblScheduleTitle
            // 
            lblScheduleTitle.AutoSize = true;
            lblScheduleTitle.Font = new Font("맑은 고딕", 17F, FontStyle.Bold, GraphicsUnit.Point);
            lblScheduleTitle.ForeColor = Color.FromArgb(31, 42, 68);
            lblScheduleTitle.Location = new Point(0, 0);
            lblScheduleTitle.Name = "lblScheduleTitle";
            lblScheduleTitle.Size = new Size(114, 31);
            lblScheduleTitle.TabIndex = 0;
            lblScheduleTitle.Text = "내 시간표";
            // 
            // lblSemester
            // 
            lblSemester.AutoSize = true;
            lblSemester.Font = new Font("맑은 고딕", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblSemester.ForeColor = Color.FromArgb(107, 114, 128);
            lblSemester.Location = new Point(2, 36);
            lblSemester.Name = "lblSemester";
            lblSemester.Size = new Size(87, 17);
            lblSemester.TabIndex = 1;
            lblSemester.Text = "2026년 2학기";
            // 
            // btnAddClass
            // 
            btnAddClass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddClass.BackColor = Color.FromArgb(79, 107, 237);
            btnAddClass.FlatAppearance.BorderSize = 0;
            btnAddClass.FlatStyle = FlatStyle.Flat;
            btnAddClass.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAddClass.ForeColor = Color.White;
            btnAddClass.Location = new Point(332, 4);
            btnAddClass.Name = "btnAddClass";
            btnAddClass.Size = new Size(110, 34);
            btnAddClass.TabIndex = 2;
            btnAddClass.Text = "+ 수업 추가";
            btnAddClass.UseVisualStyleBackColor = false;
            // 
            // legendPanel
            // 
            legendPanel.Controls.Add(legendBlue);
            legendPanel.Controls.Add(lblMajorLegend);
            legendPanel.Controls.Add(legendGreen);
            legendPanel.Controls.Add(lblStudyLegend);
            legendPanel.Controls.Add(legendOrange);
            legendPanel.Controls.Add(lblEtcLegend);
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.Location = new Point(0, 58);
            legendPanel.Margin = new Padding(0);
            legendPanel.Name = "legendPanel";
            legendPanel.Padding = new Padding(0, 7, 0, 0);
            legendPanel.Size = new Size(442, 34);
            legendPanel.TabIndex = 1;
            // 
            // legendBlue
            // 
            legendBlue.BackColor = Color.FromArgb(79, 107, 237);
            legendBlue.Location = new Point(0, 11);
            legendBlue.Margin = new Padding(0, 4, 6, 0);
            legendBlue.Name = "legendBlue";
            legendBlue.Size = new Size(10, 10);
            legendBlue.TabIndex = 0;
            // 
            // lblMajorLegend
            // 
            lblMajorLegend.AutoSize = true;
            lblMajorLegend.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblMajorLegend.ForeColor = Color.FromArgb(107, 114, 128);
            lblMajorLegend.Location = new Point(16, 7);
            lblMajorLegend.Margin = new Padding(0, 0, 18, 0);
            lblMajorLegend.Name = "lblMajorLegend";
            lblMajorLegend.Size = new Size(31, 15);
            lblMajorLegend.TabIndex = 1;
            lblMajorLegend.Text = "전공";
            // 
            // legendGreen
            // 
            legendGreen.BackColor = Color.FromArgb(46, 157, 103);
            legendGreen.Location = new Point(65, 11);
            legendGreen.Margin = new Padding(0, 4, 6, 0);
            legendGreen.Name = "legendGreen";
            legendGreen.Size = new Size(10, 10);
            legendGreen.TabIndex = 2;
            // 
            // lblStudyLegend
            // 
            lblStudyLegend.AutoSize = true;
            lblStudyLegend.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStudyLegend.ForeColor = Color.FromArgb(107, 114, 128);
            lblStudyLegend.Location = new Point(81, 7);
            lblStudyLegend.Margin = new Padding(0, 0, 18, 0);
            lblStudyLegend.Name = "lblStudyLegend";
            lblStudyLegend.Size = new Size(31, 15);
            lblStudyLegend.TabIndex = 3;
            lblStudyLegend.Text = "교양";
            // 
            // legendOrange
            // 
            legendOrange.BackColor = Color.FromArgb(244, 160, 72);
            legendOrange.Location = new Point(130, 11);
            legendOrange.Margin = new Padding(0, 4, 6, 0);
            legendOrange.Name = "legendOrange";
            legendOrange.Size = new Size(10, 10);
            legendOrange.TabIndex = 4;
            // 
            // lblEtcLegend
            // 
            lblEtcLegend.AutoSize = true;
            lblEtcLegend.Font = new Font("맑은 고딕", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblEtcLegend.ForeColor = Color.FromArgb(107, 114, 128);
            lblEtcLegend.Location = new Point(146, 7);
            lblEtcLegend.Margin = new Padding(0);
            lblEtcLegend.Name = "lblEtcLegend";
            lblEtcLegend.Size = new Size(31, 15);
            lblEtcLegend.TabIndex = 5;
            lblEtcLegend.Text = "기타";
            // 
            // scheduleScrollPanel
            // 
            scheduleScrollPanel.AutoScroll = true;
            scheduleScrollPanel.Controls.Add(scheduleTable);
            scheduleScrollPanel.Dock = DockStyle.Fill;
            scheduleScrollPanel.Location = new Point(0, 106);
            scheduleScrollPanel.Margin = new Padding(0);
            scheduleScrollPanel.Name = "scheduleScrollPanel";
            scheduleScrollPanel.Size = new Size(442, 316);
            scheduleScrollPanel.TabIndex = 2;
            // 
            // scheduleTable
            // 
            scheduleTable.BackColor = Color.White;
            scheduleTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            scheduleTable.ColumnCount = 6;
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 68F));
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            scheduleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            scheduleTable.Controls.Add(lblHeaderTime, 0, 0);
            scheduleTable.Controls.Add(lblHeaderMonday, 1, 0);
            scheduleTable.Controls.Add(lblHeaderTuesday, 2, 0);
            scheduleTable.Controls.Add(lblHeaderWednesday, 3, 0);
            scheduleTable.Controls.Add(lblHeaderThursday, 4, 0);
            scheduleTable.Controls.Add(lblHeaderFriday, 5, 0);
            scheduleTable.Controls.Add(lblTime0900, 0, 1);
            scheduleTable.Controls.Add(lblTime1000, 0, 2);
            scheduleTable.Controls.Add(lblTime1100, 0, 3);
            scheduleTable.Controls.Add(lblTime1200, 0, 4);
            scheduleTable.Controls.Add(lblTime1300, 0, 5);
            scheduleTable.Controls.Add(lblTime1400, 0, 6);
            scheduleTable.Controls.Add(lblTime1500, 0, 7);
            scheduleTable.Controls.Add(lblTime1600, 0, 8);
            scheduleTable.Controls.Add(lblTime1700, 0, 9);
            scheduleTable.Controls.Add(lblSubjectDataStructure, 1, 1);
            scheduleTable.Controls.Add(lblSubjectOperatingSystem, 3, 1);
            scheduleTable.Controls.Add(lblSubjectCSharp, 2, 3);
            scheduleTable.Controls.Add(lblSubjectWeb, 4, 3);
            scheduleTable.Controls.Add(lblSubjectDatabase, 1, 6);
            scheduleTable.Controls.Add(lblSubjectComputer, 3, 6);
            scheduleTable.Controls.Add(lblSubjectProject, 5, 6);
            scheduleTable.Controls.Add(lblSubjectEnglish, 2, 8);
            scheduleTable.Dock = DockStyle.Top;
            scheduleTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            scheduleTable.Location = new Point(0, 0);
            scheduleTable.Margin = new Padding(0);
            scheduleTable.Name = "scheduleTable";
            scheduleTable.RowCount = 10;
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            scheduleTable.Size = new Size(425, 535);
            scheduleTable.TabIndex = 0;
            // 
            // lblHeaderTime
            // 
            lblHeaderTime.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderTime.Dock = DockStyle.Fill;
            lblHeaderTime.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderTime.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderTime.Location = new Point(1, 1);
            lblHeaderTime.Margin = new Padding(0);
            lblHeaderTime.Name = "lblHeaderTime";
            lblHeaderTime.Size = new Size(68, 44);
            lblHeaderTime.TabIndex = 0;
            lblHeaderTime.Text = "시간";
            lblHeaderTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderMonday
            // 
            lblHeaderMonday.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderMonday.Dock = DockStyle.Fill;
            lblHeaderMonday.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderMonday.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderMonday.Location = new Point(70, 1);
            lblHeaderMonday.Margin = new Padding(0);
            lblHeaderMonday.Name = "lblHeaderMonday";
            lblHeaderMonday.Size = new Size(70, 44);
            lblHeaderMonday.TabIndex = 1;
            lblHeaderMonday.Text = "월";
            lblHeaderMonday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderTuesday
            // 
            lblHeaderTuesday.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderTuesday.Dock = DockStyle.Fill;
            lblHeaderTuesday.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderTuesday.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderTuesday.Location = new Point(141, 1);
            lblHeaderTuesday.Margin = new Padding(0);
            lblHeaderTuesday.Name = "lblHeaderTuesday";
            lblHeaderTuesday.Size = new Size(70, 44);
            lblHeaderTuesday.TabIndex = 2;
            lblHeaderTuesday.Text = "화";
            lblHeaderTuesday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderWednesday
            // 
            lblHeaderWednesday.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderWednesday.Dock = DockStyle.Fill;
            lblHeaderWednesday.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderWednesday.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderWednesday.Location = new Point(212, 1);
            lblHeaderWednesday.Margin = new Padding(0);
            lblHeaderWednesday.Name = "lblHeaderWednesday";
            lblHeaderWednesday.Size = new Size(70, 44);
            lblHeaderWednesday.TabIndex = 3;
            lblHeaderWednesday.Text = "수";
            lblHeaderWednesday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderThursday
            // 
            lblHeaderThursday.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderThursday.Dock = DockStyle.Fill;
            lblHeaderThursday.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderThursday.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderThursday.Location = new Point(283, 1);
            lblHeaderThursday.Margin = new Padding(0);
            lblHeaderThursday.Name = "lblHeaderThursday";
            lblHeaderThursday.Size = new Size(70, 44);
            lblHeaderThursday.TabIndex = 4;
            lblHeaderThursday.Text = "목";
            lblHeaderThursday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderFriday
            // 
            lblHeaderFriday.BackColor = Color.FromArgb(238, 242, 255);
            lblHeaderFriday.Dock = DockStyle.Fill;
            lblHeaderFriday.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeaderFriday.ForeColor = Color.FromArgb(55, 65, 81);
            lblHeaderFriday.Location = new Point(354, 1);
            lblHeaderFriday.Margin = new Padding(0);
            lblHeaderFriday.Name = "lblHeaderFriday";
            lblHeaderFriday.Size = new Size(70, 44);
            lblHeaderFriday.TabIndex = 5;
            lblHeaderFriday.Text = "금";
            lblHeaderFriday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime0900
            // 
            lblTime0900.BackColor = Color.FromArgb(249, 250, 252);
            lblTime0900.Dock = DockStyle.Fill;
            lblTime0900.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime0900.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime0900.Location = new Point(1, 46);
            lblTime0900.Margin = new Padding(0);
            lblTime0900.Name = "lblTime0900";
            lblTime0900.Size = new Size(68, 54);
            lblTime0900.TabIndex = 6;
            lblTime0900.Text = "09:00";
            lblTime0900.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1000
            // 
            lblTime1000.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1000.Dock = DockStyle.Fill;
            lblTime1000.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1000.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1000.Location = new Point(1, 101);
            lblTime1000.Margin = new Padding(0);
            lblTime1000.Name = "lblTime1000";
            lblTime1000.Size = new Size(68, 54);
            lblTime1000.TabIndex = 7;
            lblTime1000.Text = "10:00";
            lblTime1000.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1100
            // 
            lblTime1100.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1100.Dock = DockStyle.Fill;
            lblTime1100.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1100.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1100.Location = new Point(1, 156);
            lblTime1100.Margin = new Padding(0);
            lblTime1100.Name = "lblTime1100";
            lblTime1100.Size = new Size(68, 54);
            lblTime1100.TabIndex = 8;
            lblTime1100.Text = "11:00";
            lblTime1100.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1200
            // 
            lblTime1200.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1200.Dock = DockStyle.Fill;
            lblTime1200.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1200.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1200.Location = new Point(1, 211);
            lblTime1200.Margin = new Padding(0);
            lblTime1200.Name = "lblTime1200";
            lblTime1200.Size = new Size(68, 54);
            lblTime1200.TabIndex = 9;
            lblTime1200.Text = "12:00";
            lblTime1200.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1300
            // 
            lblTime1300.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1300.Dock = DockStyle.Fill;
            lblTime1300.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1300.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1300.Location = new Point(1, 266);
            lblTime1300.Margin = new Padding(0);
            lblTime1300.Name = "lblTime1300";
            lblTime1300.Size = new Size(68, 54);
            lblTime1300.TabIndex = 10;
            lblTime1300.Text = "13:00";
            lblTime1300.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1400
            // 
            lblTime1400.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1400.Dock = DockStyle.Fill;
            lblTime1400.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1400.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1400.Location = new Point(1, 321);
            lblTime1400.Margin = new Padding(0);
            lblTime1400.Name = "lblTime1400";
            lblTime1400.Size = new Size(68, 54);
            lblTime1400.TabIndex = 11;
            lblTime1400.Text = "14:00";
            lblTime1400.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1500
            // 
            lblTime1500.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1500.Dock = DockStyle.Fill;
            lblTime1500.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1500.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1500.Location = new Point(1, 376);
            lblTime1500.Margin = new Padding(0);
            lblTime1500.Name = "lblTime1500";
            lblTime1500.Size = new Size(68, 54);
            lblTime1500.TabIndex = 12;
            lblTime1500.Text = "15:00";
            lblTime1500.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1600
            // 
            lblTime1600.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1600.Dock = DockStyle.Fill;
            lblTime1600.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1600.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1600.Location = new Point(1, 431);
            lblTime1600.Margin = new Padding(0);
            lblTime1600.Name = "lblTime1600";
            lblTime1600.Size = new Size(68, 54);
            lblTime1600.TabIndex = 13;
            lblTime1600.Text = "16:00";
            lblTime1600.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTime1700
            // 
            lblTime1700.BackColor = Color.FromArgb(249, 250, 252);
            lblTime1700.Dock = DockStyle.Fill;
            lblTime1700.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTime1700.ForeColor = Color.FromArgb(107, 114, 128);
            lblTime1700.Location = new Point(1, 486);
            lblTime1700.Margin = new Padding(0);
            lblTime1700.Name = "lblTime1700";
            lblTime1700.Size = new Size(68, 54);
            lblTime1700.TabIndex = 14;
            lblTime1700.Text = "17:00";
            lblTime1700.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectDataStructure
            // 
            lblSubjectDataStructure.BackColor = Color.FromArgb(231, 237, 255);
            lblSubjectDataStructure.Dock = DockStyle.Fill;
            lblSubjectDataStructure.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectDataStructure.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectDataStructure.Location = new Point(70, 46);
            lblSubjectDataStructure.Margin = new Padding(0);
            lblSubjectDataStructure.Name = "lblSubjectDataStructure";
            scheduleTable.SetRowSpan(lblSubjectDataStructure, 2);
            lblSubjectDataStructure.Size = new Size(70, 109);
            lblSubjectDataStructure.TabIndex = 15;
            lblSubjectDataStructure.Text = "자료구조\r\n공학관 301";
            lblSubjectDataStructure.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectOperatingSystem
            // 
            lblSubjectOperatingSystem.BackColor = Color.FromArgb(231, 237, 255);
            lblSubjectOperatingSystem.Dock = DockStyle.Fill;
            lblSubjectOperatingSystem.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectOperatingSystem.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectOperatingSystem.Location = new Point(212, 46);
            lblSubjectOperatingSystem.Margin = new Padding(0);
            lblSubjectOperatingSystem.Name = "lblSubjectOperatingSystem";
            scheduleTable.SetRowSpan(lblSubjectOperatingSystem, 2);
            lblSubjectOperatingSystem.Size = new Size(70, 109);
            lblSubjectOperatingSystem.TabIndex = 16;
            lblSubjectOperatingSystem.Text = "운영체제\r\n공학관 204";
            lblSubjectOperatingSystem.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectCSharp
            // 
            lblSubjectCSharp.BackColor = Color.FromArgb(229, 247, 238);
            lblSubjectCSharp.Dock = DockStyle.Fill;
            lblSubjectCSharp.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectCSharp.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectCSharp.Location = new Point(141, 156);
            lblSubjectCSharp.Margin = new Padding(0);
            lblSubjectCSharp.Name = "lblSubjectCSharp";
            scheduleTable.SetRowSpan(lblSubjectCSharp, 2);
            lblSubjectCSharp.Size = new Size(70, 109);
            lblSubjectCSharp.TabIndex = 17;
            lblSubjectCSharp.Text = "C# 프로그래밍\r\n실습실 B";
            lblSubjectCSharp.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectWeb
            // 
            lblSubjectWeb.BackColor = Color.FromArgb(231, 237, 255);
            lblSubjectWeb.Dock = DockStyle.Fill;
            lblSubjectWeb.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectWeb.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectWeb.Location = new Point(283, 156);
            lblSubjectWeb.Margin = new Padding(0);
            lblSubjectWeb.Name = "lblSubjectWeb";
            scheduleTable.SetRowSpan(lblSubjectWeb, 2);
            lblSubjectWeb.Size = new Size(70, 109);
            lblSubjectWeb.TabIndex = 18;
            lblSubjectWeb.Text = "웹 프로그래밍\r\n실습실 A";
            lblSubjectWeb.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectDatabase
            // 
            lblSubjectDatabase.BackColor = Color.FromArgb(231, 237, 255);
            lblSubjectDatabase.Dock = DockStyle.Fill;
            lblSubjectDatabase.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectDatabase.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectDatabase.Location = new Point(70, 321);
            lblSubjectDatabase.Margin = new Padding(0);
            lblSubjectDatabase.Name = "lblSubjectDatabase";
            scheduleTable.SetRowSpan(lblSubjectDatabase, 2);
            lblSubjectDatabase.Size = new Size(70, 109);
            lblSubjectDatabase.TabIndex = 19;
            lblSubjectDatabase.Text = "데이터베이스\r\n공학관 202";
            lblSubjectDatabase.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectComputer
            // 
            lblSubjectComputer.BackColor = Color.FromArgb(231, 237, 255);
            lblSubjectComputer.Dock = DockStyle.Fill;
            lblSubjectComputer.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectComputer.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectComputer.Location = new Point(212, 321);
            lblSubjectComputer.Margin = new Padding(0);
            lblSubjectComputer.Name = "lblSubjectComputer";
            scheduleTable.SetRowSpan(lblSubjectComputer, 2);
            lblSubjectComputer.Size = new Size(70, 109);
            lblSubjectComputer.TabIndex = 20;
            lblSubjectComputer.Text = "컴퓨터 구조\r\n공학관 105";
            lblSubjectComputer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectProject
            // 
            lblSubjectProject.BackColor = Color.FromArgb(255, 242, 225);
            lblSubjectProject.Dock = DockStyle.Fill;
            lblSubjectProject.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectProject.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectProject.Location = new Point(354, 321);
            lblSubjectProject.Margin = new Padding(0);
            lblSubjectProject.Name = "lblSubjectProject";
            scheduleTable.SetRowSpan(lblSubjectProject, 2);
            lblSubjectProject.Size = new Size(70, 109);
            lblSubjectProject.TabIndex = 21;
            lblSubjectProject.Text = "팀 프로젝트\r\n프로젝트실";
            lblSubjectProject.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubjectEnglish
            // 
            lblSubjectEnglish.BackColor = Color.FromArgb(229, 247, 238);
            lblSubjectEnglish.Dock = DockStyle.Fill;
            lblSubjectEnglish.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubjectEnglish.ForeColor = Color.FromArgb(31, 42, 68);
            lblSubjectEnglish.Location = new Point(141, 431);
            lblSubjectEnglish.Margin = new Padding(0);
            lblSubjectEnglish.Name = "lblSubjectEnglish";
            scheduleTable.SetRowSpan(lblSubjectEnglish, 2);
            lblSubjectEnglish.Size = new Size(70, 109);
            lblSubjectEnglish.TabIndex = 22;
            lblSubjectEnglish.Text = "교양 영어\r\n인문관 103";
            lblSubjectEnglish.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Timetable
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(244, 246, 250);
            Controls.Add(rootLayout);
            MinimumSize = new Size(760, 450);
            Name = "Timetable";
            Size = new Size(808, 496);
            rootLayout.ResumeLayout(false);
            todayPanel.ResumeLayout(false);
            todayLayout.ResumeLayout(false);
            todayLayout.PerformLayout();
            progressPanel.ResumeLayout(false);
            progressPanel.PerformLayout();
            todayClassList.ResumeLayout(false);
            classCard1.ResumeLayout(false);
            classCard1.PerformLayout();
            classCard2.ResumeLayout(false);
            classCard2.PerformLayout();
            classCard3.ResumeLayout(false);
            classCard3.PerformLayout();
            schedulePanel.ResumeLayout(false);
            scheduleLayout.ResumeLayout(false);
            scheduleHeader.ResumeLayout(false);
            scheduleHeader.PerformLayout();
            legendPanel.ResumeLayout(false);
            legendPanel.PerformLayout();
            scheduleScrollPanel.ResumeLayout(false);
            scheduleTable.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Panel todayPanel;
        private TableLayoutPanel todayLayout;
        private Label lblTodayTitle;
        private Label lblTodayDate;
        private Panel progressPanel;
        private Label lblProgressTitle;
        private Label lblProgressCount;
        private ProgressBar todayProgressBar;
        private Label lblProgressMessage;
        private Label lblTodayClasses;
        private FlowLayoutPanel todayClassList;
        private Panel classCard1;
        private Panel classAccent1;
        private Label lblClassTime1;
        private Label lblClassName1;
        private Label lblClassRoom1;
        private Label lblClassStatus1;
        private Panel classCard2;
        private Panel classAccent2;
        private Label lblClassTime2;
        private Label lblClassName2;
        private Label lblClassRoom2;
        private Label lblClassStatus2;
        private Panel classCard3;
        private Panel classAccent3;
        private Label lblClassTime3;
        private Label lblClassName3;
        private Label lblClassRoom3;
        private Label lblClassStatus3;
        private Panel schedulePanel;
        private TableLayoutPanel scheduleLayout;
        private Panel scheduleHeader;
        private Label lblScheduleTitle;
        private Label lblSemester;
        private Button btnAddClass;
        private FlowLayoutPanel legendPanel;
        private Label legendBlue;
        private Label lblMajorLegend;
        private Label legendGreen;
        private Label lblStudyLegend;
        private Label legendOrange;
        private Label lblEtcLegend;
        private Panel scheduleScrollPanel;
        private TableLayoutPanel scheduleTable;
        private Label lblHeaderTime;
        private Label lblHeaderMonday;
        private Label lblHeaderTuesday;
        private Label lblHeaderWednesday;
        private Label lblHeaderThursday;
        private Label lblHeaderFriday;
        private Label lblTime0900;
        private Label lblTime1000;
        private Label lblTime1100;
        private Label lblTime1200;
        private Label lblTime1300;
        private Label lblTime1400;
        private Label lblTime1500;
        private Label lblTime1600;
        private Label lblTime1700;
        private Label lblSubjectDataStructure;
        private Label lblSubjectOperatingSystem;
        private Label lblSubjectCSharp;
        private Label lblSubjectWeb;
        private Label lblSubjectDatabase;
        private Label lblSubjectComputer;
        private Label lblSubjectProject;
        private Label lblSubjectEnglish;
    }
}
