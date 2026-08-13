using calendar4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace calendar4.Services
{
    internal class PlannerDbRepository
    {
        private readonly DBConnection dbConnection = new();


        // =========================================================
        // 특정 날짜 플래너 저장
        // =========================================================
        public void Save(
            int userId,
            DateTime date,
            PlannerData data)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // -----------------------------------------
                // 기존 해당 날짜 체크리스트 삭제
                // -----------------------------------------
                const string deleteTasksSql = @"
                    DELETE FROM user_planner
                    WHERE user_id = @user_id
                      AND `date` = @date";

                using (var command =
                    new MySqlCommand(
                        deleteTasksSql,
                        connection,
                        transaction))
                {
                    command.Parameters.AddWithValue(
                        "@user_id",
                        userId);

                    command.Parameters.AddWithValue(
                        "@date",
                        date.Date);

                    command.ExecuteNonQuery();
                }


                // -----------------------------------------
                // 기존 해당 날짜 시간 블록 삭제
                // -----------------------------------------
                const string deleteTimeSql = @"
                    DELETE FROM user_planner_time
                    WHERE user_id = @user_id
                      AND `date` = @date";

                using (var command =
                    new MySqlCommand(
                        deleteTimeSql,
                        connection,
                        transaction))
                {
                    command.Parameters.AddWithValue(
                        "@user_id",
                        userId);

                    command.Parameters.AddWithValue(
                        "@date",
                        date.Date);

                    command.ExecuteNonQuery();
                }


                // -----------------------------------------
                // 체크리스트 저장
                // -----------------------------------------
                const string insertTaskSql = @"
                    INSERT INTO user_planner
                    (
                        user_id,
                        `date`,
                        title,
                        completed
                    )
                    VALUES
                    (
                        @user_id,
                        @date,
                        @title,
                        @completed
                    )";

                foreach (var task in data.Tasks)
                {
                    using var command =
                        new MySqlCommand(
                            insertTaskSql,
                            connection,
                            transaction);

                    command.Parameters.AddWithValue(
                        "@user_id",
                        userId);

                    command.Parameters.AddWithValue(
                        "@date",
                        date.Date);

                    command.Parameters.AddWithValue(
                        "@title",
                        task.Name);

                    command.Parameters.AddWithValue(
                        "@completed",
                        task.Completed);

                    command.ExecuteNonQuery();
                }


                // -----------------------------------------
                // 시간 블록 저장
                // -----------------------------------------
                const string insertTimeSql = @"
                    INSERT INTO user_planner_time
                    (
                        user_id,
                        `date`,
                        title,
                        hour,
                        start_m,
                        end_m,
                        color_r,
                        color_g,
                        color_b
                    )
                    VALUES
                    (
                        @user_id,
                        @date,
                        @title,
                        @hour,
                        @start_m,
                        @end_m,
                        @color_r,
                        @color_g,
                        @color_b
                    )";

                foreach (var slot in data.TimeSlots)
                {
                    int startMinute =
                        GetStartMinute(slot);

                    int endMinute =
                        GetEndMinute(slot);

                    using var command =
                        new MySqlCommand(
                            insertTimeSql,
                            connection,
                            transaction);

                    command.Parameters.AddWithValue(
                        "@user_id",
                        userId);

                    command.Parameters.AddWithValue(
                        "@date",
                        date.Date);

                    command.Parameters.AddWithValue(
                        "@title",
                        string.IsNullOrWhiteSpace(slot.TaskName)
                            ? DBNull.Value
                            : slot.TaskName);

                    command.Parameters.AddWithValue(
                        "@hour",
                        slot.Hour);

                    command.Parameters.AddWithValue(
                        "@start_m",
                        startMinute);

                    command.Parameters.AddWithValue(
                        "@end_m",
                        endMinute);

                    command.Parameters.AddWithValue(
                        "@color_r",
                        slot.R);

                    command.Parameters.AddWithValue(
                        "@color_g",
                        slot.G);

                    command.Parameters.AddWithValue(
                        "@color_b",
                        slot.B);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        // =========================================================
        // 로그인한 사용자의 플래너 전체 불러오기
        // =========================================================
        public Dictionary<string, PlannerData> Load(int userId)
        {
            var plannerMap =
                new Dictionary<string, PlannerData>();

            using var connection =
                dbConnection.GetConnection();

            connection.Open();


            // =====================================================
            // 체크리스트 불러오기
            // =====================================================
            const string taskSql = @"
                SELECT
                    `date`,
                    title,
                    completed
                FROM user_planner
                WHERE user_id = @user_id
                ORDER BY `date`, planner_id";

            using (var command =
                new MySqlCommand(taskSql, connection))
            {
                command.Parameters.AddWithValue(
                    "@user_id",
                    userId);

                using var reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date =
                        reader.GetDateTime("date");

                    string key =
                        date.ToString("yyyy-MM-dd");

                    if (!plannerMap.ContainsKey(key))
                    {
                        plannerMap[key] =
                            new PlannerData();
                    }

                    plannerMap[key].Tasks.Add(
                        new PlannerTask
                        {
                            Name =
                                reader.GetString("title"),

                            Completed =
                                reader.GetBoolean("completed")
                        });
                }
            }


            // =====================================================
            // 시간 블록 불러오기
            // =====================================================
            const string timeSql = @"
                SELECT
                    `date`,
                    title,
                    hour,
                    start_m,
                    end_m,
                    color_r,
                    color_g,
                    color_b
                FROM user_planner_time
                WHERE user_id = @user_id
                ORDER BY `date`, hour, start_m";

            using (var command =
                new MySqlCommand(timeSql, connection))
            {
                command.Parameters.AddWithValue(
                    "@user_id",
                    userId);

                using var reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date =
                        reader.GetDateTime("date");

                    string key =
                        date.ToString("yyyy-MM-dd");

                    if (!plannerMap.ContainsKey(key))
                    {
                        plannerMap[key] =
                            new PlannerData();
                    }

                    var slot =
                        new PlannerTimeSlot
                        {
                            Hour =
                                reader.GetInt32("hour"),

                            TaskName =
                                reader.IsDBNull(
                                    reader.GetOrdinal("title"))
                                    ? string.Empty
                                    : reader.GetString("title"),

                            R =
                                reader.GetInt32("color_r"),

                            G =
                                reader.GetInt32("color_g"),

                            B =
                                reader.GetInt32("color_b")
                        };

                    int startMinute =
                        reader.GetInt32("start_m");

                    int endMinute =
                        reader.GetInt32("end_m");

                    SetStartMinute(
                        slot,
                        startMinute);

                    SetEndMinute(
                        slot,
                        endMinute);

                    plannerMap[key]
                        .TimeSlots
                        .Add(slot);
                }
            }

            return plannerMap;
        }


        // =========================================================
        // PlannerTimeSlot 시작/종료 분 처리
        //
        // 현재 PlannerControl 코드가
        // StartMinute / StartMin 두 이름을 모두 대응하고 있어서
        // DB Repository도 동일하게 처리
        // =========================================================

        private static int GetStartMinute(
            PlannerTimeSlot slot)
        {
            var property =
                typeof(PlannerTimeSlot)
                    .GetProperty("StartMinute")
                ??
                typeof(PlannerTimeSlot)
                    .GetProperty("StartMin");

            if (property == null)
                return 0;

            return Convert.ToInt32(
                property.GetValue(slot) ?? 0);
        }


        private static int GetEndMinute(
            PlannerTimeSlot slot)
        {
            var property =
                typeof(PlannerTimeSlot)
                    .GetProperty("EndMinute")
                ??
                typeof(PlannerTimeSlot)
                    .GetProperty("EndMin");

            if (property == null)
                return 0;

            return Convert.ToInt32(
                property.GetValue(slot) ?? 0);
        }


        private static void SetStartMinute(
            PlannerTimeSlot slot,
            int value)
        {
            var property =
                typeof(PlannerTimeSlot)
                    .GetProperty("StartMinute")
                ??
                typeof(PlannerTimeSlot)
                    .GetProperty("StartMin");

            property?.SetValue(
                slot,
                value);
        }


        private static void SetEndMinute(
            PlannerTimeSlot slot,
            int value)
        {
            var property =
                typeof(PlannerTimeSlot)
                    .GetProperty("EndMinute")
                ??
                typeof(PlannerTimeSlot)
                    .GetProperty("EndMin");

            property?.SetValue(
                slot,
                value);
        }
    }
}