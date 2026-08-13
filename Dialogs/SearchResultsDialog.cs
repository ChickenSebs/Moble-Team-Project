namespace calendar4;

internal sealed record SearchResultItem(
    DateTime Date,
    string Title,
    string Detail)
{
    public override string ToString() => $"{Title}\n{Detail}";
}

internal sealed class SearchResultsDialog : Form
{
    private readonly ListBox resultList = new();

    public SearchResultItem? SelectedResult =>
        resultList.SelectedItem as SearchResultItem;

    public SearchResultsDialog(
        string searchScope,
        string keyword,
        IReadOnlyCollection<SearchResultItem> results)
    {
        Text = $"{searchScope} 검색 결과";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(620, 430);
        MinimumSize = new Size(520, 360);
        Font = new Font("맑은 고딕", 9F);

        var header = new Label
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(12, 10, 12, 6),
            Text = $"‘{keyword}’ 검색 결과  {results.Count}개",
            Font = new Font("맑은 고딕", 11F, FontStyle.Bold)
        };

        resultList.Dock = DockStyle.Fill;
        resultList.IntegralHeight = false;
        resultList.HorizontalScrollbar = true;
        resultList.ItemHeight = 48;
        resultList.DrawMode = DrawMode.OwnerDrawFixed;
        resultList.DrawItem += DrawResultItem;
        resultList.DoubleClick += (_, _) => SelectCurrentResult();
        resultList.Items.AddRange(results.Cast<object>().ToArray());
        if (resultList.Items.Count > 0)
            resultList.SelectedIndex = 0;

        var moveButton = new Button
        {
            Text = "일간 보기로 이동",
            AutoSize = true,
            DialogResult = DialogResult.None
        };
        moveButton.Click += (_, _) => SelectCurrentResult();

        var cancelButton = new Button
        {
            Text = "닫기",
            AutoSize = true,
            DialogResult = DialogResult.Cancel
        };

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8),
            WrapContents = false
        };
        buttonPanel.Controls.Add(cancelButton);
        buttonPanel.Controls.Add(moveButton);

        Controls.Add(resultList);
        Controls.Add(buttonPanel);
        Controls.Add(header);

        AcceptButton = moveButton;
        CancelButton = cancelButton;
    }

    private void SelectCurrentResult()
    {
        if (SelectedResult is null)
            return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void DrawResultItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= resultList.Items.Count)
            return;

        e.DrawBackground();
        if (resultList.Items[e.Index] is not SearchResultItem item)
            return;

        Color textColor = (e.State & DrawItemState.Selected) != 0
            ? SystemColors.HighlightText
            : SystemColors.ControlText;

        Rectangle titleBounds = new(
            e.Bounds.X + 8,
            e.Bounds.Y + 4,
            e.Bounds.Width - 16,
            20);
        Rectangle detailBounds = new(
            e.Bounds.X + 8,
            e.Bounds.Y + 25,
            e.Bounds.Width - 16,
            18);

        using var titleFont = new Font(Font, FontStyle.Bold);
        TextRenderer.DrawText(
            e.Graphics,
            item.Title,
            titleFont,
            titleBounds,
            textColor,
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        TextRenderer.DrawText(
            e.Graphics,
            item.Detail,
            Font,
            detailBounds,
            textColor,
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

        e.DrawFocusRectangle();
    }
}
