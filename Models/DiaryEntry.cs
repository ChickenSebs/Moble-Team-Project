namespace calendar4;

public sealed class DiaryEntry
{
    // DB의 diary_id
    public int? DiaryId { get; set; }

    public string DateStr { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}