namespace calendar4;

public sealed class AlarmManager : IDisposable
{
    private readonly TabControl tabControl;
    private readonly System.Windows.Forms.Timer checkTimer;
    private readonly HashSet<AlarmKey> notifiedSchedules = new();
    private bool disposed;

    public AlarmManager(TabControl tabControl)
    {
        this.tabControl = tabControl ?? throw new ArgumentNullException(nameof(tabControl));
        checkTimer = new System.Windows.Forms.Timer
        {
            Interval = 30_000
        };
        checkTimer.Tick += CheckTimer_Tick;
    }

    public void Start()
    {
        ThrowIfDisposed();
        CheckSchedules(DateTime.Now);
        checkTimer.Start();
    }

    public void Stop()
    {
        if (!disposed)
            checkTimer.Stop();
    }

    public void Dispose()
    {
        if (disposed)
            return;

        disposed = true;
        checkTimer.Stop();
        checkTimer.Tick -= CheckTimer_Tick;
        checkTimer.Dispose();
        notifiedSchedules.Clear();
    }

    private void CheckTimer_Tick(object? sender, EventArgs e)
    {
        CheckSchedules(DateTime.Now);
    }

    private void CheckSchedules(DateTime now)
    {
        if (tabControl.IsDisposed)
            return;

        var activeKeys = new HashSet<AlarmKey>();

        foreach (TabPage tab in tabControl.TabPages)
        {
            if (tab.Controls.Count == 0 || tab.Controls[0] is not CalendarControl calendar)
                continue;

            foreach (var (date, schedules) in calendar.GetScheduleMap())
            {
                foreach (var schedule in schedules)
                {
                    if (schedule.NotificationOffset <= 0)
                        continue;

                    var startAt = date.Date.AddHours(schedule.StartHour);
                    var key = new AlarmKey(schedule.Id, startAt);
                    activeKeys.Add(key);

                    var notifyAt = startAt.AddMinutes(-schedule.NotificationOffset);
                    if (now < notifyAt || now >= startAt || notifiedSchedules.Contains(key))
                        continue;

                    notifiedSchedules.Add(key);
                    ShowAlarm(tab.Text, schedule, startAt);
                }
            }
        }

        notifiedSchedules.RemoveWhere(key => !activeKeys.Contains(key));
    }

    private static void ShowAlarm(
        string calendarName,
        CalendarScheduleEntry schedule,
        DateTime startAt)
    {
        var title = string.IsNullOrWhiteSpace(calendarName)
            ? "일정 알림"
            : $"{calendarName} 알림";
        var message = $"{startAt:HH:mm}  {schedule.Text}\n{FormatOffset(schedule.NotificationOffset)} 뒤에 일정이 시작됩니다.";

        new ddayalarm(title, message).Show();
    }

    private static string FormatOffset(int minutes)
    {
        if (minutes < 60)
            return $"{minutes}분";

        int hours = minutes / 60;
        int remainingMinutes = minutes % 60;
        return remainingMinutes == 0
            ? $"{hours}시간"
            : $"{hours}시간 {remainingMinutes}분";
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(AlarmManager));
    }

    private readonly record struct AlarmKey(Guid ScheduleId, DateTime StartAt);
}
