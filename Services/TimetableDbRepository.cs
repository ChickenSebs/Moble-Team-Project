using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace calendar4.Services
{
    internal class TimetableDbRepository
    {
        private readonly DBConnection dbConnection = new();

        // 수업 추가
        public int Add(int userId, ClassSchedule schedule)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                INSERT INTO user_timetable
                (
                    user_id,
                    subject,
                    classroom,
                    dayofweek,
                    start_time,
                    end_time,
                    category_id,
                    color
                )
                VALUES
                (
                    @user_id,
                    @subject,
                    @classroom,
                    @dayofweek,
                    @start_time,
                    @end_time,
                    @category_id,
                    @color
                )";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@subject", schedule.SubjectName);

            command.Parameters.AddWithValue(
                "@classroom",
                string.IsNullOrWhiteSpace(schedule.Classroom)
                    ? DBNull.Value
                    : schedule.Classroom);

            command.Parameters.AddWithValue("@dayofweek", (int)schedule.Day);

            command.Parameters.AddWithValue(
                "@start_time",
                TimeSpan.FromHours(schedule.StartHour));

            command.Parameters.AddWithValue(
                "@end_time",
                TimeSpan.FromHours(schedule.EndHour));

            command.Parameters.AddWithValue(
                "@category_id",
                (int)schedule.Category);

            command.Parameters.AddWithValue(
                "@color",
                schedule.CustomColorArgb.HasValue
                    ? schedule.CustomColorArgb.Value
                    : DBNull.Value);

            command.ExecuteNonQuery();

            return (int)command.LastInsertedId;
        }


        // 수업 수정
        public void Update(int userId, ClassSchedule schedule)
        {
            if (schedule.TimetableId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                UPDATE user_timetable
                SET
                    subject = @subject,
                    classroom = @classroom,
                    dayofweek = @dayofweek,
                    start_time = @start_time,
                    end_time = @end_time,
                    category_id = @category_id,
                    color = @color
                WHERE timetable_id = @timetable_id
                  AND user_id = @user_id";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@timetable_id",
                schedule.TimetableId.Value);

            command.Parameters.AddWithValue("@user_id", userId);
            command.Parameters.AddWithValue("@subject", schedule.SubjectName);

            command.Parameters.AddWithValue(
                "@classroom",
                string.IsNullOrWhiteSpace(schedule.Classroom)
                    ? DBNull.Value
                    : schedule.Classroom);

            command.Parameters.AddWithValue("@dayofweek", (int)schedule.Day);

            command.Parameters.AddWithValue(
                "@start_time",
                TimeSpan.FromHours(schedule.StartHour));

            command.Parameters.AddWithValue(
                "@end_time",
                TimeSpan.FromHours(schedule.EndHour));

            command.Parameters.AddWithValue(
                "@category_id",
                (int)schedule.Category);

            command.Parameters.AddWithValue(
                "@color",
                schedule.CustomColorArgb.HasValue
                    ? schedule.CustomColorArgb.Value
                    : DBNull.Value);

            command.ExecuteNonQuery();
        }


        // 수업 삭제
        public void Delete(int userId, ClassSchedule schedule)
        {
            if (schedule.TimetableId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                DELETE FROM user_timetable
                WHERE timetable_id = @timetable_id
                  AND user_id = @user_id";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@timetable_id",
                schedule.TimetableId.Value);

            command.Parameters.AddWithValue("@user_id", userId);

            command.ExecuteNonQuery();
        }


        // 로그인한 사용자의 시간표 불러오기
        public List<ClassSchedule> Load(int userId)
        {
            var schedules = new List<ClassSchedule>();

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT
                    timetable_id,
                    subject,
                    classroom,
                    dayofweek,
                    start_time,
                    end_time,
                    category_id,
                    color
                FROM user_timetable
                WHERE user_id = @user_id
                ORDER BY dayofweek, start_time";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@user_id", userId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var startTime = reader.GetTimeSpan("start_time");
                var endTime = reader.GetTimeSpan("end_time");

                var schedule = new ClassSchedule
                {
                    TimetableId = reader.GetInt32("timetable_id"),

                    SubjectName = reader.GetString("subject"),

                    Classroom =
                        reader.IsDBNull(reader.GetOrdinal("classroom"))
                            ? string.Empty
                            : reader.GetString("classroom"),

                    Day =
                        (DayOfWeek)reader.GetInt32("dayofweek"),

                    StartHour =
                        startTime.Hours,

                    EndHour =
                        endTime.Hours,

                    Category =
                        (ScheduleCategory)reader.GetInt32("category_id"),

                    CustomColorArgb =
                        reader.IsDBNull(reader.GetOrdinal("color"))
                            ? null
                            : reader.GetInt32("color")
                };

                schedules.Add(schedule);
            }

            return schedules;
        }
    }
}