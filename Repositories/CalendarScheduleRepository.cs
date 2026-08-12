using System.Text.Json;

namespace calendar4;

public sealed class CalendarScheduleRepository
{
    private readonly string filePath;

    public CalendarScheduleRepository(string filePath = "saved_schedules.json")
    {
        this.filePath = filePath;
    }

    public Dictionary<DateTime, List<CalendarScheduleEntry>> Load()
    {
        var result = new Dictionary<DateTime, List<CalendarScheduleEntry>>();
        if (!File.Exists(filePath))
            return result;

        try
        {
            var json = File.ReadAllText(filePath);
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
                return result;

            var migratedLegacyData = false;
            foreach (var property in document.RootElement.EnumerateObject())
            {
                if (!DateTime.TryParse(property.Name, out var date))
                    continue;

                List<CalendarScheduleEntry>? entries = null;
                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    entries = property.Value.Deserialize<List<CalendarScheduleEntry>>();
                }
                else if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    var legacyEntry = property.Value.Deserialize<CalendarScheduleEntry>();
                    if (legacyEntry is not null)
                    {
                        entries = new List<CalendarScheduleEntry> { legacyEntry };
                        migratedLegacyData = true;
                    }
                }

                if (entries is null)
                    continue;

                foreach (var entry in entries)
                {
                    if (entry.Id == Guid.Empty)
                        entry.Id = Guid.NewGuid();
                    if (string.IsNullOrWhiteSpace(entry.CategoryId))
                        entry.CategoryId = UserCategoryStore.HomeId;
                }

                var validEntries = entries
                    .Where(item => !string.IsNullOrWhiteSpace(item.Text))
                    .OrderBy(item => item.StartHour)
                    .ThenBy(item => item.EndHour)
                    .ToList();
                if (validEntries.Count > 0)
                    result[date.Date] = validEntries;
            }

            if (migratedLegacyData)
            {
                BackupLegacyFile();
                Save(result);
            }
        }
        catch (IOException)
        {
        }
        catch (JsonException)
        {
        }

        return result;
    }

    public void Save(IReadOnlyDictionary<DateTime, List<CalendarScheduleEntry>> schedules)
    {
        var serializable = schedules.ToDictionary(
            item => item.Key.ToString("yyyy-MM-dd"),
            item => item.Value);
        File.WriteAllText(filePath, JsonSerializer.Serialize(serializable));
    }

    private void BackupLegacyFile()
    {
        var backupPath = Path.Combine(
            Path.GetDirectoryName(Path.GetFullPath(filePath))!,
            "saved_schedules.backup.json");
        if (!File.Exists(backupPath))
            File.Copy(filePath, backupPath);
    }
}
