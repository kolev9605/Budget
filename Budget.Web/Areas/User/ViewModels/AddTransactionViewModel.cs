namespace Budget.Web.Areas.User.ViewModels
{
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

        [Required(ErrorMessage = "You must select a category to add transaction.")]
        public int CategoryId { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }

        [HiddenInput]
        public string UserId { get; set; }
    }
}
