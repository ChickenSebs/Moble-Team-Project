using System;
using System.Collections.Generic;

namespace calendar4
{
    // ============================================================
    // 플래너 전체 데이터
    // ============================================================
    public class PlannerData
    {
        public List<PlannerTask> Tasks { get; set; } = new List<PlannerTask>();

        public List<PlannerTimeSlot> TimeSlots { get; set; } =
            new List<PlannerTimeSlot>();
    }

    // ============================================================
    // 할 일 데이터
    // ============================================================
    public class PlannerTask
    {
        public string Name { get; set; } = string.Empty;

        public bool Completed { get; set; }
    }

    // ============================================================
    // 타임테이블 형광펜 데이터
    // ============================================================
    public class PlannerTimeSlot
    {
        // 실제 시간
        // 예:
        // 7 = 오전 7시
        // 13 = 오후 1시
        // 0 = 자정
        public int Hour { get; set; }

        // 시작 분
        // 예: 30 -> XX시 30분
        public int StartMinute { get; set; }

        // 종료 분
        // 예: 50 -> XX시 50분
        public int EndMinute { get; set; }

        // 연결된 할 일
        public string TaskName { get; set; } = string.Empty;

        // 형광펜 색상
        public int R { get; set; }

        public int G { get; set; }

        public int B { get; set; }
    }
}