namespace calendar4;

public static class CalendarTitleFormatter
{
    public static string Format(DateTime date, CalendarControl.CalendarViewMode mode)
    {
        if (mode == CalendarControl.CalendarViewMode.Day)
            return date.ToString("M월 d일");
        if (mode == CalendarControl.CalendarViewMode.Month)
            return date.ToString("yyyy년 M월");

        var start = date.Date.AddDays(-(int)date.DayOfWeek);
        return $"{start:M.d} ~ {start.AddDays(6):M.d}";
    }
}
