namespace calendar4;

public sealed class PlannerData
{
    public List<PlannerTask> Tasks { get; set; } = new();
    public List<PlannerTimeSlot> TimeSlots { get; set; } = new();
}

public sealed class PlannerTask
{
    public string Name { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public sealed class PlannerTimeSlot
{
    public int Hour { get; set; }

    public int StartMinute { get; set; }

    public int EndMinute { get; set; }

    public string TaskName { get; set; } = string.Empty;

    public int R { get; set; }

    public int G { get; set; }

    public int B { get; set; }
}
