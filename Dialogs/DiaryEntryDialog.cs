namespace calendar4;

internal sealed class DiaryEntryDialog : Form
{
    private readonly TextBox titleBox;
    private readonly TextBox contentBox;

    public DiaryEntryDialog(DateTime date, DiaryEntry? entry)
    {
        Text = $"{date:yyyy년 MM월 dd일} 다이어리 작성";
        ClientSize = new Size(500, 420);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var titleLabel = new Label
        {
            Left = 15,
            Top = 15,
            Text = "제목:",
            AutoSize = true,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold)
        };
        titleBox = new TextBox
        {
            Left = 60,
            Top = 12,
            Width = 405,
            Text = entry?.Title ?? string.Empty,
            Font = new Font("맑은 고딕", 10)
        };
        var contentLabel = new Label
        {
            Left = 15,
            Top = 48,
            Text = "본문:",
            AutoSize = true,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold)
        };
        contentBox = new TextBox
        {
            Left = 15,
            Top = 75,
            Width = 450,
            Height = 280,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Text = entry?.Content ?? string.Empty,
            Font = new Font("맑은 고딕", 10)
        };
        var saveButton = new Button
        {
            Text = "저장",
            Left = 275,
            Top = 365,
            Width = 90,
            Height = 32,
            DialogResult = DialogResult.OK,
            BackColor = Color.LightSkyBlue,
            FlatStyle = FlatStyle.Flat
        };
        var deleteButton = new Button
        {
            Text = "삭제",
            Left = 375,
            Top = 365,
            Width = 90,
            Height = 32,
            DialogResult = DialogResult.Yes,
            FlatStyle = FlatStyle.Flat,
            Visible = entry is not null
        };

        Controls.AddRange(new Control[]
        {
            titleLabel, titleBox, contentLabel, contentBox, saveButton, deleteButton
        });
        AcceptButton = saveButton;
    }

    public string DiaryTitle => titleBox.Text;
    public string DiaryContent => contentBox.Text;
    public bool IsEmpty => string.IsNullOrWhiteSpace(DiaryTitle) && string.IsNullOrWhiteSpace(DiaryContent);
}
