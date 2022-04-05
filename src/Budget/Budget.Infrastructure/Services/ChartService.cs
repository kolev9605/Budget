using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Charts.CashFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class ChartService : IChartService
    {
        private readonly IRecordRepository _recordRepository;

        public ChartService(
            IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<CashFlowChartModel> GetCashFlowChartData(CashFlowChartRequestModel cashFlowChartRequestModel, string userId)
        {
            var records = await _recordRepository.GetAllInRangeAndAccountsAsync(userId, cashFlowChartRequestModel.StartDate, cashFlowChartRequestModel.EndDate, cashFlowChartRequestModel.AccountIds);

            if (!records.Any())
            {
                return null;
            }

            var cashFlowItems = records
                .GroupBy(r => r.RecordDate.Date)
                .ToDictionary(r => r.Key, r => r.Sum(v => v.Amount))
                .Select(r => new CashFlowItemModel(GetCashFlow(records, r.Key), r.Key))
                .ToList();

            var chartData = new CashFlowChartModel()
            {
                Items = cashFlowItems,
                StartDate = cashFlowItems.Min(r => r.Date),
                EndDate = cashFlowItems.Max(r => r.Date),
                CashFlowForPeriod = records.Sum(r => r.Amount),

            };

            return chartData;
        }

        private decimal GetCashFlow(IEnumerable<Record> records, DateTime date)
            => records
                .Where(r => r.RecordDate.Date <= date.Date)
                .Sum(r => r.Amount);
    }
}
