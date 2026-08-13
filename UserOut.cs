using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace calendar4
{
    public partial class UserOut : Form
    {
        private readonly int loggedInUserId;

        public UserOut(int userId)
        {
            InitializeComponent();
            this.loggedInUserId = userId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = txtOutpw.Text.Trim();
            DeleteAccount(password);
        }

        private void DeleteAccount(string password)
        {
            using var connection = new DBConnection().GetConnection();

            try
            {
                connection.Open();

                // 1. 현재 로그인 사용자 + 비밀번호가 일치하는지 확인
                const string checkSql = @"
            SELECT COUNT(*)
            FROM user
            WHERE user_id = @user_id
              AND pw = @password";

                using (var checkCommand = new MySqlCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue(
                        "@user_id",
                        loggedInUserId);

                    checkCommand.Parameters.AddWithValue(
                        "@password",
                        password);

                    int count =
                        Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        MessageBox.Show(
                            "비밀번호가 일치하지 않습니다.",
                            "회원탈퇴",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }


                // 2. 하나라도 실패하면 전체 삭제를 취소하기 위한 Transaction
                using var transaction =
                    connection.BeginTransaction();

                try
                {
                    string[] deleteQueries =
                    {
                "DELETE FROM user_cal WHERE user_id = @user_id",
                "DELETE FROM user_dday WHERE user_id = @user_id",
                "DELETE FROM user_diary WHERE user_id = @user_id",
                "DELETE FROM user_planner WHERE user_id = @user_id",
                "DELETE FROM user_planner_time WHERE user_id = @user_id",
                "DELETE FROM user_timetable WHERE user_id = @user_id",

                // user는 반드시 마지막
                "DELETE FROM user WHERE user_id = @user_id"
            };

                    foreach (string sql in deleteQueries)
                    {
                        using var command =
                            new MySqlCommand(
                                sql,
                                connection,
                                transaction);

                        command.Parameters.AddWithValue(
                            "@user_id",
                            loggedInUserId);

                        command.ExecuteNonQuery();
                    }

                    // 모든 DELETE가 성공했을 때만 확정
                    transaction.Commit();


                    MessageBox.Show(
                        "회원탈퇴가 완료되었습니다.",
                        "회원탈퇴",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    // 중간에 하나라도 실패하면 삭제 전 상태로 복구
                    transaction.Rollback();

                    MessageBox.Show(
                        $"회원탈퇴 처리 중 오류가 발생했습니다.\n\n{ex.Message}",
                        "DB 오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"DB 연결 중 오류가 발생했습니다.\n\n{ex.Message}",
                    "DB 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
