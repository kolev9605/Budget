using Budget.Data.Models.Enums;

namespace Budget.Web.Areas.User.ViewModels
{
    public class ChartViewModel
    {
        public string DataJson { get; set; }

        public string LabelsJson { get; set; }

        public string ColorsJson { get; set; }

        public bool IsCurrentChartEmpty => this.DataJson == "[]" || this.LabelsJson == "[]" || this.ColorsJson == "[]";
    }
}