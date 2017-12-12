namespace Budget.Web.Areas.User.ViewModels
{
    using Budget.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddTransactionViewModel
    {
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Oops! The entered amount is not valid.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public int CategoryId { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string Description { get; set; }

        [HiddenInput]
        public string UserId { get; set; }

        [HiddenInput]
        public TransactionType TransactionType { get; set; }
    }
}
