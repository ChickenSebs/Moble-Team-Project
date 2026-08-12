namespace calendar4;

public sealed class UserCategory
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = string.Empty;
    public int ColorArgb { get; set; }
    public bool IsDefault { get; set; }

    public override string ToString() => Name;
}
