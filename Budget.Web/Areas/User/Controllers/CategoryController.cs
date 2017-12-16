namespace Budget.Web.Areas.User.Controllers
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Web.Areas.User.ViewModels;
    using Budget.Web.Common;
    using Budget.Web.Common.ColorGenerator;
    using Budget.Web.Extensions.MvcExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IColorGenerator colorGenerator;

        public CategoryController(
            ICategoryService categoryService,
            IColorGenerator colorGenerator)
        {
            this.categoryService = categoryService;
            this.colorGenerator = colorGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel addCategoryViewModel)
        {
            var categoryId = await this.categoryService.AddOrGetCategoryAsync(addCategoryViewModel.Name, (TransactionType)addCategoryViewModel.TransactionTypeId, this.colorGenerator.GetColor());
            var successful = await this.categoryService.AddUserCategoryAsync(categoryId, this.User.GetUserId());

            if (successful)
            {
                TempData[GlobalConstants.SuccessMessageKey] = GlobalConstants.CategoryAddedSuccessfully;
            }
            else
            {
                TempData[GlobalConstants.WarningMessageKey] = GlobalConstants.CategoryAddedUnsuccessfully;
            }

            return RedirectToAction("AddTransaction", "Transaction", new { type = (TransactionType)addCategoryViewModel.TransactionTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CategoriesViewModel categoriesViewModel)
        {
            var successful = await this.categoryService.DeleteUserCategoryAsync(categoriesViewModel.CategoryId, this.User.GetUserId());
            if (successful)
            {
                TempData[GlobalConstants.SuccessMessageKey] = GlobalConstants.CategoryRemovedSuccessfully;
            }
            else
            {
                TempData[GlobalConstants.WarningMessageKey] = GlobalConstants.CategoryHasTransactions;
            }

            TransactionType type = await this.categoryService.GetTransactionTypeByCategoryIdAsync(categoriesViewModel.CategoryId);
            return RedirectToAction("AddTransaction", "Transaction", new { type = type });
        }
    }
}