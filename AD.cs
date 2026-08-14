using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace calendar4
{
    public partial class AD : Form
    {
        private readonly Random random = new Random();
        public AD()
        {
            InitializeComponent();
            ShowRandomAd();
        }
        private void ShowRandomAd()
        {
            string[] ads =
            {
                "ad1.png",
                "ad2.png"
            };

            string selectedAd = ads[random.Next(ads.Length)];

            string imagePath = Path.Combine(Application.StartupPath,"Ads",selectedAd);

            pictureBoxAd.Image = Image.FromFile(imagePath);

            pictureBoxAd.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("프리미엄 회원이 되면 광고를 제거할 수 있습니다.\n(마이페이지에서 요금제 구매 가능)", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
