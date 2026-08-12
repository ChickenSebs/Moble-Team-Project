using System.Text.Json;

namespace calendar4;

public sealed class TimetableRepository
{
    private readonly string filePath;

    public TimetableRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public List<ClassSchedule> Load()
    {
        if (!File.Exists(filePath))
            return new List<ClassSchedule>();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<ClassSchedule>>(json) ?? new List<ClassSchedule>();
    }

    public void Save(IReadOnlyCollection<ClassSchedule> schedules)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(
            filePath,
            JsonSerializer.Serialize(schedules, new JsonSerializerOptions { WriteIndented = true }));
    }
}
