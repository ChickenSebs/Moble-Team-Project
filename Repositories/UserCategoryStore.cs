using System.Text.Json;

namespace calendar4;

public sealed class UserCategoryStore
{
    public const string HomeId = "home";
    public const string WorkId = "work";

    private readonly string filePath;
    private List<UserCategory> categories;

    public UserCategoryStore(string fileName)
    {
        filePath = Path.Combine(Application.LocalUserAppDataPath, fileName);
        categories = Load();
    }

    public event EventHandler? Changed;
    public IReadOnlyList<UserCategory> Categories => categories;

    public UserCategory Get(string? id) =>
        categories.FirstOrDefault(item => item.Id == id)
        ?? categories.First(item => item.Id == HomeId);

    public Color GetAccentColor(string? id) => Color.FromArgb(Get(id).ColorArgb);

    public Color GetScheduleAccentColor(string? id, int? customColorArgb) =>
        customColorArgb.HasValue ? Color.FromArgb(customColorArgb.Value) : GetAccentColor(id);

    public Color GetBackgroundColor(string? id) => BlendWithWhite(GetAccentColor(id));

    public Color GetScheduleBackgroundColor(string? id, int? customColorArgb) =>
        BlendWithWhite(GetScheduleAccentColor(id, customColorArgb));

    public void Save(IEnumerable<UserCategory> updatedCategories)
    {
        categories = EnsureDefaults(updatedCategories.Select(Copy).ToList());
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(
            filePath,
            JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true }));
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private List<UserCategory> Load()
    {
        try
        {
            if (File.Exists(filePath))
            {
                var loaded = JsonSerializer.Deserialize<List<UserCategory>>(File.ReadAllText(filePath));
                if (loaded is not null)
                    return EnsureDefaults(loaded);
            }
        }
        catch (IOException)
        {
        }
        catch (JsonException)
        {
        }

        return CreateDefaults();
    }

    private static List<UserCategory> EnsureDefaults(List<UserCategory> items)
    {
        var defaults = CreateDefaults();
        if (items.All(item => item.Id != HomeId))
            items.Insert(0, defaults[0]);
        if (items.All(item => item.Id != WorkId))
            items.Insert(Math.Min(1, items.Count), defaults[1]);

        foreach (var item in items)
        {
            item.IsDefault = item.Id is HomeId or WorkId;
            if (item.Id == HomeId)
                item.Name = "집";
            else if (item.Id == WorkId)
                item.Name = "직장";
            else if (string.IsNullOrWhiteSpace(item.Name))
                item.Name = "새 카테고리";
        }

        return items.GroupBy(item => item.Id).Select(group => group.First()).ToList();
    }

    private static List<UserCategory> CreateDefaults() => new()
    {
        new UserCategory
        {
            Id = HomeId,
            Name = "집",
            ColorArgb = Color.FromArgb(79, 107, 237).ToArgb(),
            IsDefault = true
        },
        new UserCategory
        {
            Id = WorkId,
            Name = "직장",
            ColorArgb = Color.FromArgb(46, 157, 103).ToArgb(),
            IsDefault = true
        }
    };

    private static UserCategory Copy(UserCategory item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        ColorArgb = item.ColorArgb,
        IsDefault = item.IsDefault
    };

    private static Color BlendWithWhite(Color color) => Color.FromArgb(
        (int)(color.R * 0.16f + 255 * 0.84f),
        (int)(color.G * 0.16f + 255 * 0.84f),
        (int)(color.B * 0.16f + 255 * 0.84f));
}
