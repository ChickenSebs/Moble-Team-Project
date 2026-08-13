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
using MySql.Data.MySqlClient;
using calendar4.Services;

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


        // ============================================================
        // 생성자
        // ============================================================

        public mainForm()
        {
            InitializeComponent();
            InitCalendarViewOptions();
        }

        public mainForm(int userId) : this()
        {
            loggedInUserId = userId;
        }


        // ============================================================
        // 테마 선택
        // ============================================================

        private void ApplySelectedTheme(AppTheme theme)
        {
            // 프리미엄 전용 테마 검사
            if (UiThemeService.IsPremiumTheme(theme) &&
                !CurrentUser.IsPremium)
            {
                MessageBox.Show(
                    "프리미엄 사용자만 사용할 수 있는 테마입니다.",
                    "Premium",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 현재 프로그램 테마 변경
            UiThemeService.SetTheme(theme);

            // 메인폼에 테마 적용
            UiThemeService.ApplyTheme(this);

            // 열려있는 모든 탭에도 테마 적용
            ApplyThemeToAllTabs();

            // 선택한 테마를 DB에 저장
            SaveUserTheme(theme);
        }


        // ============================================================
        // 열려있는 모든 탭에 현재 테마 적용
        // ============================================================

        private void ApplyThemeToAllTabs()
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                // + 탭 등 컨트롤이 없는 경우
                if (tab.Controls.Count == 0)
                    continue;

                foreach (Control control in tab.Controls)
                {
                    if (control is CalendarControl calendarControl)
                    {
                        calendarControl.ApplyCurrentTheme();
                    }
                    else if (control is DiaryControl diaryControl)
                    {
                        diaryControl.ApplyCurrentTheme();
                    }
                    else if (control is PlannerControl plannerControl)
                    {
                        plannerControl.ApplyCurrentTheme();
                    }
                    else if (control is Timetable timetable)
                    {
                        timetable.ApplyCurrentTheme();
                    }
                    else
                    {
                        // 일반 컨트롤
                        UiThemeService.ApplyTheme(control);
                    }
                }
            }
        }


        // ============================================================
        // AppTheme → DB 번호
        //
        // 0 = Light
        // 1 = Dark
        // 2 = Blossom
        // 3 = Mint
        // 4 = Lavender
        // 5 = Cozy
        // ============================================================

        private int GetThemeNumber(AppTheme theme)
        {
            return theme switch
            {
                AppTheme.Dark => 1,
                AppTheme.Blossom => 2,
                AppTheme.Mint => 3,
                AppTheme.Lavender => 4,
                AppTheme.Cozy => 5,

                _ => 0
            };
        }


        // ============================================================
        // 사용자 테마 DB 저장
        // ============================================================

        private void SaveUserTheme(AppTheme theme)
        {
            try
            {
                using var connection =
                    new DBConnection().GetConnection();

                connection.Open();

                const string sql = @"
                    UPDATE user
                    SET theme = @theme
                    WHERE user_id = @user_id";

                using var command =
                    new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@theme",
                    GetThemeNumber(theme));

                command.Parameters.AddWithValue(
                    "@user_id",
                    loggedInUserId);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"테마 저장 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // 사용자 테마 DB 불러오기
        // ============================================================

        private AppTheme LoadUserTheme()
        {
            try
            {
                using var connection =
                    new DBConnection().GetConnection();

                connection.Open();

                const string sql = @"
                    SELECT theme
                    FROM user
                    WHERE user_id = @user_id";

                using var command =
                    new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@user_id",
                    loggedInUserId);

                object? result =
                    command.ExecuteScalar();

                // DB 값이 없으면 기본 Light
                if (result == null ||
                    result == DBNull.Value)
                {
                    return AppTheme.Light;
                }

                int themeValue =
                    Convert.ToInt32(result);

                return themeValue switch
                {
                    1 => AppTheme.Dark,
                    2 => AppTheme.Blossom,
                    3 => AppTheme.Mint,
                    4 => AppTheme.Lavender,
                    5 => AppTheme.Cozy,

                    _ => AppTheme.Light
                };
            }
            catch
            {
                // DB 문제가 있어도 프로그램은 실행되도록
                return AppTheme.Light;
            }
        }


        // ============================================================
        // Form Load
        // ============================================================

        private async void mainForm_Load(
            object sender,
            EventArgs e)
        {
            tabControl1.Multiline = false;

            InitTabAddMenu();
            InitTabContextMenu();
            InitRenameEditor();

            tabControl1.Selecting +=
                tabControl1_Selecting;

            tabControl1.SelectedIndexChanged +=
                tabControl1_SelectedIndexChanged;

            tabControl1.DoubleClick +=
                tabControl1_DoubleClick;

            tabControl1.MouseDown +=
                tabControl1_MouseDown;

            this.MouseDown +=
                Form_MouseDown_ApplyRename;

            InitSmallCalendarEvent();

            // 저장된 탭 생성
            LoadTabs();
            LoadSavedDday();


            // ========================================================
            // 로그인한 사용자의 저장된 테마 불러오기
            // ========================================================

            AppTheme savedTheme =
                LoadUserTheme();

            UiThemeService.SetTheme(
                savedTheme);

            // 메인폼 적용
            UiThemeService.ApplyTheme(
                this);

            // LoadTabs()에서 이미 생성된 탭에도 적용
            ApplyThemeToAllTabs();


            // ========================================================
            // 알람 시작
            // ========================================================

            alarmManager =
                new AlarmManager(tabControl1);

            alarmManager.Start();

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }


        // ============================================================
        // 공휴일
        // ============================================================

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
                    calCtrl.SetHolidayMap(
                        holidayMap);
                }
            }
        }


        // ============================================================
        // Form Closing
        // ============================================================

        private void mainForm_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            alarmManager?.Dispose();

            SaveTabs();

            Application.Exit();
        }


        // ============================================================
        // 달력 보기 옵션
        // ============================================================

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
                    calCtrl.SetViewMode(
                        viewMode);
                }
                else if (tab.Controls[0] is DiaryControl diaryCtrl)
                {
                    diaryCtrl.SetViewMode(
                        viewMode);
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

                if (selectedControl
                    is CalendarControl calendarControl)
                {
                    return calendarControl.GetViewMode();
                }

                if (selectedControl
                    is DiaryControl diaryControl)
                {
                    return diaryControl.GetViewMode();
                }
            }

            return CalendarControl.CalendarViewMode.Month;
        }


        // ============================================================
        // 작은 달력
        // ============================================================

        private void InitSmallCalendarEvent()
        {
            if (this.Controls.Find(
                "monthCalendar1",
                true).Length > 0)
            {
                MonthCalendar miniCal =
                    this.Controls.Find(
                        "monthCalendar1",
                        true)[0]
                    as MonthCalendar;

                if (miniCal != null)
                {
                    miniCal.DateChanged +=
                        MiniCal_DateChanged;
                }
            }
        }

        private async void MiniCal_DateChanged(
            object sender,
            DateRangeEventArgs e)
        {
            ApplyTabRename();

            currentMonth =
                e.Start.Date;

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
                        true)[0]
                    as MonthCalendar;

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
        // 탭 변경
        // ============================================================

        private void tabControl1_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            UpdateSummaryView();
            UpdateCalendarViewMenu();
            UpdateCalendarTitle();
        }


        // ============================================================
        // 전체 View 날짜 갱신
        // ============================================================

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
                else if (tab.Controls[0] is PlannerControl plannerCtrl)
                {
                    plannerCtrl.SetDate(
                        currentMonth);
                }
            }

            UpdateSummaryView();
            UpdateCalendarTitle();
        }


        // ============================================================
        // 일정 요약
        // ============================================================

        private void UpdateSummaryView()
        {
            if (tabControl1.SelectedTab is null)
                return;

            var summaryBoxes =
                Controls.Find(
                    "richTextBox1",
                    true);

            if (summaryBoxes.Length == 0 ||
                summaryBoxes[0]
                is not RichTextBox summaryBox)
            {
                return;
            }

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

            CalendarControl? calendarControl =
                null;

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


        // ============================================================
        // 상단 날짜 제목
        // ============================================================

        private void UpdateCalendarTitle()
        {
            if (lbmain_title is null)
                return;

            lbmain_title.Text =
                CalendarTitleFormatter.Format(
                    currentMonth,
                    GetSelectedViewMode());
        }


        // ============================================================
        // 이전
        // ============================================================

        private async void btnPrev_Click(
            object sender,
            EventArgs e)
        {
            ApplyTabRename();

            CalendarControl.CalendarViewMode mode =
                GetSelectedViewMode();

            currentMonth = mode switch
            {
                CalendarControl.CalendarViewMode.Week =>
                    currentMonth.AddDays(-7),

                CalendarControl.CalendarViewMode.Day =>
                    currentMonth.AddDays(-1),

                _ =>
                    currentMonth.AddMonths(-1)
            };

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }


        // ============================================================
        // 다음
        // ============================================================

        private async void btnNext_Click(
            object sender,
            EventArgs e)
        {
            ApplyTabRename();

            CalendarControl.CalendarViewMode mode =
                GetSelectedViewMode();

            currentMonth = mode switch
            {
                CalendarControl.CalendarViewMode.Week =>
                    currentMonth.AddDays(7),

                CalendarControl.CalendarViewMode.Day =>
                    currentMonth.AddDays(1),

                _ =>
                    currentMonth.AddMonths(1)
            };

            SyncSmallCalendar();

            await LoadHolidaysAsync(
                currentMonth.Year,
                currentMonth.Month);

            RefreshAllViews();
        }


        // ============================================================
        // + 탭 메뉴
        // ============================================================

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


        // ============================================================
        // 탭 우클릭
        // ============================================================

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
                    if (tabControl1.GetTabRect(i)
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

                targetTab =
                    null;
            }
        }


        // ============================================================
        // 탭 이름 변경
        // ============================================================

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
                if (tabControl1.GetTabRect(i)
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

            editingTab =
                tab;

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
            if (e.KeyCode ==
                Keys.Enter)
            {
                ApplyTabRename();

                e.SuppressKeyPress =
                    true;
            }
            else if (e.KeyCode ==
                     Keys.Escape)
            {
                txtRenameEditor.Visible =
                    false;

                editingTab =
                    null;
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

                editingTab =
                    null;
            }
        }


        // ============================================================
        // 실제 탭 생성
        // ============================================================

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
                        new DiaryControl(
                            loggedInUserId)
                        {
                            Dock =
                                DockStyle.Fill
                        };

                    diaryCtrl.DataChanged +=
                        (s, ev) =>
                            UpdateSummaryView();

                    diaryCtrl.DateOrScheduleChanged +=
                        (s, ev) =>
                        {
                            currentMonth =
                                diaryCtrl.GetTargetDate();

                            SyncSmallCalendar();

                            RefreshAllViews();
                        };

                    content =
                        diaryCtrl;

                    break;


                case TabType.Planner:

                    content =
                        new PlannerControl(
                            loggedInUserId)
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


            // ========================================================
            // 새로 만든 탭에 현재 선택된 테마 적용
            // ========================================================

            UiThemeService.ApplyTheme(
                newTab);

            if (content is CalendarControl calendarControl)
            {
                calendarControl.ApplyCurrentTheme();
            }
            else if (content is DiaryControl diaryControl)
            {
                diaryControl.ApplyCurrentTheme();
            }
            else if (content is PlannerControl plannerControl)
            {
                plannerControl.ApplyCurrentTheme();
            }
            else if (content is Timetable timetable)
            {
                timetable.ApplyCurrentTheme();
            }


            return newTab;
        }


        // ============================================================
        // 탭 저장
        // ============================================================

        private void SaveTabs()
        {
            var tabs =
                tabControl1.TabPages
                    .Cast<TabPage>()
                    .Where(
                        tab => tab.Text != "+")
                    .Select(
                        tab => new TabData
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


        // ============================================================
        // 탭 불러오기
        // ============================================================

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

        private void btnMy_Click(object sender, EventArgs e)
        {
            this.Hide();
            Mypage mypage = new Mypage(loggedInUserId);

            DialogResult result = mypage.ShowDialog();

            // 회원탈퇴한 경우
            if (result == DialogResult.Abort)
            {
                Login login = new Login();
                login.Show();

                // 절대로 this.Show() 하지 않음
                return;
            }
            this.Show();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
        }


        private void menuThemeLight_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Light);
        }

        private void menuThemeDark_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Dark);
        }

        private void menuThemeBlossom_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Blossom);
        }

        private void menuThemeMint_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Mint);
        }

        private void menuThemeLavender_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Lavender);
        }

        private void menuThemeCozy_Click(
            object sender,
            EventArgs e)
        {
            ApplySelectedTheme(
                AppTheme.Cozy);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            SearchActiveTab();
        }

        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SearchActiveTab();
            }
        }
        private void SearchActiveTab()
        {
            string keyword = tb_search.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show(
                    "검색할 내용을 입력해주세요.",
                    "검색",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                tb_search.Focus();
                return;
            }

            if (tabControl1.SelectedTab?.Controls.Count is not > 0)
                return;

            Control activeControl =
                tabControl1.SelectedTab.Controls[0];

            List<SearchResultItem> results;
            string searchScope;

            if (activeControl is CalendarControl calendarControl)
            {
                searchScope = "개인 캘린더";

                results =
                    SearchCalendar(
                        calendarControl,
                        keyword);
            }
            else if (activeControl is DiaryControl diaryControl)
            {
                searchScope = "다이어리";

                results =
                    SearchDiary(
                        diaryControl,
                        keyword);
            }
            else
            {
                MessageBox.Show(
                    "개인 캘린더 또는 다이어리 탭에서 검색해주세요.",
                    "검색할 수 없는 탭",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (results.Count == 0)
            {
                MessageBox.Show(
                    $"{searchScope}에서 ‘{keyword}’와 관련된 내용을 찾지 못했습니다.",
                    "검색 결과 없음",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using var dialog =
                new SearchResultsDialog(
                    searchScope,
                    keyword,
                    results);

            if (dialog.ShowDialog(this) != DialogResult.OK ||
                dialog.SelectedResult is null)
            {
                return;
            }

            MoveToSearchResult(
                dialog.SelectedResult.Date);
        }
        private static List<SearchResultItem> SearchCalendar(CalendarControl calendarControl, string keyword)
        {
            return calendarControl
                .GetScheduleMap()
                .SelectMany(
                    pair =>
                        pair.Value.Select(
                            schedule => new
                            {
                                Date = pair.Key.Date,
                                Schedule = schedule
                            }))
                .Where(
                    item =>
                        item.Schedule.Text.Contains(
                            keyword,
                            StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.Date)
                .ThenBy(item => item.Schedule.StartHour)
                .Select(
                    item =>
                        new SearchResultItem(
                            item.Date,
                            item.Schedule.Text,
                            $"{item.Date:yyyy년 M월 d일}  " +
                            $"{item.Schedule.StartHour:00}:00~{item.Schedule.EndHour:00}:00"))
                .ToList();
        }
        private static List<SearchResultItem> SearchDiary(DiaryControl diaryControl, string keyword)
        {
            return diaryControl
                .GetDiaryMap()
                .Select(
                    pair => new
                    {
                        Date =
                            DateTime.TryParse(
                                pair.Key,
                                out DateTime date)
                                ? date.Date
                                : DateTime.MinValue,

                        Diary = pair.Value
                    })
                .Where(
                    item =>
                        item.Date != DateTime.MinValue &&
                        (
                            item.Diary.Title.Contains(
                                keyword,
                                StringComparison.OrdinalIgnoreCase)
                            ||
                            item.Diary.Content.Contains(
                                keyword,
                                StringComparison.OrdinalIgnoreCase)
                        ))
                .OrderBy(item => item.Date)
                .Select(
                    item =>
                        new SearchResultItem(
                            item.Date,

                            string.IsNullOrWhiteSpace(
                                item.Diary.Title)
                                ? "[제목 없음]"
                                : item.Diary.Title,

                            $"{item.Date:yyyy년 M월 d일}  " +
                            CreateSearchPreview(
                                item.Diary.Content)))
                .ToList();
        }
        private static string CreateSearchPreview(string content)
        {
            string preview =
                content
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Trim();

            return preview.Length > 60
                ? preview[..60] + "…"
                : preview;
        }
        private void MoveToSearchResult(DateTime date)
        {
            if (tabControl1.SelectedTab?.Controls.Count is not > 0)
                return;

            currentMonth =
                date.Date;

            SyncSmallCalendar();

            Control activeControl =
                tabControl1.SelectedTab.Controls[0];

            if (activeControl is CalendarControl calendarControl)
            {
                calendarControl.SetTargetDate(
                    currentMonth);

                calendarControl.SetViewMode(
                    CalendarControl.CalendarViewMode.Day);
            }
            else if (activeControl is DiaryControl diaryControl)
            {
                diaryControl.SetTargetDate(
                    currentMonth);

                diaryControl.SetViewMode(
                    CalendarControl.CalendarViewMode.Day);
            }

            UpdateCalendarViewMenu();
            UpdateCalendarTitle();
            UpdateSummaryView();
        }

        private void LoadSavedDday()
        {
            try
            {
                using var connection =
                    new DBConnection().GetConnection();

                connection.Open();

                const string sql = @"
            SELECT date, content, type
            FROM user_dday
            WHERE user_id = @user_id
            ORDER BY dday_id DESC
            LIMIT 1";

                using var command =
                    new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@user_id",
                    loggedInUserId);

                using var reader =
                    command.ExecuteReader();

                // 저장된 D-Day가 하나도 없는 경우
                if (!reader.Read())
                {
                    lbDday.Text = "[ D-Day 없음 ]";
                    return;
                }

                DateTime targetDate =
                    reader.GetDateTime("date");

                string title =
                    reader.GetString("content");

                int type =
                    reader.GetInt32("type");

                bool startFromOne =
                    type == 1;


                // 오늘 날짜와 D-Day 날짜 차이 계산
                int dayDifference =
                    (DateTime.Today - targetDate.Date).Days;

                int dday;

                if (startFromOne)
                {
                    if (dayDifference >= 0)
                        dday = dayDifference + 1;
                    else
                        dday = dayDifference;
                }
                else
                {
                    dday = dayDifference;
                }


                // 화면에 표시
                if (dday > 0)
                {
                    lbDday.Text =
                        $"D+{dday} | {title}";
                }
                else if (dday < 0)
                {
                    lbDday.Text =
                        $"D{dday} | {title}";
                }
                else
                {
                    lbDday.Text =
                        $"D-Day | {title}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"D-Day 정보를 불러오는 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btn_Dday_Click(object sender,EventArgs e)
        {
            using var form =
                new DdaySettingForm(
                    loggedInUserId);

            if (form.ShowDialog(this)
                != DialogResult.OK)
            {
                return;
            }

            DateTime targetDate =
                form.SelectedDate;

            int dayDifference =
                (DateTime.Today - targetDate).Days;

            int dday;

            if (form.StartFromOne)
            {
                if (dayDifference >= 0)
                    dday = dayDifference + 1;
                else
                    dday = dayDifference;
            }
            else
            {
                dday = dayDifference;
            }

            if (dday > 0)
            {
                lbDday.Text =
                    $"D+{dday} | {form.SelectedTitle}";
            }
            else if (dday < 0)
            {
                lbDday.Text =
                    $"D{dday} | {form.SelectedTitle}";
            }
            else
            {
                lbDday.Text =
                    $"D-Day | {form.SelectedTitle}";
            }

            // D-Day 추가 후 개인달력 새로고침
            foreach (TabPage tab
                in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0 &&
                    tab.Controls[0]
                    is CalendarControl calCtrl)
                {
                    calCtrl.LoadSchedules();
                    calCtrl.UpdateView();
                }
            }

            UpdateSummaryView();
        }
    }
}