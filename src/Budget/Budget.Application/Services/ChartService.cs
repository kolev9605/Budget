using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Charts.CashFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class ChartService : IChartService
    {
        private readonly IRecordRepository _recordRepository;

        public ChartService(
            IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<CashFlowChartModel> GetCashFlowChartDataAsync(CashFlowChartRequestModel cashFlowChartRequestModel, string userId)
        {
            var records = await _recordRepository.GetAllInRangeAndAccountsAsync<Record>(userId, cashFlowChartRequestModel.StartDate, cashFlowChartRequestModel.EndDate, cashFlowChartRequestModel.AccountIds);

            if (!records.Any())
            {
                return null;
            }

            // TODO: Mapster configuration to be created
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
