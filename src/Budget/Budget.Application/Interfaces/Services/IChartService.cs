using Budget.Application.Models.Charts.CashFlow;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IChartService
    {
        Task<CashFlowChartModel> GetCashFlowChartDataAsync(CashFlowChartRequestModel cashFlowChartRequestModel, string userId);
    }
}
