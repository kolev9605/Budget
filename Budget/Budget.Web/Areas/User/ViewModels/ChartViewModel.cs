using Budget.Data.Models.Enums;

namespace Budget.Web.Areas.User.ViewModels
{
    public class ChartViewModel
    {
        public string DataJson { get; set; }

        public string LabelsJson { get; set; }

        public string ColorsJson { get; set; }

        public bool IsEmpty => this.DataJson == "[]" || this.LabelsJson == "[]" || this.ColorsJson == "[]";

        public TransactionType OpositeType { get; set; }

        public string ChangeChartStatusButtonLabel => $"Show {OpositeType}";
    }
}