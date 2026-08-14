namespace calendar4
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            tool_back = new ToolStripMenuItem();
            tool_font = new ToolStripMenuItem();
            tool_theme = new ToolStripMenuItem();
            menuThemeLight = new ToolStripMenuItem();
            menuThemeDark = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            menuThemeBlossom = new ToolStripMenuItem();
            menuThemeMint = new ToolStripMenuItem();
            menuThemeLavender = new ToolStripMenuItem();
            menuThemeCozy = new ToolStripMenuItem();
            tool_con = new ToolStripMenuItem();
            tool_month = new ToolStripMenuItem();
            tool_week = new ToolStripMenuItem();
            tool_day = new ToolStripMenuItem();
            monthCalendar1 = new MonthCalendar();
            richTextBox1 = new RichTextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            tb_search = new TextBox();
            lbmain_title = new Label();
            tabPage1 = new TabPage();
            dgvCalendar = new DataGridView();
            tabControl1 = new TabControl();
            btn_exit = new Button();
            btn_search = new Button();
            btnMy = new Button();
            lbDday = new Label();
            btn_Dday = new Button();
            menuStrip1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { tool_back, tool_con });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1076, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // tool_back
            // 
            tool_back.DropDownItems.AddRange(new ToolStripItem[] { tool_font, tool_theme });
            tool_back.Name = "tool_back";
            tool_back.Size = new Size(43, 20);
            tool_back.Text = "테마";
            // 
            // tool_font
            // 
            tool_font.Name = "tool_font";
            tool_font.Size = new Size(126, 22);
            tool_font.Text = "폰트";
            // 
            // tool_theme
            // 
            tool_theme.DropDownItems.AddRange(new ToolStripItem[] { menuThemeLight, menuThemeDark, toolStripSeparator2, menuThemeBlossom, menuThemeMint, menuThemeLavender, menuThemeCozy });
            tool_theme.Name = "tool_theme";
            tool_theme.Size = new Size(126, 22);
            tool_theme.Text = "테마 변경";
            // 
            // menuThemeLight
            // 
            menuThemeLight.Name = "menuThemeLight";
            menuThemeLight.Size = new Size(126, 22);
            menuThemeLight.Text = "라이트";
            menuThemeLight.Click += menuThemeLight_Click;
            // 
            // menuThemeDark
            // 
            menuThemeDark.Name = "menuThemeDark";
            menuThemeDark.Size = new Size(126, 22);
            menuThemeDark.Text = "다크";
            menuThemeDark.Click += menuThemeDark_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(123, 6);
            // 
            // menuThemeBlossom
            // 
            menuThemeBlossom.Name = "menuThemeBlossom";
            menuThemeBlossom.Size = new Size(126, 22);
            menuThemeBlossom.Text = "🌸 블라썸";
            menuThemeBlossom.Click += menuThemeBlossom_Click;
            // 
            // menuThemeMint
            // 
            menuThemeMint.Name = "menuThemeMint";
            menuThemeMint.Size = new Size(126, 22);
            menuThemeMint.Text = "🍀 민트";
            menuThemeMint.Click += menuThemeMint_Click;
            // 
            // menuThemeLavender
            // 
            menuThemeLavender.Name = "menuThemeLavender";
            menuThemeLavender.Size = new Size(126, 22);
            menuThemeLavender.Text = "☁ 라벤더";
            menuThemeLavender.Click += menuThemeLavender_Click;
            // 
            // menuThemeCozy
            // 
            menuThemeCozy.Name = "menuThemeCozy";
            menuThemeCozy.Size = new Size(126, 22);
            menuThemeCozy.Text = "\U0001f9f8 코지";
            menuThemeCozy.Click += menuThemeCozy_Click;
            // 
            // tool_con
            // 
            tool_con.DropDownItems.AddRange(new ToolStripItem[] { tool_month, tool_week, tool_day });
            tool_con.Name = "tool_con";
            tool_con.Size = new Size(71, 20);
            tool_con.Text = "보기 옵션";
            // 
            // tool_month
            // 
            tool_month.Name = "tool_month";
            tool_month.ShortcutKeys = Keys.Control | Keys.M;
            tool_month.Size = new Size(131, 22);
            tool_month.Text = "월";
            // 
            // tool_week
            // 
            tool_week.Name = "tool_week";
            tool_week.ShortcutKeys = Keys.Control | Keys.W;
            tool_week.Size = new Size(131, 22);
            tool_week.Text = "주";
            // 
            // tool_day
            // 
            tool_day.Name = "tool_day";
            tool_day.ShortcutKeys = Keys.Control | Keys.D;
            tool_day.Size = new Size(131, 22);
            tool_day.Text = "일";
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(9, 69);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(9, 227);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(220, 372);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // tb_search
            // 
            tb_search.Font = new Font("맑은 고딕", 12.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_search.Location = new Point(815, 40);
            tb_search.Name = "tb_search";
            tb_search.Size = new Size(149, 30);
            tb_search.TabIndex = 6;
            // 
            // lbmain_title
            // 
            lbmain_title.AutoSize = true;
            lbmain_title.BackColor = Color.Transparent;
            lbmain_title.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lbmain_title.Location = new Point(583, 27);
            lbmain_title.Name = "lbmain_title";
            lbmain_title.Size = new Size(112, 45);
            lbmain_title.TabIndex = 7;
            lbmain_title.Text = "label1";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvCalendar);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(814, 502);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvCalendar
            // 
            dgvCalendar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCalendar.Dock = DockStyle.Fill;
            dgvCalendar.Location = new Point(3, 3);
            dgvCalendar.Name = "dgvCalendar";
            dgvCalendar.RowTemplate.Height = 25;
            dgvCalendar.Size = new Size(808, 496);
            dgvCalendar.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new Point(250, 69);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(822, 530);
            tabControl1.TabIndex = 3;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // btn_exit
            // 
            btn_exit.Location = new Point(134, 27);
            btn_exit.Name = "btn_exit";
            btn_exit.Size = new Size(95, 32);
            btn_exit.TabIndex = 4;
            btn_exit.Text = "로그아웃";
            btn_exit.UseVisualStyleBackColor = true;
            btn_exit.Click += btn_exit_Click;
            // 
            // btn_search
            // 
            btn_search.Location = new Point(970, 40);
            btn_search.Name = "btn_search";
            btn_search.Size = new Size(95, 30);
            btn_search.TabIndex = 5;
            btn_search.Text = "검색";
            btn_search.UseVisualStyleBackColor = true;
            btn_search.Click += btn_search_Click;
            // 
            // btnMy
            // 
            btnMy.Location = new Point(12, 27);
            btnMy.Name = "btnMy";
            btnMy.Size = new Size(95, 32);
            btnMy.TabIndex = 4;
            btnMy.Text = "마이페이지";
            btnMy.UseVisualStyleBackColor = true;
            btnMy.Click += btnMy_Click;
            // 
            // lbDday
            // 
            lbDday.AutoSize = true;
            lbDday.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbDday.Location = new Point(250, 38);
            lbDday.Name = "lbDday";
            lbDday.Size = new Size(97, 21);
            lbDday.TabIndex = 8;
            lbDday.Text = "D-Day 없음";
            // 
            // btn_Dday
            // 
            btn_Dday.Location = new Point(353, 35);
            btn_Dday.Name = "btn_Dday";
            btn_Dday.Size = new Size(66, 30);
            btn_Dday.TabIndex = 9;
            btn_Dday.Text = "설정";
            btn_Dday.UseVisualStyleBackColor = true;
            btn_Dday.Click += btn_Dday_Click;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1076, 603);
            Controls.Add(btn_Dday);
            Controls.Add(lbDday);
            Controls.Add(lbmain_title);
            Controls.Add(tb_search);
            Controls.Add(btn_search);
            Controls.Add(btnMy);
            Controls.Add(btn_exit);
            Controls.Add(tabControl1);
            Controls.Add(richTextBox1);
            Controls.Add(monthCalendar1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "mainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "캘린더";
            FormClosing += mainForm_FormClosing;
            Load += mainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem tool_back;
        private ToolStripMenuItem tool_font;
        private ToolStripMenuItem tool_theme;
        private ToolStripMenuItem tool_con;
        private ToolStripMenuItem tool_month;
        private ToolStripMenuItem tool_week;
        private MonthCalendar monthCalendar1;
        private RichTextBox richTextBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TextBox tb_search;
        private Label lbmain_title;
        private TabPage tabPage1;
        private DataGridView dgvCalendar;
        private TabControl tabControl1;
        private Button btn_exit;
        private Button btn_search;
        private ToolStripMenuItem tool_day;
        private Button btnMy;
        private ToolStripMenuItem menuThemeLight;
        private ToolStripMenuItem menuThemeDark;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuThemeBlossom;
        private ToolStripMenuItem menuThemeMint;
        private ToolStripMenuItem menuThemeLavender;
        private ToolStripMenuItem menuThemeCozy;
        private Label lbDday;
        private Button btn_Dday;
    }
}
