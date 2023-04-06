using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Charts.CashFlow;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class ChartService : IChartService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public ChartService(
            IBudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<CashFlowChartModel> GetCashFlowChartDataAsync(CashFlowChartRequestModel cashFlowChartRequestModel, string userId)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= cashFlowChartRequestModel.StartDate && r.RecordDate <= cashFlowChartRequestModel.EndDate)
                .Where(r => cashFlowChartRequestModel.AccountIds.Contains(r.AccountId))
                .OrderBy(r => r.RecordDate)
                .ToListAsync();

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
