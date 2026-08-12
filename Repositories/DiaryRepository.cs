using System.Text.Json;

namespace calendar4;

public sealed class DiaryRepository
{
    private readonly string filePath;

    public DiaryRepository(string filePath = "saved_diaries.json")
    {
        this.filePath = filePath;
    }

    public Dictionary<string, DiaryEntry> Load()
    {
        if (!File.Exists(filePath))
            return new Dictionary<string, DiaryEntry>();

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, DiaryEntry>>(
                File.ReadAllText(filePath)) ?? new Dictionary<string, DiaryEntry>();
        }
        catch (IOException)
        {
            return new Dictionary<string, DiaryEntry>();
        }
        catch (JsonException)
        {
            return new Dictionary<string, DiaryEntry>();
        }
    }

    public void Save(IReadOnlyDictionary<string, DiaryEntry> diaries)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(diaries));
    }
}
