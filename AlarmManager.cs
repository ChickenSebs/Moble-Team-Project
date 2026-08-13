using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

                    // ✨ 수정된 부분: 남은 시간을 계산하기 위해 현재 시간(now)을 같이 넘겨줍니다.
                    ShowAlarm(tab.Text, schedule, startAt, now);
                }
            }
        }

        notifiedSchedules.RemoveWhere(key => !activeKeys.Contains(key));
    }

    private static void ShowAlarm(
        string calendarName,
        CalendarScheduleEntry schedule,
        DateTime startAt,
        DateTime now) // ✨ 수정된 부분: 파라미터에 DateTime now 추가
    {
        var title = string.IsNullOrWhiteSpace(calendarName)
            ? "일정 알림"
            : $"{calendarName} 알림";

        // 0813 수정된 부분: 시작 시간과 현재 시간의 차이를 계산하여 남은 분(Minute)을 구합니다.
        TimeSpan remainingTime = startAt - now;
        int totalMinutes = (int)Math.Round(remainingTime.TotalMinutes);

        string timeString;
        if (totalMinutes > 0)
        {
            // 남은 시간이 1분 이상일 경우, 기존에 있던 FormatOffset을 재활용하여 텍스트를 만듭니다.
            timeString = $"{FormatOffset(totalMinutes)} 뒤에";
        }
        else
        {
            // 남은 시간이 0분이거나 이미 지났을 경우
            timeString = "곧";
        }

        // 수정된 부분: 계산된 동적 텍스트(timeString)를 사용하도록 메시지를 변경했습니다.
        var message = $"{startAt:HH:mm}  {schedule.Text}\n{timeString} 일정이 시작됩니다.";

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