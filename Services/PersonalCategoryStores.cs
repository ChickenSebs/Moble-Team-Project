namespace calendar4;

public static class PersonalCategoryStores
{
    public static UserCategoryStore Calendar { get; } = new("calendar_categories.json");
}
