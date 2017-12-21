namespace Budget.Web.Areas.User.ViewModels
{
    using Budget.Data.Models.Enums;
    using System.Collections.Generic;
    using System.Linq;

    public class TransactionsViewModel
    {
        public ChartViewModel ChartViewModel { get; set; }

        public IEnumerable<TransactionDataViewModel> TransactionDataViewModel { get; set; }

        public bool HasTransactionDataViewModel => this.TransactionDataViewModel != null && this.TransactionDataViewModel.Any();

        public bool HasTransaction { get; set; }

        public TransactionType Type { get; set; }

        public TransactionType OpositeType { get; set; }

        public string ChangeChartStatusButtonLabel => $"Show {OpositeType}s";

        public decimal Balance { get; set; }
    }
}
