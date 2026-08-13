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

        public int Add(
    int userId,
    CalendarScheduleEntry schedule,
    DateTime date)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
        INSERT INTO user_cal
        (user_id, title, start_date, end_date,
         category_id, color, important, notification)
        VALUES
        (@user_id, @title, @start_date, @end_date,
         @category_id, @color, @important, @notification)";

            using var command = new MySqlCommand(sql, connection);

            DateTime startDate =
                date.Date.AddHours(schedule.StartHour);

            DateTime endDate =
                date.Date.AddHours(schedule.EndHour);

            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@title", schedule.Text);
            command.Parameters.AddWithValue("@start_date", startDate);
            command.Parameters.AddWithValue("@end_date", endDate);

            command.Parameters.AddWithValue(
                "@category_id",
                schedule.CategoryId);

            command.Parameters.AddWithValue(
                "@color",
                schedule.CustomColorArgb.HasValue
                    ? schedule.CustomColorArgb.Value
                    : DBNull.Value);

            command.Parameters.AddWithValue(
                "@important",
                schedule.IsHighPriority);

            command.Parameters.AddWithValue(
                "@notification",
                schedule.NotificationOffset);

            command.ExecuteNonQuery();

            return (int)command.LastInsertedId;
        }

        public void Update(
            int userId,
            CalendarScheduleEntry schedule,
            DateTime date)
        {
            if (schedule.CalId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                UPDATE user_cal
                SET title = @title,
                    start_date = @start_date,
                    end_date = @end_date,
                    category_id = @category_id,
                    color = @color,
                    important = @important,
                    notification = @notification
                WHERE cal_id = @cal_id
                  AND user_id = @user_id";

            using var command = new MySqlCommand(sql, connection);

            DateTime startDate =
                date.Date.AddHours(schedule.StartHour);

            DateTime endDate =
                date.Date.AddHours(schedule.EndHour);

            command.Parameters.AddWithValue(
                "@cal_id",
                schedule.CalId.Value);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            command.Parameters.AddWithValue(
                "@title",
                schedule.Text);

            command.Parameters.AddWithValue(
                "@start_date",
                startDate);

            command.Parameters.AddWithValue(
                "@end_date",
                endDate);

            command.Parameters.AddWithValue(
                "@category_id",
                schedule.CategoryId);

            command.Parameters.AddWithValue(
                "@color",
                schedule.CustomColorArgb.HasValue
                    ? schedule.CustomColorArgb.Value
                    : DBNull.Value);

            command.Parameters.AddWithValue(
                "@important",
                schedule.IsHighPriority);

            command.Parameters.AddWithValue(
                "@notification",
                schedule.NotificationOffset);

            command.ExecuteNonQuery();
        }


        public void Delete(
            int userId,
            CalendarScheduleEntry schedule)
        {
            if (schedule.CalId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                DELETE FROM user_cal
                WHERE cal_id = @cal_id
                  AND user_id = @user_id";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@cal_id",
                schedule.CalId.Value);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            command.ExecuteNonQuery();
        }


        public Dictionary<DateTime, List<CalendarScheduleEntry>> Load(
            int userId)
        {
            var scheduleMap =
                new Dictionary<DateTime, List<CalendarScheduleEntry>>();

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT
                    cal_id,
                    title,
                    start_date,
                    end_date,
                    category_id,
                    color,
                    important,
                    notification
                FROM user_cal
                WHERE user_id = @user_id
                ORDER BY start_date";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int calId =
                    reader.GetInt32("cal_id");

                string title =
                    reader.GetString("title");

                DateTime startDate =
                    reader.GetDateTime("start_date");

                DateTime endDate =
                    reader.GetDateTime("end_date");

                DateTime date =
                    startDate.Date;

                var schedule = new CalendarScheduleEntry
                {
                    CalId = calId,
                    Text = title,
                    StartHour = startDate.Hour,
                    EndHour = endDate.Hour,

                    CategoryId =
                        reader.GetString("category_id"),

                    CustomColorArgb =
                        reader.IsDBNull(
                            reader.GetOrdinal("color"))
                        ? null
                        : reader.GetInt32("color"),

                    IsHighPriority =
                        reader.GetBoolean("important"),

                    NotificationOffset =
                        reader.GetInt32("notification")
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