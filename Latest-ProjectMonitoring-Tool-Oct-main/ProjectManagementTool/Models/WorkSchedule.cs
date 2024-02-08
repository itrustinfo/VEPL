using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class WorkSchedule
    {
        public string MileStoneId { get; set; }

        public string MileStoneName { get; set; }
        public string TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime WorkDate { get; set; }
        public float PlannedValue { get; set; }
        public float AchievedValue { get; set; }
    }
}