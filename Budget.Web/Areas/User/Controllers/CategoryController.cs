namespace Budget.Web.Areas.User.Controllers
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Web.Areas.User.ViewModels;
    using Budget.Web.Infrastructure;
    using Budget.Web.Infrastructure.ColorGenerator;
    using Budget.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IColorGenerator colorGenerator;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public CategoryController(
            ICategoryService categoryService,
            IColorGenerator colorGenerator,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.categoryService = categoryService;
            this.colorGenerator = colorGenerator;
            this.stringLocalizer = stringLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[GlobalConstants.DangerMessageKey] = this.stringLocalizer["GlobalErrorMessage"].Value;
                return RedirectToAction("AddTransaction", "Transaction", new { type = (TransactionType)addCategoryViewModel.TransactionTypeId });
            }

            var categoryId = await this.categoryService.AddOrGetCategoryAsync(addCategoryViewModel.Name, (TransactionType)addCategoryViewModel.TransactionTypeId, this.colorGenerator.GetColor());
            var successful = await this.categoryService.AddUserCategoryAsync(categoryId, this.User.GetUserId());

            if (successful)
            {
                TempData[GlobalConstants.SuccessMessageKey] = this.stringLocalizer["CategoryAddedSuccessfully"].Value;
            }
            else
            {
                TempData[GlobalConstants.WarningMessageKey] = this.stringLocalizer["CategoryAddedUnsuccessfully"].Value;
            }

            return RedirectToAction("AddTransaction", "Transaction", new { type = (TransactionType)addCategoryViewModel.TransactionTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CategoriesViewModel categoriesViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[GlobalConstants.DangerMessageKey] = this.stringLocalizer["GlobalErrorMessage"].Value;
                return RedirectToAction("Index", "Transaction");
            }

            var successful = await this.categoryService.DeleteUserCategoryAsync(categoriesViewModel.CategoryId, this.User.GetUserId());
            if (successful)
            {
                TempData[GlobalConstants.SuccessMessageKey] = this.stringLocalizer["CategoryRemovedSuccessfully"].Value;
            }
            else
            {
                TempData[GlobalConstants.WarningMessageKey] = this.stringLocalizer["CategoryHasTransactions"].Value;
            }

            TransactionType type = await this.categoryService.GetTransactionTypeByCategoryIdAsync(categoriesViewModel.CategoryId);
            return RedirectToAction("AddTransaction", "Transaction", new { type = type });
        }
    }
}