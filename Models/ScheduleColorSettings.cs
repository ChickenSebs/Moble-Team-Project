namespace calendar4;

public sealed class ScheduleColorSettings
{
    public int MajorColorArgb { get; set; } = Color.FromArgb(79, 107, 237).ToArgb();
    public int GeneralColorArgb { get; set; } = Color.FromArgb(46, 157, 103).ToArgb();
    public int OtherColorArgb { get; set; } = Color.FromArgb(244, 160, 72).ToArgb();
}
