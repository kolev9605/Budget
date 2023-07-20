using System;
using System.Collections.Generic;

namespace Budget.Application.Models.Statistics
{
    public class StatisticsRequestModel
    {
        public IEnumerable<int> AccountIds { get; set; } = new List<int>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
