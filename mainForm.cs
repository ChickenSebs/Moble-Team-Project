using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using tap;

namespace calendar4
{
    public partial class mainForm : Form
    {
        private readonly int loggedInUserId;
        private ContextMenuStrip tabAddMenu;
        private ContextMenuStrip tabContextMenu;
        private TabPage targetTab = null;
        private TextBox txtRenameEditor;
        private TabPage editingTab = null;

        private readonly HolidayService holidayService = new();
        private readonly SummaryService summaryService = new();
        private readonly TabRepository tabRepository = new();
        private AlarmManager? alarmManager;
        private DateTime currentMonth = DateTime.Now;

        private Dictionary<DateTime, string> holidayMap =
            new Dictionary<DateTime, string>();

        public enum TabType
        {
            Diary,
            Planner,
            Calendar,
            SharedCalendar,
            Timetable
        }

        public mainForm()
        {
            InitializeComponent();
            InitCalendarViewOptions();
            InitUIStyleEvents();
        }

        public mainForm(int userId) : this()
        {
            loggedInUserId = userId;
        }

        private async void mainForm_Load(object sender, EventArgs e)
        {
            tabControl1.Multiline = false;

            InitTabAddMenu();
            InitTabContextMenu();
            InitRenameEditor();

            tabControl1.Selecting += tabControl1_Selecting;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            tabControl1.DoubleClick += tabControl1_DoubleClick;
            tabControl1.MouseDown += tabControl1_MouseDown;

            this.MouseDown += Form_MouseDown_ApplyRename;

            InitSmallCalendarEvent();
            LoadTabs();

            alarmManager = new AlarmManager(tabControl1);
            alarmManager.Start();

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }

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

                ApplyHolidayMapToCalendarControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "공휴일 정보를 가져오지 못했습니다.\n\n" +
                    ex.Message,
                    "공휴일 API 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ApplyHolidayMapToCalendarControls()
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0 &&
                    tab.Controls[0] is CalendarControl calCtrl)
                {
                    calCtrl.SetHolidayMap(holidayMap);
                }
            }
        }

        private void mainForm_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            alarmManager?.Dispose();
            SaveTabs();
        }

        private void InitUIStyleEvents()
        {
            if (tool_font != null)
                tool_font.Click += tool_font_Click;

            if (tool_theme != null)
                tool_theme.Click += tool_theme_Click;

            if (tool_image != null)
                tool_image.Click += tool_image_Click;
        }

        private void tool_font_Click(
            object sender,
            EventArgs e)
        {
            using (FontDialog fontDialog =
                   new FontDialog())
            {
                if (fontDialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    ApplyFontToControls(
                        this,
                        fontDialog.Font);
                }
            }
        }

        private void ApplyFontToControls(
            Control parent,
            Font font)
        {
            UiThemeService.ApplyFont(
                parent,
                font);
        }

        private void tool_theme_Click(
            object sender,
            EventArgs e)
        {
            using (ColorDialog colorDialog =
                   new ColorDialog())
            {
                if (colorDialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    this.BackColor =
                        colorDialog.Color;

                    tabControl1.BackColor =
                        colorDialog.Color;
                }
            }
        }

        private void tool_image_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog ofd =
                   new OpenFileDialog())
            {
                ofd.Filter =
                    "이미지 파일|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() ==
                    DialogResult.OK)
                {
                    this.BackgroundImage =
                        Image.FromFile(
                            ofd.FileName);

                    this.BackgroundImageLayout =
                        ImageLayout.Stretch;
                }
            }
        }

        private void InitCalendarViewOptions()
        {
            if (tool_month != null)
            {
                tool_month.Click += (_, _) =>
                    ChangeCalendarView(
                        CalendarControl.CalendarViewMode.Month);
            }

            if (tool_week != null)
            {
                tool_week.Click += (_, _) =>
                    ChangeCalendarView(
                        CalendarControl.CalendarViewMode.Week);
            }

            if (tool_day != null)
            {
                tool_day.Click += (_, _) =>
                    ChangeCalendarView(
                        CalendarControl.CalendarViewMode.Day);
            }

            UpdateCalendarViewMenu();
        }

        private void ChangeCalendarView(
            CalendarControl.CalendarViewMode viewMode)
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count == 0)
                    continue;

                if (tab.Controls[0] is CalendarControl calCtrl)
                {
                    calCtrl.SetViewMode(viewMode);
                }
                else if (tab.Controls[0] is DiaryControl diaryCtrl)
                {
                    diaryCtrl.SetViewMode(viewMode);
                }
            }

            UpdateCalendarViewMenu();
            UpdateCalendarTitle();
        }

        private void UpdateCalendarViewMenu()
        {
            CalendarControl.CalendarViewMode currentMode =
                GetSelectedViewMode();

            if (tool_month != null)
            {
                tool_month.Checked =
                    currentMode ==
                    CalendarControl.CalendarViewMode.Month;
            }

            if (tool_week != null)
            {
                tool_week.Checked =
                    currentMode ==
                    CalendarControl.CalendarViewMode.Week;
            }

            if (tool_day != null)
            {
                tool_day.Checked =
                    currentMode ==
                    CalendarControl.CalendarViewMode.Day;
            }
        }

        private CalendarControl.CalendarViewMode
            GetSelectedViewMode()
        {
            if (tabControl1.SelectedTab?.Controls.Count > 0)
            {
                Control selectedControl =
                    tabControl1.SelectedTab.Controls[0];

                if (selectedControl is CalendarControl calendarControl)
                {
                    return calendarControl.GetViewMode();
                }

                if (selectedControl is DiaryControl diaryControl)
                {
                    return diaryControl.GetViewMode();
                }
            }

            return CalendarControl.CalendarViewMode.Month;
        }

        private void InitSmallCalendarEvent()
        {
            if (this.Controls.Find(
                    "monthCalendar1",
                    true).Length > 0)
            {
                MonthCalendar miniCal =
                    this.Controls.Find(
                        "monthCalendar1",
                        true)[0] as MonthCalendar;

                if (miniCal != null)
                {
                    miniCal.DateChanged +=
                        MiniCal_DateChanged;
                }
            }
        }


        // ============================================================
        // ★ 추가 ①
        // 윈폼 달력 날짜를 눌렀을 때
        // 현재 탭이 Planner이면 해당 날짜의 플래너 표시
        // ============================================================

        private async void MiniCal_DateChanged(
            object sender,
            DateRangeEventArgs e)
        {
            ApplyTabRename();

            currentMonth =
                e.Start.Date;

            // ★ 추가 시작
            // 현재 선택된 탭이 스터디 플래너인지 확인
            if (tabControl1.SelectedTab != null &&
                tabControl1.SelectedTab.Controls.Count > 0 &&
                tabControl1.SelectedTab.Controls[0]
                    is PlannerControl plannerControl)
            {
                plannerControl.SetDate(
                    currentMonth);
            }
            // ★ 추가 끝

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }


        private void SyncSmallCalendar()
        {
            if (this.Controls.Find(
                    "monthCalendar1",
                    true).Length > 0)
            {
                MonthCalendar miniCal =
                    this.Controls.Find(
                        "monthCalendar1",
                        true)[0] as MonthCalendar;

                if (miniCal != null)
                {
                    miniCal.DateChanged -=
                        MiniCal_DateChanged;

                    miniCal.SetDate(
                        currentMonth);

                    miniCal.DateChanged +=
                        MiniCal_DateChanged;
                }
            }
        }


        // ============================================================
        // ★ 추가 ②
        // 스터디 플래너 탭으로 들어왔을 때
        // 현재 윈폼 달력 날짜를 플래너에 적용
        // ============================================================

        private void tabControl1_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            // ★ 추가 시작
            if (tabControl1.SelectedTab != null &&
                tabControl1.SelectedTab.Controls.Count > 0 &&
                tabControl1.SelectedTab.Controls[0]
                    is PlannerControl plannerControl)
            {
                plannerControl.SetDate(
                    currentMonth);
            }
            // ★ 추가 끝

            UpdateSummaryView();
            UpdateCalendarViewMenu();
            UpdateCalendarTitle();
        }


        private void RefreshAllViews()
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count == 0)
                    continue;

                if (tab.Controls[0] is DiaryControl diaryCtrl)
                {
                    diaryCtrl.SetTargetDate(
                        currentMonth);
                }
                else if (tab.Controls[0] is CalendarControl calCtrl)
                {
                    calCtrl.SetHolidayMap(
                        holidayMap);

                    calCtrl.SetTargetDate(
                        currentMonth);
                }
            }

            UpdateSummaryView();
            UpdateCalendarTitle();
        }

        private void UpdateSummaryView()
        {
            if (tabControl1.SelectedTab is null)
                return;

            var summaryBoxes =
                Controls.Find(
                    "richTextBox1",
                    true);

            if (summaryBoxes.Length == 0 ||
                summaryBoxes[0] is not RichTextBox summaryBox)
                return;

            if (tabControl1.SelectedTab.Controls.Count > 0 &&
                tabControl1.SelectedTab.Controls[0]
                    is DiaryControl diaryControl)
            {
                summaryBox.Text =
                    summaryService.CreateDiarySummary(
                        diaryControl.GetDiaryMap());

                return;
            }

            if (tabControl1.SelectedTab.Controls.Count > 0 &&
                tabControl1.SelectedTab.Controls[0]
                    is CalendarControl currentCalControl)
            {
                summaryBox.Text =
                    summaryService.CreateCalendarSummary(
                        currentCalControl.GetScheduleMap());

                return;
            }

            CalendarControl? calendarControl = null;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0 &&
                    tab.Controls[0] is CalendarControl cal)
                {
                    calendarControl = cal;
                    break;
                }
            }

            if (calendarControl != null)
            {
                summaryBox.Text =
                    summaryService.CreateCalendarSummary(
                        calendarControl.GetScheduleMap());
            }
            else
            {
                summaryBox.Text =
                    "📋 [전체 일정 요약]\n\n" +
                    "열려있는 캘린더가 없습니다.";
            }
        }

        private void UpdateCalendarTitle()
        {
            if (lbmain_title is null)
                return;

            lbmain_title.Text =
                CalendarTitleFormatter.Format(
                    currentMonth,
                    GetSelectedViewMode());
        }

        private async void btnPrev_Click(
            object sender,
            EventArgs e)
        {
            ApplyTabRename();

            CalendarControl.CalendarViewMode mode =
                GetSelectedViewMode();

            currentMonth =
                mode switch
                {
                    CalendarControl.CalendarViewMode.Week
                        => currentMonth.AddDays(-7),

                    CalendarControl.CalendarViewMode.Day
                        => currentMonth.AddDays(-1),

                    _
                        => currentMonth.AddMonths(-1)
                };

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }

        private async void btnNext_Click(
            object sender,
            EventArgs e)
        {
            ApplyTabRename();

            CalendarControl.CalendarViewMode mode =
                GetSelectedViewMode();

            currentMonth =
                mode switch
                {
                    CalendarControl.CalendarViewMode.Week
                        => currentMonth.AddDays(7),

                    CalendarControl.CalendarViewMode.Day
                        => currentMonth.AddDays(1),

                    _
                        => currentMonth.AddMonths(1)
                };

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }

        private void InitTabAddMenu()
        {
            tabAddMenu =
                new ContextMenuStrip();

            tabAddMenu.Items.Add(
                "다이어리",
                null,
                (s, ev) =>
                    AddNewCustomTab(
                        "다이어리",
                        TabType.Diary));

            tabAddMenu.Items.Add(
                "스터디 플래너",
                null,
                (s, ev) =>
                    AddNewCustomTab(
                        "스터디 플래너",
                        TabType.Planner));

            tabAddMenu.Items.Add(
                "시간표",
                null,
                (s, ev) =>
                    AddNewCustomTab(
                        "시간표",
                        TabType.Timetable));

            tabAddMenu.Items.Add(
                "개인 캘린더",
                null,
                (s, ev) =>
                    AddNewCustomTab(
                        "개인 캘린더",
                        TabType.Calendar));

            tabAddMenu.Items.Add(
                "공유 캘린더",
                null,
                (s, ev) =>
                    AddNewCustomTab(
                        "공유 캘린더",
                        TabType.SharedCalendar));
        }

        private void tabControl1_Selecting(
            object sender,
            TabControlCancelEventArgs e)
        {
            ApplyTabRename();

            if (e.TabPage != null &&
                e.TabPage.Text == "+")
            {
                e.Cancel = true;

                tabAddMenu?.Show(
                    Cursor.Position);
            }
        }

        private void AddNewCustomTab(
            string title,
            TabType type)
        {
            int plusIndex =
                tabControl1.TabPages.Count - 1;

            if (plusIndex >= 0 &&
                tabControl1.TabPages[plusIndex].Text == "+")
            {
                tabControl1.TabPages.RemoveAt(
                    plusIndex);
            }

            TabPage newTab =
                CreateTabPage(
                    title,
                    type);

            tabControl1.TabPages.Add(
                new TabPage("+"));

            tabControl1.SelectedTab =
                newTab;
        }

        private void InitTabContextMenu()
        {
            tabContextMenu =
                new ContextMenuStrip();

            tabContextMenu.Items.Add(
                "이름 변경",
                null,
                (s, ev) =>
                {
                    if (targetTab != null)
                    {
                        StartInlineRename(
                            targetTab);
                    }
                });

            tabContextMenu.Items.Add(
                "탭 삭제",
                null,
                DeleteItem_Click);
        }

        private void tabControl1_MouseDown(
            object sender,
            MouseEventArgs e)
        {
            ApplyTabRename();

            if (e.Button ==
                MouseButtons.Right)
            {
                for (int i = 0;
                     i < tabControl1.TabPages.Count;
                     i++)
                {
                    if (tabControl1
                        .GetTabRect(i)
                        .Contains(e.Location))
                    {
                        TabPage clickedTab =
                            tabControl1.TabPages[i];

                        if (clickedTab.Text != "+")
                        {
                            targetTab =
                                clickedTab;

                            tabContextMenu?.Show(
                                tabControl1,
                                e.Location);
                        }

                        break;
                    }
                }
            }
        }

        private void DeleteItem_Click(
            object sender,
            EventArgs e)
        {
            if (targetTab == null)
                return;

            if (tabControl1.TabPages.Count - 1 <= 1)
            {
                MessageBox.Show(
                    "최소 하나의 탭은 유지해야 합니다.",
                    "알림",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (MessageBox.Show(
                    $"[{targetTab.Text}] 탭을 정말 삭제하시겠습니까?",
                    "탭 삭제 확인",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                tabControl1.TabPages.Remove(
                    targetTab);

                targetTab = null;
            }
        }

        private void InitRenameEditor()
        {
            txtRenameEditor =
                new TextBox
                {
                    Visible = false,
                    BorderStyle =
                        BorderStyle.FixedSingle
                };

            txtRenameEditor.KeyDown +=
                TxtRenameEditor_KeyDown;

            txtRenameEditor.Leave +=
                (s, ev) =>
                    ApplyTabRename();

            this.Controls.Add(
                txtRenameEditor);
        }

        private void tabControl1_DoubleClick(
            object sender,
            EventArgs e)
        {
            Point clientPoint =
                tabControl1.PointToClient(
                    Cursor.Position);

            for (int i = 0;
                 i < tabControl1.TabPages.Count;
                 i++)
            {
                if (tabControl1
                    .GetTabRect(i)
                    .Contains(clientPoint) &&
                    tabControl1.TabPages[i].Text != "+")
                {
                    StartInlineRename(
                        tabControl1.TabPages[i]);

                    break;
                }
            }
        }

        private void StartInlineRename(
            TabPage tab)
        {
            if (txtRenameEditor == null)
                return;

            editingTab = tab;

            Rectangle tabRect =
                tabControl1.GetTabRect(
                    tabControl1.TabPages.IndexOf(
                        tab));

            Point formPoint =
                this.PointToClient(
                    tabControl1.PointToScreen(
                        tabRect.Location));

            txtRenameEditor.Bounds =
                new Rectangle(
                    formPoint.X + 4,
                    formPoint.Y + 3,
                    tabRect.Width - 8,
                    tabRect.Height - 6);

            txtRenameEditor.Text =
                editingTab.Text;

            txtRenameEditor.Visible =
                true;

            txtRenameEditor.BringToFront();

            txtRenameEditor.Focus();

            txtRenameEditor.SelectAll();
        }

        private void TxtRenameEditor_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyTabRename();

                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtRenameEditor.Visible =
                    false;

                editingTab = null;
            }
        }

        private void Form_MouseDown_ApplyRename(
            object sender,
            MouseEventArgs e)
        {
            ApplyTabRename();
        }

        private void ApplyTabRename()
        {
            if (editingTab != null &&
                txtRenameEditor != null &&
                txtRenameEditor.Visible)
            {
                if (!string.IsNullOrWhiteSpace(
                        txtRenameEditor.Text))
                {
                    editingTab.Text =
                        txtRenameEditor.Text;
                }

                txtRenameEditor.Visible =
                    false;

                editingTab = null;
            }
        }

        private TabPage CreateTabPage(
            string title,
            TabType type)
        {
            TabPage newTab =
                new TabPage(title)
                {
                    Tag = type
                };

            Control content;

            switch (type)
            {
                case TabType.Diary:

                    var diaryCtrl =
                        new DiaryControl
                        {
                            Dock =
                                DockStyle.Fill
                        };

                    diaryCtrl.DataChanged +=
                        (s, ev) =>
                            UpdateSummaryView();

                    content = diaryCtrl;

                    break;


                case TabType.Planner:

                    content =
                        new PlannerControl
                        {
                            Dock =
                                DockStyle.Fill
                        };

                    break;


                case TabType.Timetable:

                    content =
                        new Timetable(
                            loggedInUserId)
                        {
                            Dock =
                                DockStyle.Fill
                        };

                    break;


                case TabType.SharedCalendar:

                    content =
                        new Label
                        {
                            Text =
                                $"{title} (공유 캘린더 화면)",

                            Dock =
                                DockStyle.Fill,

                            TextAlign =
                                ContentAlignment.MiddleCenter
                        };

                    break;


                case TabType.Calendar:

                default:

                    var calCtrl =
                        new CalendarControl(
                            loggedInUserId)
                        {
                            Dock =
                                DockStyle.Fill
                        };

                    calCtrl.SetHolidayMap(
                        holidayMap);

                    calCtrl.DateOrScheduleChanged +=
                        (s, ev) =>
                        {
                            currentMonth =
                                calCtrl.GetTargetDate();

                            SyncSmallCalendar();

                            RefreshAllViews();
                        };

                    content =
                        calCtrl;

                    break;
            }

            newTab.Controls.Add(
                content);

            tabControl1.TabPages.Add(
                newTab);

            return newTab;
        }

        private void SaveTabs()
        {
            var tabs =
                tabControl1.TabPages
                    .Cast<TabPage>()
                    .Where(tab =>
                        tab.Text != "+")
                    .Select(tab =>
                        new TabData
                        {
                            Title =
                                tab.Text,

                            Type =
                                tab.Tag is TabType type
                                    ? type
                                    : TabType.Calendar
                        })
                    .ToList();

            tabRepository.Save(
                tabs);
        }

        private void LoadTabs()
        {
            var tabs =
                tabRepository.Load();

            if (tabs.Count == 0)
            {
                SetupDefaultFirstTab();
            }
            else
            {
                tabControl1.TabPages.Clear();

                foreach (var tab in tabs)
                {
                    CreateTabPage(
                        tab.Title,
                        tab.Type);
                }
            }

            tabControl1.TabPages.Add(
                new TabPage("+"));
        }

        private void SetupDefaultFirstTab()
        {
            tabControl1.TabPages.Clear();

            CreateTabPage(
                "개인 캘린더",
                TabType.Calendar);
        }

        private void btnMy_Click(
            object sender,
            EventArgs e)
        {
            this.Close();

            Mypage mypage =
                new Mypage(
                    this.loggedInUserId);

            mypage.ShowDialog();
        }

        private void btn_exit_Click(
            object sender,
            EventArgs e)
        {
            this.Close();

            Login login =
                new Login();

            login.ShowDialog();
        }
    }
}