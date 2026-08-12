using calendar4;
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

namespace tap
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load_1(object sender, EventArgs e)
        {
            if (calendar4.remember.Default.IsIdSaved)
            {
                txtLoginId.Text = calendar4.remember.Default.SavedId;
                chkRememberId.Checked = true;
            }
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            Signup signup = new Signup();
            signup.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginId = txtLoginId.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("아이디와 비밀번호를 모두 입력해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // 아이디와 비밀번호가 일치하는 회원 정보 조회
                    string selectQuery = "SELECT user_id, name FROM user WHERE login_id = @login_id AND pw = @pw";

                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@login_id", loginId);
                        cmd.Parameters.AddWithValue("@pw", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // 조회 결과가 존재하는 경우 (로그인 성공)
                            if (reader.Read())
                            {
                                if (chkRememberId.Checked)
                                {
                                    calendar4.remember.Default.SavedId = loginId;
                                    calendar4.remember.Default.IsIdSaved = true;
                                }
                                else
                                {
                                    calendar4.remember.Default.SavedId = "";
                                    calendar4.remember.Default.IsIdSaved = false;
                                }
                                calendar4.remember.Default.Save();

                                int loggedInUserId = Convert.ToInt32(reader["user_id"]);
                                string userName = reader["name"].ToString();

                                MessageBox.Show($"{userName}님, 환영합니다!", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // 메인 폼으로 user_id를 전달하며 이동
                                mainForm mainForm = new mainForm(loggedInUserId);
                                mainForm.Show();

                                // 로그인 폼 숨기기
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB 연결 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHello_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello cho");
        }
    }
}
