namespace calendar4;

public sealed class TimetableScheduleService
{
    public bool HasTimeConflict(
        IEnumerable<ClassSchedule> schedules,
        ClassSchedule candidate,
        Guid? excludedId = null)
    {
        return schedules.Any(item =>
            item.Id != excludedId &&
            item.Day == candidate.Day &&
            candidate.StartHour < item.EndHour &&
            candidate.EndHour > item.StartHour);
    }

    public bool IsValid(ClassSchedule schedule)
    {
        return schedule.Day is >= DayOfWeek.Monday and <= DayOfWeek.Friday &&
               schedule.StartHour >= 9 &&
               schedule.EndHour <= 18 &&
               schedule.StartHour < schedule.EndHour &&
               !string.IsNullOrWhiteSpace(schedule.SubjectName);
    }
}
