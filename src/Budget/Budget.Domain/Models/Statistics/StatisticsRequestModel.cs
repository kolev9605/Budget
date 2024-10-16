using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Statistics;

public class StatisticsRequestModel
{
    public IEnumerable<Guid> AccountIds { get; set; } = new List<Guid>();

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
