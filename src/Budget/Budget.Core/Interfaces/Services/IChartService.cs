using Budget.Core.Models.Charts.CashFlow;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IChartService
    {
        Task<CashFlowChartModel> GetCashFlowChartDataAsync(CashFlowChartRequestModel cashFlowChartRequestModel, string userId);
    }
}
