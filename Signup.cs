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
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            string loginId = txtSignupId.Text.Trim();
            string password = txtSignupPassword.Text.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();

            // 1. 입력값 빈칸 검사
            if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("모든 항목을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // 2. 아이디 중복 확인
                    string checkQuery = "SELECT COUNT(*) FROM user WHERE login_id = @login_id";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@login_id", loginId);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("이미 존재하는 아이디입니다.", "중복 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (password != txtPasswordCheck.Text)
                        {
                            MessageBox.Show("비밀번호가 일치하지 않습니다.", "비밀번호 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // 3. DB에 회원 정보 INSERT
                    string insertQuery = "INSERT INTO user (login_id, pw, name, email) VALUES (@login_id, @pw, @name, @email)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@login_id", loginId);
                        insertCmd.Parameters.AddWithValue("@pw", password); // ※ 실무에서는 비밀번호 암호화 후 저장 권장
                        insertCmd.Parameters.AddWithValue("@name", name);
                        insertCmd.Parameters.AddWithValue("@email", email);

                        int result = insertCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("회원가입이 완료되었습니다!", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // 회원가입 창 닫기 (로그인 창으로 복귀)
                        }
                        else
                        {
                            MessageBox.Show("회원가입에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB 연결 및 처리 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
