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

        public async Task<CashFlowChartModel> GetCashFlowChartData(string userId, int month)
        {
            var records = await _recordRepository.GetAllByMonthAsync(userId, month);

            var cashFlowItems = records
                .GroupBy(r => r.RecordDate.Date)
                .ToDictionary(r => r.Key, r => r.Sum(v => v.Amount))
                .Select(r => new CashFlowItemModel(GetCashFlow(records, r.Key), r.Key))
                .ToList();

            if (!cashFlowItems.Any())
            {
                return new CashFlowChartModel();
            }

            var startDate = cashFlowItems.Min(r => r.Date);
            var endDate = cashFlowItems.Max(r => r.Date);
            var monthBalance = records.Sum(r => r.Amount);

            //cashFlowItems = FillInbetweenDates(cashFlowItems, records, startDate, endDate, monthBalance);

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
                .Where(r => r.RecordDate <= date)
                .Sum(r => r.Amount);

        private List<CashFlowItemModel> FillInbetweenDates(
            ICollection<CashFlowItemModel> cashFlowItems, 
            IEnumerable<Record> records, 
            DateTime startDate, 
            DateTime endDate, 
            decimal monthBalance)
        {
            var startDay = startDate.Day;
            var endDay = endDate.Day;

            for (var day = startDay; day <= endDay; day++)
            {
                var date = new DateTime(startDate.Year, startDate.Month, day);
                if (cashFlowItems.Any(i => i.Date == date))
                {
                    continue;
                }

                var sum = records
                    .Where(r => r.RecordDate <= date)
                    .Sum(r => r.Amount);

                var item = new CashFlowItemModel(sum, date);

                cashFlowItems.Add(item);
            }

            //cashFlowItems.Add(new CashFlowItemModel(monthBalance, _dateTimeProvider.Today));

            return cashFlowItems
                .OrderBy(i => i.Date)
                .ToList();
        }

    }
}
