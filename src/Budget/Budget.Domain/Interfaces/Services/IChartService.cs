using Budget.Domain.Models.Charts.CashFlow;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IChartService
    {
        Task<CashFlowChartModel> GetCashFlowChartDataAsync(CashFlowChartRequestModel cashFlowChartRequestModel, string userId);
    }
}
