using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace calendar4.Services
{
    internal class DiaryDbRepository
    {
        private readonly DBConnection dbConnection = new();


        // ==============================
        // 일기 추가
        // ==============================
        public int Add(
            int userId,
            DiaryEntry diary)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                INSERT INTO user_diary
                (user_id, title, content, `date`)
                VALUES
                (@user_id, @title, @content, @date)";

            using var command =
                new MySqlCommand(sql, connection);

            DateTime diaryDate =
                DateTime.Parse(diary.DateStr);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            command.Parameters.AddWithValue(
                "@title",
                diary.Title);

            command.Parameters.AddWithValue(
                "@content",
                diary.Content);

            command.Parameters.AddWithValue(
                "@date",
                diaryDate.Date);

            command.ExecuteNonQuery();

            return (int)command.LastInsertedId;
        }


        // ==============================
        // 일기 수정
        // ==============================
        public void Update(
            int userId,
            DiaryEntry diary)
        {
            if (diary.DiaryId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                UPDATE user_diary
                SET
                    title = @title,
                    content = @content,
                    `date` = @date
                WHERE diary_id = @diary_id
                  AND user_id = @user_id";

            using var command =
                new MySqlCommand(sql, connection);

            DateTime diaryDate =
                DateTime.Parse(diary.DateStr);

            command.Parameters.AddWithValue(
                "@diary_id",
                diary.DiaryId.Value);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            command.Parameters.AddWithValue(
                "@title",
                diary.Title);

            command.Parameters.AddWithValue(
                "@content",
                diary.Content);

            command.Parameters.AddWithValue(
                "@date",
                diaryDate.Date);

            command.ExecuteNonQuery();
        }


        // ==============================
        // 일기 삭제
        // ==============================
        public void Delete(
            int userId,
            DiaryEntry diary)
        {
            if (diary.DiaryId is null)
                return;

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                DELETE FROM user_diary
                WHERE diary_id = @diary_id
                  AND user_id = @user_id";

            using var command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@diary_id",
                diary.DiaryId.Value);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            command.ExecuteNonQuery();
        }


        // ==============================
        // 로그인 사용자의 일기 전체 불러오기
        // ==============================
        public Dictionary<string, DiaryEntry> Load(
            int userId)
        {
            var diaryMap =
                new Dictionary<string, DiaryEntry>();

            using var connection = dbConnection.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT
                    diary_id,
                    title,
                    content,
                    `date`
                FROM user_diary
                WHERE user_id = @user_id
                ORDER BY `date`";

            using var command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@user_id",
                userId);

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                int diaryId =
                    reader.GetInt32("diary_id");

                string title =
                    reader.GetString("title");

                string content =
                    reader.GetString("content");

                DateTime diaryDate =
                    reader.GetDateTime("date");

                string dateKey =
                    diaryDate.ToString("yyyy-MM-dd");

                diaryMap[dateKey] =
                    new DiaryEntry
                    {
                        DiaryId = diaryId,
                        DateStr = dateKey,
                        Title = title,
                        Content = content
                    };
            }

            return diaryMap;
        }
    }
}