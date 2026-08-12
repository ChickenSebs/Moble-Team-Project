namespace calendar4;

public partial class ddayalarm : Form
{
    private readonly System.Windows.Forms.Timer autoCloseTimer;

    public ddayalarm(string title, string message)
    {
        InitializeComponent();
        titleLabel.Text = title;
        messageLabel.Text = message;

        autoCloseTimer = new System.Windows.Forms.Timer(components)
        {
            Interval = 5_000
        };
        autoCloseTimer.Tick += (_, _) => Close();
        autoCloseTimer.Start();
    }

    protected override bool ShowWithoutActivation => true;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Rectangle workingArea = Screen.FromPoint(Cursor.Position).WorkingArea;
        int stackIndex = Application.OpenForms
            .OfType<ddayalarm>()
            .Count(form => !ReferenceEquals(form, this));
        Location = new Point(
            workingArea.Right - Width - 12,
            workingArea.Bottom - Height - 12 - (stackIndex * (Height + 8)));
    }
}
