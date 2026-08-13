using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace calendar4
{

    public partial class Mypage : Form
    {
        // 로그인된 사용자의 ID를 보관하는 변수 (생성자 등을 통해 받아옵니다)
        private int loggedInUserId;

        public Mypage()
        {
            InitializeComponent();
        }

        // 로그인 폼이나 메인 폼에서 user_id를 전달받을 수 있는 생성자 예시
        public Mypage(int userId)
        {
            InitializeComponent();
            this.loggedInUserId = userId;

            LoadPremiumStatus();
        }
        private void LoadPremiumStatus()
        {
            string connStr = "Server=localhost;Database=teamproject;Uid=root;Pwd=1111;";
            string query = "SELECT premium FROM user WHERE user_id = @userId";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int premiumValue = Convert.ToInt32(result);

                            if (premiumValue == 1)
                            {
                                lblPremiumStatus.Text = "프리미엄 사용자";
                            }
                            else
                            {
                                lblPremiumStatus.Text = "일반 사용자";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "프리미엄 상태 확인 중 오류 발생: " + ex.Message,
                        "오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            // 1. 기존 비밀번호 입력 여부 검사
            if (string.IsNullOrWhiteSpace(txtPw.Text))
            {
                MessageBox.Show("회원정보를 수정하려면 기존 비밀번호를 먼저 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPw.Focus();
                return;
            }

            string currentPw = txtPw.Text.Trim();
            string newPw = txtRepw.Text.Trim();
            string newName = txtRename.Text.Trim();
            string newEmail = txtReemail.Text.Trim();

            // 2. 수정할 내용이 하나라도 입력되었는지 검사
            if (string.IsNullOrWhiteSpace(newPw) &&
                string.IsNullOrWhiteSpace(newName) &&
                string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("수정하려는 항목을 하나라도 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. 변경할 항목에 따라 동적으로 SQL UPDATE 문 구성
            List<string> updateFields = new List<string>();
            List<string> updatedItemsText = new List<string>();

            if (!string.IsNullOrWhiteSpace(newPw))
            {
                updateFields.Add("pw = @newPw");
                updatedItemsText.Add("비밀번호");
            }
            if (!string.IsNullOrWhiteSpace(newName))
            {
                updateFields.Add("name = @newName");
                updatedItemsText.Add("이름");
            }
            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                updateFields.Add("email = @newEmail");
                updatedItemsText.Add("이메일");
            }

            // 동적 UPDATE 쿼리 생성
            // 예: UPDATE user SET pw = @newPw, name = @newName WHERE user_id = @userId AND pw = @currentPw
            string query = $"UPDATE user SET {string.Join(", ", updateFields)} WHERE user_id = @userId AND pw = @currentPw";
            string connStr = "Server=localhost;Database=teamproject;Uid=root;Pwd=1111;"; // 또는 DBHelper.GetConnection() 사용

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // 조건절 파라미터 (로그인된 사용자 식별)
                        cmd.Parameters.AddWithValue("@userId", loggedInUserId);
                        cmd.Parameters.AddWithValue("@currentPw", currentPw);

                        // 변경 파라미터 바인딩
                        if (!string.IsNullOrWhiteSpace(newPw))
                            cmd.Parameters.AddWithValue("@newPw", newPw);
                        if (!string.IsNullOrWhiteSpace(newName))
                            cmd.Parameters.AddWithValue("@newName", newName);
                        if (!string.IsNullOrWhiteSpace(newEmail))
                            cmd.Parameters.AddWithValue("@newEmail", newEmail);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string resultMessage = string.Join(", ", updatedItemsText) + " 수정 완료";
                            MessageBox.Show(resultMessage, "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // 입력 필드 초기화
                            txtPw.Clear();
                            txtRepw.Clear();
                            txtRename.Clear();
                            txtReemail.Clear();
                        }
                        else
                        {
                            // rowsAffected가 0이면 기존 비밀번호가 틀렸거나 해당 user_id가 없는 경우
                            MessageBox.Show("기존 비밀번호가 일치하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 발생: " + ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void Mypage_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 실행 중인 폼들 중에서 이름이 "mainForm"인 폼을 찾습니다.
            await Task.Delay(200);
            Form main = Application.OpenForms["mainForm"];

            if (main != null)
            {
                // 숨겨져 있던 메인 폼을 다시 화면에 보여줍니다.
                main.Show();
            }
            else
            {
                mainForm newMain = new mainForm();
                newMain.Show();
            }
        }

        private void btnPremium_Click(object sender, EventArgs e)
        {
            premium premiumForm = new premium(loggedInUserId);
            premiumForm.ShowDialog(this);

            LoadPremiumStatus();
        }
    }
}