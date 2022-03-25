using Budget.Core.Models.Charts.CashFlow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IChartService
    {
        Task<CashFlowChartModel> GetCashFlowChartData(string userId, int month);
    }
}
