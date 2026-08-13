using calendar4.Services;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calendar4
{
    public partial class DdaySettingForm : Form
    {
        private readonly CalendarDbRepository calendarDbRepository = new();

        private readonly int loggedInUserId;
        public DateTime SelectedDate { get; private set; }
        public string SelectedTitle { get; private set; }
        public bool StartFromOne { get; private set; }
        public DdaySettingForm(int userId)
        {
            InitializeComponent();

            loggedInUserId = userId;

            LoadSchedules();
        }
        private void LoadSchedules()
        {
            var scheduleMap =
                calendarDbRepository.Load(loggedInUserId);

            lstSchedules.Items.Clear();

            foreach (var date in scheduleMap.Keys.OrderBy(x => x))
            {
                foreach (var schedule in scheduleMap[date])
                {
                    lstSchedules.Items.Add(
                        $"{date:yyyy-MM-dd}   {schedule.Text}");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstSchedules.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "D-Day로 설정할 일정을 선택해주세요.",
                    "D-Day 설정",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string selectedSchedule =
                lstSchedules.SelectedItem.ToString();

            string dateText =
                selectedSchedule.Substring(0, 10);

            SelectedDate =
                DateTime.Parse(dateText);

            SelectedTitle =
                selectedSchedule.Substring(13);

            StartFromOne =
                rdoOne.Checked;

            // ★ DB에도 D-day 저장
            SaveDdayToDatabase();

            DialogResult =
                DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void lstSchedules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSchedules.SelectedIndex == -1)
            {
                lblSelected.Text = "선택된 일정이 없습니다.";
                return;
            }

            lblSelected.Text =
                $"선택된 일정: {lstSchedules.SelectedItem}";
        }

        private void btnNewDday_Click(object sender, EventArgs e)
        {
            using (var form = new NewDdayForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    SelectedDate = form.SelectedDate;
                    SelectedTitle = form.SelectedTitle;
                    StartFromOne = form.StartFromOne;

                    // ---------------------------------
                    // 1. 일반 일정(user_cal)으로 추가
                    // ---------------------------------

                    var newSchedule = new CalendarScheduleEntry
                    {
                        Text = SelectedTitle,

                        // NewDdayForm에는 시간 선택이 없으므로
                        // 기본값인 09:00 ~ 10:00 사용
                        StartHour = 9,
                        EndHour = 10,

                        // 기본 카테고리
                        CategoryId = UserCategoryStore.HomeId,

                        // 기본 설정
                        CustomColorArgb = null,
                        IsHighPriority = false,
                        NotificationOffset = 0
                    };

                    calendarDbRepository.Add(
                        loggedInUserId,
                        newSchedule,
                        SelectedDate);


                    // ---------------------------------
                    // 2. D-day(user_dday)에도 저장
                    // ---------------------------------

                    SaveDdayToDatabase();


                    // ---------------------------------
                    // 3. 창 닫기
                    // ---------------------------------

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
        private void SaveDdayToDatabase()
{
    using var connection = new DBConnection().GetConnection();
    connection.Open();

    const string sql = @"
        INSERT INTO user_dday
        (user_id, date, content, type)
        VALUES
        (@user_id, @date, @content, @type)";

    using var command = new MySqlCommand(sql, connection);

    command.Parameters.AddWithValue("@user_id", loggedInUserId);
    command.Parameters.AddWithValue("@date", SelectedDate.Date);
    command.Parameters.AddWithValue("@content", SelectedTitle);
    command.Parameters.AddWithValue(
        "@type",
        StartFromOne ? 1 : 0);

    command.ExecuteNonQuery();
}
    }
}
