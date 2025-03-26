using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Charts.CashFlow;

public class CashFlowChartRequestModel
{
    public List<Guid> AccountIds { get; set; } = new();

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
