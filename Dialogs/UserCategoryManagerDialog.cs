namespace calendar4;

internal sealed class UserCategoryManagerDialog : Form
{
    private readonly UserCategoryStore store;
    private readonly List<UserCategory> categories;
    private readonly ListBox list = new();

    public UserCategoryManagerDialog(string title, UserCategoryStore store)
    {
        this.store = store;
        categories = store.Categories.Select(CopyCategory).ToList();

        Text = title;
        ClientSize = new Size(430, 330);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        list.Location = new Point(22, 22);
        list.Size = new Size(386, 210);
        list.DrawMode = DrawMode.OwnerDrawFixed;
        list.ItemHeight = 34;
        list.DrawItem += DrawCategory;
        list.DoubleClick += (_, _) => EditSelected();

        var addButton = CreateButton("추가", 22, AddCategory);
        var editButton = CreateButton("수정", 106, EditSelected);
        var deleteButton = CreateButton("삭제", 190, DeleteSelected);
        var saveButton = new Button
        {
            Text = "저장",
            Location = new Point(250, 274),
            Size = new Size(76, 34),
            BackColor = Color.FromArgb(79, 107, 237),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        saveButton.Click += (_, _) =>
        {
            store.Save(categories);
            DialogResult = DialogResult.OK;
            Close();
        };
        var cancelButton = new Button
        {
            Text = "취소",
            Location = new Point(332, 274),
            Size = new Size(76, 34),
            DialogResult = DialogResult.Cancel
        };

        Controls.AddRange(new Control[] { list, addButton, editButton, deleteButton, saveButton, cancelButton });
        AcceptButton = saveButton;
        CancelButton = cancelButton;
        RefreshList();
    }

    private static UserCategory CopyCategory(UserCategory item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        ColorArgb = item.ColorArgb,
        IsDefault = item.IsDefault
    };

    private Button CreateButton(string text, int left, Action action)
    {
        var button = new Button
        {
            Text = text,
            Location = new Point(left, 242),
            Size = new Size(76, 30)
        };
        button.Click += (_, _) => action();
        return button;
    }

    private void AddCategory()
    {
        using var editor = new UserCategoryEditorDialog();
        if (editor.ShowDialog(this) != DialogResult.OK)
            return;

        if (HasDuplicateName(editor.Category.Name))
        {
            MessageBox.Show("같은 이름의 카테고리가 이미 있습니다.", "카테고리 확인");
            return;
        }

        categories.Add(editor.Category);
        RefreshList(editor.Category.Id);
    }

    private void EditSelected()
    {
        if (list.SelectedItem is not UserCategory selected)
            return;

        using var editor = new UserCategoryEditorDialog(selected);
        if (editor.ShowDialog(this) != DialogResult.OK)
            return;

        if (HasDuplicateName(editor.Category.Name, selected.Id))
        {
            MessageBox.Show("같은 이름의 카테고리가 이미 있습니다.", "카테고리 확인");
            return;
        }

        var index = categories.FindIndex(item => item.Id == selected.Id);
        if (index >= 0)
            categories[index] = editor.Category;
        RefreshList(editor.Category.Id);
    }

    private bool HasDuplicateName(string name, string? exceptId = null) =>
        categories.Any(item => item.Id != exceptId &&
            string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));

    private void DeleteSelected()
    {
        if (list.SelectedItem is not UserCategory selected)
            return;

        if (selected.IsDefault)
        {
            MessageBox.Show("기본 카테고리인 집과 직장은 삭제할 수 없습니다.", "카테고리 삭제");
            return;
        }

        categories.RemoveAll(item => item.Id == selected.Id);
        RefreshList();
    }

    private void RefreshList(string? selectedId = null)
    {
        list.BeginUpdate();
        list.Items.Clear();
        foreach (var category in categories)
            list.Items.Add(category);
        list.EndUpdate();

        if (selectedId is null)
            return;

        for (var i = 0; i < list.Items.Count; i++)
        {
            if (((UserCategory)list.Items[i]).Id != selectedId)
                continue;

            list.SelectedIndex = i;
            break;
        }
    }

    private void DrawCategory(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0)
            return;

        e.DrawBackground();
        var category = (UserCategory)list.Items[e.Index];
        using var brush = new SolidBrush(Color.FromArgb(category.ColorArgb));
        e.Graphics.FillRectangle(brush, e.Bounds.Left + 8, e.Bounds.Top + 7, 18, 18);
        TextRenderer.DrawText(
            e.Graphics,
            category.Name + (category.IsDefault ? "  (기본)" : string.Empty),
            list.Font,
            new Rectangle(e.Bounds.Left + 36, e.Bounds.Top, e.Bounds.Width - 40, e.Bounds.Height),
            e.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }
}
