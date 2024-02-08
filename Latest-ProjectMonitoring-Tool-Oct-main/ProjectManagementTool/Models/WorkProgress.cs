using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class WorkProgress
    {
        public string Range { get; set; }
        public string UnitforProgress { get; set; }
        public int UnitQuantity { get; set; }
        public float Achieved { get; set; }
        public float Planned { get; set; }

    }
}