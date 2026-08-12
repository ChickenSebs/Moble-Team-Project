using System.Text.Json;

namespace calendar4;

public sealed class TabRepository
{
    private readonly string filePath;

    public TabRepository(string filePath = "saved_tabs.json")
    {
        this.filePath = filePath;
    }

    public List<TabData> Load()
    {
        if (!File.Exists(filePath))
            return new List<TabData>();

        try
        {
            return JsonSerializer.Deserialize<List<TabData>>(File.ReadAllText(filePath))
                ?? new List<TabData>();
        }
        catch (IOException)
        {
            return new List<TabData>();
        }
        catch (JsonException)
        {
            return new List<TabData>();
        }
    }

    public void Save(IReadOnlyCollection<TabData> tabs)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(tabs));
    }
}
