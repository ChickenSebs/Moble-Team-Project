using System.Drawing.Drawing2D;

namespace calendar4;

internal sealed class CalendarMonthCellRenderer
{
    private const int HorizontalPadding = 6;
    private const int ScheduleRowHeight = 20;
    private readonly UserCategoryStore categoryStore;

    public CalendarMonthCellRenderer(UserCategoryStore categoryStore)
    {
        this.categoryStore = categoryStore;
    }

    public void Draw(
        Graphics graphics,
        Rectangle bounds,
        DateTime date,
        string? holidayName,
        IReadOnlyList<CalendarScheduleEntry> schedules,
        
        Font baseFont,
        Color dateColor,
        bool isSelected)
    {
        var previousSmoothingMode = graphics.SmoothingMode;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        DrawDate(graphics, bounds, date, baseFont, dateColor);
        

        var scheduleTop = bounds.Top + 25;
        if (!string.IsNullOrWhiteSpace(holidayName))
        {
            DrawHoliday(graphics, bounds, holidayName, baseFont);
            scheduleTop += 16;
        }

        DrawSchedules(graphics, bounds, scheduleTop, schedules, baseFont);
        DrawCellOutline(graphics, bounds, date.Date == DateTime.Today, isSelected);

        graphics.SmoothingMode = previousSmoothingMode;
    }

    private static void DrawDate(
        Graphics graphics,
        Rectangle bounds,
        DateTime date,
        Font font,
        Color color)
    {
        TextRenderer.DrawText(
            graphics,
            date.Day.ToString(),
            font,
            new Rectangle(bounds.Left + HorizontalPadding, bounds.Top + 3, bounds.Width - 12, 20),
            color,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }
    

    private static void DrawHoliday(
        Graphics graphics,
        Rectangle bounds,
        string holidayName,
        Font baseFont)
    {
        using var holidayFont = new Font(baseFont.FontFamily, 8F, FontStyle.Regular);
        TextRenderer.DrawText(
            graphics,
            holidayName,
            holidayFont,
            new Rectangle(bounds.Left + HorizontalPadding, bounds.Top + 22, bounds.Width - 12, 16),
            Color.Firebrick,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
    }

    private void DrawSchedules(
        Graphics graphics,
        Rectangle bounds,
        int scheduleTop,
        IReadOnlyList<CalendarScheduleEntry> schedules,
        Font baseFont)
    {
        if (schedules.Count == 0)
            return;

        var availableHeight = bounds.Bottom - scheduleTop - 4;
        var visibleSlots = Math.Max(0, availableHeight / ScheduleRowHeight);
        if (visibleSlots == 0)
            return;

        var orderedSchedules = schedules
            .OrderByDescending(item => item.IsHighPriority)
            .ThenBy(item => item.StartHour)
            .ThenBy(item => item.EndHour)
            .ToList();

        var showMore = orderedSchedules.Count > visibleSlots;
        var schedulesToDraw = showMore ? Math.Max(0, visibleSlots - 1) : orderedSchedules.Count;

        using var normalFont = new Font(baseFont.FontFamily, 8.5F, FontStyle.Regular);
        using var priorityFont = new Font(baseFont.FontFamily, 8.5F, FontStyle.Bold);

        for (var index = 0; index < schedulesToDraw; index++)
        {
            var schedule = orderedSchedules[index];
            var rowBounds = new Rectangle(
                bounds.Left + HorizontalPadding,
                scheduleTop + index * ScheduleRowHeight,
                bounds.Width - HorizontalPadding * 2,
                ScheduleRowHeight);
            DrawSchedule(graphics, rowBounds, schedule, normalFont, priorityFont);
        }

        if (!showMore)
            return;

        var hiddenCount = orderedSchedules.Count - schedulesToDraw;
        var moreTop = scheduleTop + schedulesToDraw * ScheduleRowHeight;
        TextRenderer.DrawText(
            graphics,
            $"+{hiddenCount}개 더보기",
            normalFont,
            new Rectangle(bounds.Left + 17, moreTop, bounds.Width - 23, ScheduleRowHeight),
            Color.DimGray,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
    }

    private void DrawSchedule(
        Graphics graphics,
        Rectangle bounds,
        CalendarScheduleEntry schedule,
        Font normalFont,
        Font priorityFont)
    {
        var accentColor = categoryStore.GetScheduleAccentColor(
            schedule.CategoryId,
            schedule.CustomColorArgb);

        using var accentBrush = new SolidBrush(accentColor);
        using var path = CreateRoundedRectangle(
            new Rectangle(bounds.Left, bounds.Top + 3, 5, bounds.Height - 6),
            2);
        graphics.FillPath(accentBrush, path);

        var title = schedule.IsHighPriority ? $"★ {schedule.Text}" : schedule.Text;
        var titleColor = schedule.IsHighPriority
            ? Color.FromArgb(180, 48, 70)
            : Color.FromArgb(40, 48, 62);
        TextRenderer.DrawText(
            graphics,
            title,
            schedule.IsHighPriority ? priorityFont : normalFont,
            new Rectangle(bounds.Left + 11, bounds.Top, bounds.Width - 11, bounds.Height),
            titleColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
    }

    private static void DrawCellOutline(
        Graphics graphics,
        Rectangle bounds,
        bool isToday,
        bool isSelected)
    {
        if (!isToday && !isSelected)
            return;

        var color = isSelected
            ? Color.FromArgb(79, 107, 237)
            : Color.FromArgb(78, 177, 169);
        using var pen = new Pen(color, 2F);
        using var path = CreateRoundedRectangle(
            Rectangle.Inflate(bounds, -3, -3),
            8);
        graphics.DrawPath(pen, path);
    }

    private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
