namespace calendar4;

internal sealed class ScheduleColorSettingsDialog : Form
{
    private readonly Dictionary<ScheduleCategory, Panel> previews = new();
    private readonly Dictionary<ScheduleCategory, Color> colors = new();

    public ScheduleColorSettingsDialog()
    {
        Text = "시간표 카테고리 색상 설정";
        ClientSize = new Size(390, 250);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;

        colors[ScheduleCategory.Major] = ScheduleColorService.GetAccentColor(ScheduleCategory.Major);
        colors[ScheduleCategory.General] = ScheduleColorService.GetAccentColor(ScheduleCategory.General);
        colors[ScheduleCategory.Other] = ScheduleColorService.GetAccentColor(ScheduleCategory.Other);

        AddColorRow(ScheduleCategory.Major, 28);
        AddColorRow(ScheduleCategory.General, 78);
        AddColorRow(ScheduleCategory.Other, 128);

        var resetButton = new Button
        {
            Text = "기본값 복원",
            Location = new Point(24, 190),
            Size = new Size(105, 34)
        };
        resetButton.Click += (_, _) => ResetColors();

        var saveButton = new Button
        {
            Text = "저장",
            Location = new Point(176, 190),
            Size = new Size(88, 34),
            BackColor = Color.FromArgb(79, 107, 237),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        saveButton.Click += (_, _) => SaveColors();

        var cancelButton = new Button
        {
            Text = "취소",
            Location = new Point(274, 190),
            Size = new Size(88, 34),
            DialogResult = DialogResult.Cancel
        };

        Controls.AddRange(new Control[] { resetButton, saveButton, cancelButton });
        AcceptButton = saveButton;
        CancelButton = cancelButton;
    }

    private void AddColorRow(ScheduleCategory category, int top)
    {
        var label = new Label
        {
            Text = ScheduleColorService.GetCategoryName(category),
            Location = new Point(28, top + 7),
            AutoSize = true
        };
        var preview = new Panel
        {
            Location = new Point(105, top),
            Size = new Size(145, 30),
            BackColor = colors[category],
            BorderStyle = BorderStyle.FixedSingle
        };
        var chooseButton = new Button
        {
            Text = "색상 선택",
            Location = new Point(270, top),
            Size = new Size(92, 30)
        };
        chooseButton.Click += (_, _) => ChooseColor(category, preview);

        previews[category] = preview;
        Controls.AddRange(new Control[] { label, preview, chooseButton });
    }

    private void ChooseColor(ScheduleCategory category, Panel preview)
    {
        using var picker = new ColorDialog { Color = colors[category], FullOpen = true };
        if (picker.ShowDialog(this) != DialogResult.OK)
            return;

        colors[category] = picker.Color;
        preview.BackColor = picker.Color;
    }

    private void ResetColors()
    {
        var defaults = new ScheduleColorSettings();
        colors[ScheduleCategory.Major] = Color.FromArgb(defaults.MajorColorArgb);
        colors[ScheduleCategory.General] = Color.FromArgb(defaults.GeneralColorArgb);
        colors[ScheduleCategory.Other] = Color.FromArgb(defaults.OtherColorArgb);
        RefreshPreviews();
    }

    private void SaveColors()
    {
        ScheduleColorService.Save(new ScheduleColorSettings
        {
            MajorColorArgb = colors[ScheduleCategory.Major].ToArgb(),
            GeneralColorArgb = colors[ScheduleCategory.General].ToArgb(),
            OtherColorArgb = colors[ScheduleCategory.Other].ToArgb()
        });
        DialogResult = DialogResult.OK;
        Close();
    }

    private void RefreshPreviews()
    {
        foreach (var item in previews)
            item.Value.BackColor = colors[item.Key];
    }
}
