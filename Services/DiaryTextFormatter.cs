namespace calendar4;

public static class DiaryTextFormatter
{
    public static string GetTitle(DiaryEntry diary) =>
        string.IsNullOrWhiteSpace(diary.Title) ? "[제목 없음]" : diary.Title;

    public static string GetPreview(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return "본문 없음";

        var singleLine = content.Replace("\r", " ").Replace("\n", " ").Trim();
        return singleLine.Length > 60 ? $"{singleLine[..60]}…" : singleLine;
    }
}
