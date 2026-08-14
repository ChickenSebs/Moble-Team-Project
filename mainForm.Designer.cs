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
            맑은고딕ToolStripMenuItem = new ToolStripMenuItem();
            바탕체ToolStripMenuItem = new ToolStripMenuItem();
            돋움ToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            한컴말랑말랑ToolStripMenuItem = new ToolStripMenuItem();
            훈민정음가로쓰기ToolStripMenuItem = new ToolStripMenuItem();
            한컴산뜻돋움ToolStripMenuItem = new ToolStripMenuItem();
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
            계정ToolStripMenuItem = new ToolStripMenuItem();
            마이페이지ToolStripMenuItem = new ToolStripMenuItem();
            로그인ToolStripMenuItem = new ToolStripMenuItem();
            디데이ToolStripMenuItem = new ToolStripMenuItem();
            monthCalendar1 = new MonthCalendar();
            richTextBox1 = new RichTextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            tb_search = new TextBox();
            lbmain_title = new Label();
            tabPage1 = new TabPage();
            dgvCalendar = new DataGridView();
            tabControl1 = new TabControl();
            btn_search = new Button();
            lbDday = new Label();
            menuStrip1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCalendar).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Window;
            menuStrip1.Items.AddRange(new ToolStripItem[] { tool_back, tool_con, 계정ToolStripMenuItem, 디데이ToolStripMenuItem });
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
            tool_font.DropDownItems.AddRange(new ToolStripItem[] { 맑은고딕ToolStripMenuItem, 바탕체ToolStripMenuItem, 돋움ToolStripMenuItem, toolStripSeparator1, 한컴말랑말랑ToolStripMenuItem, 훈민정음가로쓰기ToolStripMenuItem, 한컴산뜻돋움ToolStripMenuItem });
            tool_font.Name = "tool_font";
            tool_font.Size = new Size(126, 22);
            tool_font.Text = "폰트";
            // 
            // 맑은고딕ToolStripMenuItem
            // 
            맑은고딕ToolStripMenuItem.Name = "맑은고딕ToolStripMenuItem";
            맑은고딕ToolStripMenuItem.Size = new Size(174, 22);
            맑은고딕ToolStripMenuItem.Text = "맑은 고딕";
            맑은고딕ToolStripMenuItem.Click += 맑은고딕ToolStripMenuItem_Click;
            // 
            // 바탕체ToolStripMenuItem
            // 
            바탕체ToolStripMenuItem.Name = "바탕체ToolStripMenuItem";
            바탕체ToolStripMenuItem.Size = new Size(174, 22);
            바탕체ToolStripMenuItem.Text = "바탕체";
            바탕체ToolStripMenuItem.Click += 바탕체ToolStripMenuItem_Click;
            // 
            // 돋움ToolStripMenuItem
            // 
            돋움ToolStripMenuItem.Name = "돋움ToolStripMenuItem";
            돋움ToolStripMenuItem.Size = new Size(174, 22);
            돋움ToolStripMenuItem.Text = "돋움";
            돋움ToolStripMenuItem.Click += 돋움ToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(171, 6);
            // 
            // 한컴말랑말랑ToolStripMenuItem
            // 
            한컴말랑말랑ToolStripMenuItem.Name = "한컴말랑말랑ToolStripMenuItem";
            한컴말랑말랑ToolStripMenuItem.Size = new Size(174, 22);
            한컴말랑말랑ToolStripMenuItem.Text = "한컴 말랑말랑";
            한컴말랑말랑ToolStripMenuItem.Click += 한컴말랑말랑ToolStripMenuItem_Click;
            // 
            // 훈민정음가로쓰기ToolStripMenuItem
            // 
            훈민정음가로쓰기ToolStripMenuItem.Name = "훈민정음가로쓰기ToolStripMenuItem";
            훈민정음가로쓰기ToolStripMenuItem.Size = new Size(174, 22);
            훈민정음가로쓰기ToolStripMenuItem.Text = "훈민정음 가로쓰기";
            훈민정음가로쓰기ToolStripMenuItem.Click += 훈민정음가로쓰기ToolStripMenuItem_Click;
            // 
            // 한컴산뜻돋움ToolStripMenuItem
            // 
            한컴산뜻돋움ToolStripMenuItem.Name = "한컴산뜻돋움ToolStripMenuItem";
            한컴산뜻돋움ToolStripMenuItem.Size = new Size(174, 22);
            한컴산뜻돋움ToolStripMenuItem.Text = "한컴 산뜻돋움";
            한컴산뜻돋움ToolStripMenuItem.Click += 한컴산뜻돋움ToolStripMenuItem_Click;
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
            // 계정ToolStripMenuItem
            // 
            계정ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 마이페이지ToolStripMenuItem, 로그인ToolStripMenuItem });
            계정ToolStripMenuItem.Name = "계정ToolStripMenuItem";
            계정ToolStripMenuItem.Size = new Size(43, 20);
            계정ToolStripMenuItem.Text = "계정";
            // 
            // 마이페이지ToolStripMenuItem
            // 
            마이페이지ToolStripMenuItem.Name = "마이페이지ToolStripMenuItem";
            마이페이지ToolStripMenuItem.Size = new Size(134, 22);
            마이페이지ToolStripMenuItem.Text = "마이페이지";
            마이페이지ToolStripMenuItem.Click += btnMy_Click;
            // 
            // 로그인ToolStripMenuItem
            // 
            로그인ToolStripMenuItem.Name = "로그인ToolStripMenuItem";
            로그인ToolStripMenuItem.Size = new Size(134, 22);
            로그인ToolStripMenuItem.Text = "로그아웃";
            로그인ToolStripMenuItem.Click += btn_exit_Click;
            // 
            // 디데이ToolStripMenuItem
            // 
            디데이ToolStripMenuItem.Name = "디데이ToolStripMenuItem";
            디데이ToolStripMenuItem.Size = new Size(55, 20);
            디데이ToolStripMenuItem.Text = "디데이";
            디데이ToolStripMenuItem.Click += btn_Dday_Click;
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(11, 111);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 285);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(220, 310);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // tb_search
            // 
            tb_search.Font = new Font("맑은 고딕", 12.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_search.Location = new Point(11, 69);
            tb_search.Name = "tb_search";
            tb_search.Size = new Size(149, 30);
            tb_search.TabIndex = 6;
            // 
            // lbmain_title
            // 
            lbmain_title.AutoSize = true;
            lbmain_title.BackColor = Color.Transparent;
            lbmain_title.FlatStyle = FlatStyle.Flat;
            lbmain_title.Font = new Font("맑은 고딕", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            lbmain_title.Location = new Point(243, 26);
            lbmain_title.Name = "lbmain_title";
            lbmain_title.Size = new Size(100, 40);
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
            dgvCalendar.BackgroundColor = SystemColors.Window;
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
            tabControl1.Location = new Point(243, 69);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(822, 530);
            tabControl1.TabIndex = 3;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // btn_search
            // 
            btn_search.Location = new Point(166, 69);
            btn_search.Name = "btn_search";
            btn_search.Size = new Size(65, 30);
            btn_search.TabIndex = 5;
            btn_search.Text = "검색";
            btn_search.UseVisualStyleBackColor = true;
            btn_search.Click += btn_search_Click;
            // 
            // lbDday
            // 
            lbDday.AutoSize = true;
            lbDday.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbDday.Location = new Point(12, 42);
            lbDday.Name = "lbDday";
            lbDday.Size = new Size(97, 21);
            lbDday.TabIndex = 8;
            lbDday.Text = "D-Day 없음";
            // 
            // mainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1076, 603);
            Controls.Add(lbDday);
            Controls.Add(lbmain_title);
            Controls.Add(tb_search);
            Controls.Add(btn_search);
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
        private Button btn_search;
        private ToolStripMenuItem tool_day;
        private ToolStripMenuItem menuThemeLight;
        private ToolStripMenuItem menuThemeDark;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuThemeBlossom;
        private ToolStripMenuItem menuThemeMint;
        private ToolStripMenuItem menuThemeLavender;
        private ToolStripMenuItem menuThemeCozy;
        private Label lbDday;
        private ToolStripMenuItem 계정ToolStripMenuItem;
        private ToolStripMenuItem 마이페이지ToolStripMenuItem;
        private ToolStripMenuItem 로그인ToolStripMenuItem;
        private ToolStripMenuItem 맑은고딕ToolStripMenuItem;
        private ToolStripMenuItem 바탕체ToolStripMenuItem;
        private ToolStripMenuItem 돋움ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem 한컴말랑말랑ToolStripMenuItem;
        private ToolStripMenuItem 훈민정음가로쓰기ToolStripMenuItem;
        private ToolStripMenuItem 한컴산뜻돋움ToolStripMenuItem;
        private ToolStripMenuItem 디데이ToolStripMenuItem;
    }
}
