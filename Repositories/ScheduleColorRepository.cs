using System.Text.Json;

namespace calendar4;

public sealed class ScheduleColorRepository
{
    private readonly string filePath = Path.Combine(
        Application.LocalUserAppDataPath,
        "schedule_colors.json");

    public ScheduleColorSettings Load()
    {
        try
        {
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<ScheduleColorSettings>(
                    File.ReadAllText(filePath)) ?? new ScheduleColorSettings();
            }
        }
        catch (IOException)
        {
        }
        catch (JsonException)
        {
        }

        return new ScheduleColorSettings();
    }

    public void Save(ScheduleColorSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(
            filePath,
            JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
    }
}
