namespace Budget.Web.Areas.User.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public int TransactionTypeId { get; set; }

        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
    }
}
