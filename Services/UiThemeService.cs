using System.Drawing;
using System.Windows.Forms;

namespace calendar4
{
    public enum AppTheme
    {
        Light,
        Dark,
        Blossom,
        Mint,
        Lavender,
        Cozy
    }

    public static class UiThemeService
    {
        // ���� ���� ���� �׸�
        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Light;


        // =========================================================
        // �۲� ����
        // ���߿� ���� / �����̾� �۲� ��ɿ��� ���
        // =========================================================
        public static void ApplyFont(Control parent, Font font)
        {
            parent.Font = font;

            foreach (Control control in parent.Controls)
            {
                ApplyFont(control, font);
            }
        }


        // =========================================================
        // ���� �׸� ����
        // =========================================================
        public static void SetTheme(AppTheme theme)
        {
            CurrentTheme = theme;
        }


        // =========================================================
        // �����̾� ���� �׸����� Ȯ��
        // =========================================================
        public static bool IsPremiumTheme(AppTheme theme)
        {
            return theme == AppTheme.Blossom ||
                   theme == AppTheme.Mint ||
                   theme == AppTheme.Lavender ||
                   theme == AppTheme.Cozy;
        }


        // =========================================================
        // ��ü ����
        // =========================================================
        public static Color BackgroundColor
        {
            get
            {
                return CurrentTheme switch
                {
                    AppTheme.Dark =>
                        Color.FromArgb(30, 30, 30),

                    AppTheme.Blossom =>
                        Color.FromArgb(255, 245, 248),

                    AppTheme.Mint =>
                        Color.FromArgb(242, 252, 248),

                    AppTheme.Lavender =>
                        Color.FromArgb(248, 245, 255),

                    AppTheme.Cozy =>
                        Color.FromArgb(250, 246, 238),

                    _ =>
                        Color.FromArgb(248, 248, 248)
                };
            }
        }


        // =========================================================
        // �г� / ī�� ����
        // =========================================================
        public static Color SurfaceColor
        {
            get
            {
                return CurrentTheme switch
                {
                    AppTheme.Dark =>
                        Color.FromArgb(45, 45, 45),

                    AppTheme.Blossom =>
                        Color.FromArgb(255, 230, 238),

                    AppTheme.Mint =>
                        Color.FromArgb(220, 245, 235),

                    AppTheme.Lavender =>
                        Color.FromArgb(235, 228, 250),

                    AppTheme.Cozy =>
                        Color.FromArgb(241, 229, 209),

                    _ =>
                        Color.White
                };
            }
        }


        // =========================================================
        // ��ư / ����Ʈ ����
        // =========================================================
        public static Color PrimaryColor
        {
            get
            {
                return CurrentTheme switch
                {
                    AppTheme.Dark =>
                        Color.FromArgb(110, 100, 170),

                    AppTheme.Blossom =>
                        Color.FromArgb(244, 143, 177),

                    AppTheme.Mint =>
                        Color.FromArgb(102, 187, 160),

                    AppTheme.Lavender =>
                        Color.FromArgb(160, 135, 210),

                    AppTheme.Cozy =>
                        Color.FromArgb(190, 150, 110),

                    _ =>
                        Color.FromArgb(120, 140, 210)
                };
            }
        }


        // =========================================================
        // �⺻ ���ڻ�
        // =========================================================
        public static Color TextColor
        {
            get
            {
                return CurrentTheme switch
                {
                    AppTheme.Dark =>
                        Color.FromArgb(240, 240, 240),

                    AppTheme.Cozy =>
                        Color.FromArgb(80, 65, 55),

                    AppTheme.Blossom =>
                        Color.FromArgb(80, 60, 65),

                    AppTheme.Mint =>
                        Color.FromArgb(55, 75, 70),

                    AppTheme.Lavender =>
                        Color.FromArgb(65, 60, 80),

                    _ =>
                        Color.FromArgb(60, 60, 60)
                };
            }
        }


        // =========================================================
        // TextBox / ComboBox / RichTextBox ���
        // =========================================================
        public static Color InputColor
        {
            get
            {
                return CurrentTheme switch
                {
                    AppTheme.Dark =>
                        Color.FromArgb(55, 55, 55),

                    AppTheme.Blossom =>
                        Color.FromArgb(255, 250, 252),

                    AppTheme.Mint =>
                        Color.FromArgb(250, 255, 253),

                    AppTheme.Lavender =>
                        Color.FromArgb(253, 251, 255),

                    AppTheme.Cozy =>
                        Color.FromArgb(255, 252, 246),

                    _ =>
                        Color.White
                };
            }
        }


        // =========================================================
        // ��ü �׸� ����
        // =========================================================
        public static void ApplyTheme(Control parent)
        {
            // ���� ��Ʈ�Ѻ��� ����
            ApplyControlTheme(parent);

            // ���ο� �ִ� ��� ��Ʈ�ѿ��� ����
            foreach (Control control in parent.Controls)
            {
                ApplyTheme(control);
            }
        }


        // =========================================================
        // ��Ʈ�� ������ ���� ���� ����
        // =========================================================
        private static void ApplyControlTheme(Control control)
        {
            // Form
            if (control is Form)
            {
                control.BackColor = BackgroundColor;
                control.ForeColor = TextColor;
            }

            // TabPage / UserControl
            else if (control is TabPage || control is UserControl)
            {
                control.BackColor = BackgroundColor;
                control.ForeColor = TextColor;
            }

            // Panel / GroupBox
            else if (control is Panel || control is GroupBox)
            {
                control.BackColor = SurfaceColor;
                control.ForeColor = TextColor;
            }

            // Button
            else if (control is Button button)
            {
                button.BackColor = PrimaryColor;
                button.ForeColor = TextColor;

                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.UseVisualStyleBackColor = false;
            }

            // TextBox
            else if (control is TextBox textBox)
            {
                textBox.BackColor = InputColor;
                textBox.ForeColor = TextColor;
            }

            // RichTextBox
            else if (control is RichTextBox richTextBox)
            {
                richTextBox.BackColor = InputColor;
                richTextBox.ForeColor = TextColor;
            }

            // ComboBox
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = InputColor;
                comboBox.ForeColor = TextColor;
            }

            // Label
            else if (control is Label label)
            {
                label.ForeColor = TextColor;
            }

            // TabControl
            else if (control is TabControl tabControl)
            {
                tabControl.BackColor = BackgroundColor;
                tabControl.ForeColor = TextColor;
            }
        }
    }
}