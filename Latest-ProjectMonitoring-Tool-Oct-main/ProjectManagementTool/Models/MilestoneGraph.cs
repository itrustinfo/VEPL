using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class MilestoneGraph
    {
        public string MileStoneId { get; set; }
        public float PlannedValue { get; set; }
        public float AchievedValue { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }
}