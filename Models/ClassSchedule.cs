namespace calendar4;

public sealed class ClassSchedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SubjectName { get; set; } = string.Empty;
    public string Classroom { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public int StartHour { get; set; }
    public int EndHour { get; set; }
    public ScheduleCategory Category { get; set; }
    public int? CustomColorArgb { get; set; }
}

public enum ScheduleCategory
{
    Major,
    General,
    Other
}
