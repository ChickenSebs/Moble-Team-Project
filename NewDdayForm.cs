using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calendar4
{
    public partial class NewDdayForm : Form
    {
        public DateTime SelectedDate { get; private set; }
        public string SelectedTitle { get; private set; }
        public bool StartFromOne { get; private set; }
        public NewDdayForm()
        {
            InitializeComponent();
            dtpDdayDate.Value = DateTime.Today;
            rdoZero.Checked = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDdayTitle.Text))
            {
                MessageBox.Show(
                    "D-Day 제목을 입력해주세요.",
                    "D-Day 설정",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtDdayTitle.Focus();
                return;
            }

            SelectedDate = dtpDdayDate.Value.Date;
            SelectedTitle = txtDdayTitle.Text.Trim();
            StartFromOne = rdoOne.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
