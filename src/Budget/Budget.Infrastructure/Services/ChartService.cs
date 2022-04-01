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
        private readonly IDateTimeProvider _dateTimeProvider;

        public ChartService(
            IRecordRepository recordRepository, 
            IDateTimeProvider dateTimeProvider)
        {
            _recordRepository = recordRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CashFlowChartModel> GetCashFlowChartData(CashFlowChartRequestModel cashFlowChartRequestModel, string userId)
        {
            var records = await _recordRepository.GetAllByMonthAndAccountsAsync(userId, cashFlowChartRequestModel.Month, cashFlowChartRequestModel.AccountIds);

            if (!records.Any())
            {
                return null;
            }

            var cashFlowItems = records
                .GroupBy(r => r.RecordDate.Date)
                .ToDictionary(r => r.Key, r => r.Sum(v => v.Amount))
                .Select(r => new CashFlowItemModel(GetCashFlow(records, r.Key), r.Key))
                .ToList();

            var startDate = cashFlowItems.Min(r => r.Date);
            var endDate = cashFlowItems.Max(r => r.Date);
            var monthBalance = records.Sum(r => r.Amount);

            var chartData = new CashFlowChartModel()
            {
                Items = cashFlowItems,
                StartDate = startDate,
                EndDate = endDate,
                Balance = monthBalance
            };

            return chartData;
        }

        private decimal GetCashFlow(IEnumerable<Record> records, DateTime date)
            => records
                .Where(r => r.RecordDate.Date <= date.Date)
                .Sum(r => r.Amount);
    }
}
