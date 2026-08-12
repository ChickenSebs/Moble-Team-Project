using System.Text.Json;

namespace calendar4;

public sealed class PlannerRepository
{
    private readonly string filePath;

    public PlannerRepository(string filePath = "saved_planners.json")
    {
        this.filePath = filePath;
    }

    public Dictionary<string, PlannerData> Load()
    {
        if (!File.Exists(filePath))
            return new Dictionary<string, PlannerData>();

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, PlannerData>>(
                File.ReadAllText(filePath)) ?? new Dictionary<string, PlannerData>();
        }
        catch (IOException)
        {
            return new Dictionary<string, PlannerData>();
        }
        catch (JsonException)
        {
            return new Dictionary<string, PlannerData>();
        }
    }

    public void Save(IReadOnlyDictionary<string, PlannerData> planners)
    {
        File.WriteAllText(
            filePath,
            JsonSerializer.Serialize(planners, new JsonSerializerOptions { WriteIndented = true }));
    }
}
