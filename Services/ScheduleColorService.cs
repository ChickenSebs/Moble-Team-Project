namespace calendar4;

public static class ScheduleColorService
{
    private static readonly ScheduleColorRepository Repository = new();

    public static ScheduleColorSettings Current { get; private set; } = Repository.Load();
    public static event EventHandler? SettingsChanged;

    public static Color GetAccentColor(ScheduleCategory category)
    {
        var argb = category switch
        {
            ScheduleCategory.Major => Current.MajorColorArgb,
            ScheduleCategory.General => Current.GeneralColorArgb,
            _ => Current.OtherColorArgb
        };
        return Color.FromArgb(argb);
    }

    public static Color GetScheduleAccentColor(ScheduleCategory category, int? customColorArgb) =>
        customColorArgb.HasValue ? Color.FromArgb(customColorArgb.Value) : GetAccentColor(category);

    public static Color GetScheduleBackgroundColor(ScheduleCategory category, int? customColorArgb) =>
        BlendWithWhite(GetScheduleAccentColor(category, customColorArgb), 0.16f);

    public static string GetCategoryName(ScheduleCategory category) => category switch
    {
        ScheduleCategory.Major => "전공",
        ScheduleCategory.General => "교양",
        _ => "기타"
    };

    public static void Save(ScheduleColorSettings settings)
    {
        Repository.Save(settings);
        Current = settings;
        SettingsChanged?.Invoke(null, EventArgs.Empty);
    }

    private static Color BlendWithWhite(Color color, float colorRatio)
    {
        var whiteRatio = 1f - colorRatio;
        return Color.FromArgb(
            (int)(color.R * colorRatio + 255 * whiteRatio),
            (int)(color.G * colorRatio + 255 * whiteRatio),
            (int)(color.B * colorRatio + 255 * whiteRatio));
    }
}
