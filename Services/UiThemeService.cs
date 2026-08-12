namespace calendar4;

public static class UiThemeService
{
    public static void ApplyFont(Control parent, Font font)
    {
        foreach (Control control in parent.Controls)
        {
            control.Font = font;
            if (control.HasChildren)
                ApplyFont(control, font);
        }
    }
}
