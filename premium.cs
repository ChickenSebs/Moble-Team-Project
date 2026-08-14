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
    public partial class premium : Form
    {
        // 1. timer1 선언을 제거하고, 팝업 폼과 진행률 변수만 남깁니다.
        private int progressValue = 0;
        private Form loadingForm;
        private ProgressBar progressBar1;
        private int loggedInUserId;

        public premium()
        {
            InitializeComponent();
        }
        public premium(int userId)
        {
            InitializeComponent();
            loggedInUserId = userId;
        }
        private void btn_out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void premium_Load(object sender, EventArgs e)
        {
            cb_pay.Items.Clear();
            cb_pay.Items.Add("신용/체크카드");
            cb_pay.Items.Add("간편결제");
            cb_pay.Items.Add("실시간 계좌이체");

            cb_pay.SelectedIndex = -1;
            cb_select.Enabled = false;

            cb_pay.SelectedIndexChanged += cb_pay_SelectedIndexChanged;

            // 2. 도구상자의 timer1 속성 및 이벤트 연결
            timer1.Interval = 30; // 30ms * 100회 = 3000ms (3초)
            timer1.Tick -= Timer1_Tick; // 중복 연결 방지 후 등록
            timer1.Tick += Timer1_Tick;
            if (IsAlreadyPremium())
            {
                btn_pay.Enabled = false;

                MessageBox.Show(
                    "이미 프리미엄 회원입니다.",
                    "프리미엄",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void cb_pay_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_select.Items.Clear();

            if (cb_pay.SelectedItem == null)
            {
                cb_select.Enabled = false;
                return;
            }
            string selectPay = cb_pay.SelectedItem.ToString();

            switch (selectPay)
            {
                case "신용/체크카드":
                    cb_select.Items.AddRange(new string[] { "삼성카드", "현대카드", "BC카드", "KB국민카드", "NH농협카드", "신한카드" });
                    break;
                case "간편결제":
                    cb_select.Items.AddRange(new string[] { "네이버페이", "카카오페이", "토스페이" });
                    break;
                case "실시간 계좌이체":
                    cb_select.Items.AddRange(new string[] { "KB국민은행", "NH농협은행", "신한은행", "카카오뱅크", "토스뱅크" });
                    break;
            }
            if (cb_select.Items.Count > 0)
            {
                cb_select.Enabled = true;
                cb_select.SelectedIndex = 0;
            }
        }

        private void btn_pay_Click(object sender, EventArgs e)
        {
            if (IsAlreadyPremium())
            {
                MessageBox.Show(
                    "이미 프리미엄 회원입니다.",
                    "프리미엄",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 2. 결제방법 선택 확인
            if (cb_pay.SelectedItem == null ||
                cb_select.SelectedItem == null)
            {
                MessageBox.Show(
                    "결제방법을 선택해주세요.",
                    "알림",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            btn_pay.Enabled = false;
            cb_pay.Enabled = false;
            cb_select.Enabled = false;

            CreateLoadingForm();

            progressValue = 0;
            progressBar1.Value = 0;
            timer1.Start();

            loadingForm.ShowDialog(this);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            progressValue += 1;

            if (progressValue <= progressBar1.Maximum)
            {
                progressBar1.Value = progressValue;
            }
            else
            {
                timer1.Stop();

                if (loadingForm != null && !loadingForm.IsDisposed)
                {
                    loadingForm.Close();
                }

                // 결제 완료 후 DB의 premium 값을 1로 변경
                bool updateSuccess = UpdatePremiumStatus();

                if (updateSuccess)
                {
                    CurrentUser.IsPremium = true;

                    MessageBox.Show(
                        "결제가 완료되었습니다!",
                        "결제 성공",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.Close();
                }

                btn_pay.Enabled = true;
                cb_pay.Enabled = true;
                cb_select.Enabled = true;
            }
        }

        private void CreateLoadingForm()
        {
            loadingForm = new Form
            {
                Text = "결제 진행 중",
                Size = new Size(320, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ControlBox = false
            };

            Label lblMessage = new Label
            {
                Text = "결제중입니다...",
                AutoSize = true,
                Left = 20,
                Top = 20,
                Font = new Font("맑은 고딕", 10F, FontStyle.Bold)
            };

            progressBar1 = new ProgressBar
            {
                Left = 20,
                Top = 50,
                Width = 260,
                Height = 23,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            loadingForm.Controls.Add(lblMessage);
            loadingForm.Controls.Add(progressBar1);
        }
        private bool IsAlreadyPremium()
        {
            string connStr =
                "Server=localhost;Database=teamproject;Uid=root;Pwd=1111;";

            string query =
                "SELECT premium FROM user WHERE user_id = @userId";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@userId",
                            loggedInUserId);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                            return false;

                        int premiumValue =
                            Convert.ToInt32(result);

                        return premiumValue == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "프리미엄 정보 확인 중 오류 발생: " + ex.Message,
                        "오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }
        }
        private bool UpdatePremiumStatus()
        {
            string connStr = "Server=localhost;Database=teamproject;Uid=root;Pwd=1111;";
            string query = "UPDATE user SET premium = 1 WHERE user_id = @userId";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                        int result = cmd.ExecuteNonQuery();

                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "프리미엄 정보 저장 중 오류 발생: " + ex.Message,
                        "오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }
        }
    }

}