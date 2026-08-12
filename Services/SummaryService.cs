using System.Text;

namespace calendar4;

public sealed class SummaryService
{
    public string CreateDiarySummary(IReadOnlyDictionary<string, DiaryEntry> diaries)
    {
        var text = new StringBuilder("📝 [다이어리 목록]\n\n");
        foreach (var entry in diaries.OrderBy(item => item.Key).Select(item => item.Value))
            text.AppendLine($"• {entry.DateStr} : {entry.Title}");
        return text.ToString();
    }

    public string CreateCalendarSummary(
        IReadOnlyDictionary<DateTime, List<CalendarScheduleEntry>> schedules)
    {
        var text = new StringBuilder("📋 [전체 일정 요약]\n\n");
        foreach (var dateSchedules in schedules.OrderBy(item => item.Key))
        {
            foreach (var entry in dateSchedules.Value
                .OrderBy(item => item.StartHour)
                .ThenBy(item => item.EndHour))
            {
                var categoryName = PersonalCategoryStores.Calendar.Get(entry.CategoryId).Name;
                text.AppendLine(
                    $"• {dateSchedules.Key:yyyy-MM-dd} " +
                    $"({entry.StartHour:00}:00~{entry.EndHour:00}:00)");
                text.AppendLine($"  [{categoryName}] {entry.Text}");
                text.AppendLine();
            }
        }
        return text.ToString();
    }
}
