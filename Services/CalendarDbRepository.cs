using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace calendar4.Services
{
    internal class CalendarDbRepository
    {
        private readonly DBConnection dbConnection = new();

        public void Save(
            int userId,
            Dictionary<DateTime, List<CalendarScheduleEntry>> scheduleMap)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // 일단 현재 사용자의 기존 일정을 모두 삭제
                using (var deleteCommand = new MySqlCommand(
                    "DELETE FROM user_cal WHERE user_id = @user_id",
                    connection,
                    transaction))
                {
                    deleteCommand.Parameters.AddWithValue("@user_id", userId);
                    deleteCommand.ExecuteNonQuery();
                }

                // 현재 메모리에 있는 일정을 DB에 다시 저장
                foreach (var dateSchedules in scheduleMap)
                {
                    DateTime date = dateSchedules.Key;

                    foreach (var schedule in dateSchedules.Value)
                    {
                        DateTime startDate =
                            date.Date.AddHours(schedule.StartHour);

                        DateTime endDate =
                            date.Date.AddHours(schedule.EndHour);

                        using var insertCommand = new MySqlCommand(
                            @"INSERT INTO user_cal
                              (user_id, title, start_date, end_date, share_id)
                              VALUES
                              (@user_id, @title, @start_date, @end_date, NULL)",
                            connection,
                            transaction);

                        insertCommand.Parameters.AddWithValue(
                            "@user_id", userId);

                        insertCommand.Parameters.AddWithValue(
                            "@title", schedule.Text);

                        insertCommand.Parameters.AddWithValue(
                            "@start_date", startDate);

                        insertCommand.Parameters.AddWithValue(
                            "@end_date", endDate);

                        insertCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public Dictionary<DateTime, List<CalendarScheduleEntry>> Load(int userId)
        {
            var scheduleMap =
                new Dictionary<DateTime, List<CalendarScheduleEntry>>();

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
        SELECT title, start_date, end_date
        FROM user_cal
        WHERE user_id = @user_id
        ORDER BY start_date";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@user_id", userId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string title = reader.GetString("title");

                DateTime startDate = reader.GetDateTime("start_date");
                DateTime endDate = reader.GetDateTime("end_date");

                DateTime date = startDate.Date;

                var schedule = new CalendarScheduleEntry
                {
                    Text = title,
                    StartHour = startDate.Hour,
                    EndHour = endDate.Hour,
                    CategoryId = "0",
                    CustomColorArgb = null
                };

                if (!scheduleMap.ContainsKey(date))
                {
                    scheduleMap[date] =
                        new List<CalendarScheduleEntry>();
                }

                scheduleMap[date].Add(schedule);
            }

            return scheduleMap;
        }
    }
}
