namespace calendar4;

internal sealed class UserCategoryEditorDialog : Form
{
    private readonly UserCategory? existing;
    private readonly TextBox nameBox = new();
    private readonly Panel preview = new();
    private Color color;

    public UserCategoryEditorDialog(UserCategory? category = null)
    {
        existing = category;
        color = category is null ? Color.FromArgb(244, 160, 72) : Color.FromArgb(category.ColorArgb);
        Text = category is null ? "카테고리 추가" : "카테고리 수정";
        ClientSize = new Size(350, 180);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        Controls.Add(new Label { Text = "이름", Location = new Point(24, 32), AutoSize = true });
        nameBox.Location = new Point(84, 28);
        nameBox.Size = new Size(238, 23);
        nameBox.Text = category?.Name ?? string.Empty;
        nameBox.ReadOnly = category?.IsDefault ?? false;

        Controls.Add(new Label { Text = "색상", Location = new Point(24, 76), AutoSize = true });
        preview.Location = new Point(84, 68);
        preview.Size = new Size(90, 30);
        preview.BackColor = color;
        preview.BorderStyle = BorderStyle.FixedSingle;

        var chooseButton = new Button
        {
            Text = "색상 선택",
            Location = new Point(184, 68),
            Size = new Size(98, 30)
        };
        chooseButton.Click += (_, _) => ChooseColor();

        var saveButton = new Button
        {
            Text = "확인",
            Location = new Point(166, 124),
            Size = new Size(76, 32)
        };
        saveButton.Click += (_, _) => SaveCategory();
        var cancelButton = new Button
        {
            Text = "취소",
            Location = new Point(248, 124),
            Size = new Size(76, 32),
            DialogResult = DialogResult.Cancel
        };

        Controls.AddRange(new Control[] { nameBox, preview, chooseButton, saveButton, cancelButton });
        AcceptButton = saveButton;
        CancelButton = cancelButton;
    }

    public UserCategory Category { get; private set; } = null!;

    private void ChooseColor()
    {
        using var picker = new ColorDialog { Color = color, FullOpen = true };
        if (picker.ShowDialog(this) == DialogResult.OK)
        {
            color = picker.Color;
            preview.BackColor = color;
        }
    }

    private void SaveCategory()
    {
        var name = nameBox.Text.Trim();
        if (name.Length == 0)
        {
            MessageBox.Show("카테고리 이름을 입력해 주세요.", "입력 확인");
            nameBox.Focus();
            return;
        }

        Category = new UserCategory
        {
            Id = existing?.Id ?? Guid.NewGuid().ToString("N"),
            Name = name,
            ColorArgb = color.ToArgb(),
            IsDefault = existing?.IsDefault ?? false
        };
        DialogResult = DialogResult.OK;
        Close();
    }
}
