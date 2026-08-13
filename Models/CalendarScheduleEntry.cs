namespace calendar4;

public sealed class CalendarScheduleEntry
{
    // 프로그램 내부에서 사용하는 ID
    public Guid Id { get; set; } = Guid.NewGuid();

    // MySQL user_cal.cal_id
    public int? CalId { get; set; }

    public string Text { get; set; } = string.Empty;
    public int StartHour { get; set; } = 9;
    public int EndHour { get; set; } = 10;
    public string CategoryId { get; set; } = UserCategoryStore.HomeId;
    public int? CustomColorArgb { get; set; }
    public bool IsHighPriority { get; set; }
    public int NotificationOffset { get; set; }

    public CalendarScheduleEntry Copy() => new()
    {
        Id = Id,
        CalId = CalId,
        Text = Text,
        StartHour = StartHour,
        EndHour = EndHour,
        CategoryId = CategoryId,
        CustomColorArgb = CustomColorArgb,
        IsHighPriority = IsHighPriority,
        NotificationOffset = NotificationOffset
    };
}