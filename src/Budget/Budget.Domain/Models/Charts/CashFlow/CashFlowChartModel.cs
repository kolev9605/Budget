﻿using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Charts.CashFlow;

public class CashFlowChartModel
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal CashFlowForPeriod { get; set; }

    public List<CashFlowItemModel> Items { get; set; } = new();
}
