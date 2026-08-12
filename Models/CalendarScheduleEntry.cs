namespace calendar4;

public sealed class CalendarScheduleEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
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
        Text = Text,
        StartHour = StartHour,
        EndHour = EndHour,
        CategoryId = CategoryId,
        CustomColorArgb = CustomColorArgb,
        IsHighPriority = IsHighPriority,
        NotificationOffset = NotificationOffset
    };
}
