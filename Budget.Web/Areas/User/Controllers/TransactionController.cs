namespace Budget.Web.Areas.User.Controllers
{
    using AutoMapper;
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Web.Areas.User.ViewModels;
    using Budget.Web.Infrastructure;
    using Budget.Web.Infrastructure.ColorGenerator;
    using Budget.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ITransactionService transactionService;
        private readonly IMapper mapper;
        private readonly IColorGenerator colorGenerator;
        private readonly IUserService userService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public TransactionController(
            ICategoryService categoryService,
            ITransactionService transactionService,
            IMapper mapper,
            IColorGenerator colorGenerator,
            IUserService userService,
            IStringLocalizer<SharedResources> stringLocalizer
            )
        {
            this.categoryService = categoryService;
            this.transactionService = transactionService;
            this.mapper = mapper;
            this.colorGenerator = colorGenerator;
            this.userService = userService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index(TransactionType type = TransactionType.Expense)
        {
            var loggedUserId = this.User.GetUserId();
            var userTransactions = await this.transactionService
                .GetAllByUserIdAsync(loggedUserId);

            var transactionsByType = userTransactions.Where(t => t.Category.TransactionType == type);

            TransactionsViewModel transactionsViewModel = this.mapper.Map<TransactionsViewModel>(userTransactions);
            transactionsViewModel.ChartViewModel = mapper.Map<ChartViewModel>(transactionsByType);
            transactionsViewModel.TransactionDataViewModel = transactionsByType.Select(t => mapper.Map<TransactionDataViewModel>(t));
            transactionsViewModel.Type = type;
            transactionsViewModel.OpositeType = type.GetOpositeTransactionType();
            transactionsViewModel.Balance = await this.userService.GetUserBalanceAsync(loggedUserId);

            return View(transactionsViewModel);
        }

        public async Task<IActionResult> AddTransaction(TransactionType type)
        {
            var categories = await this.categoryService.GetAllUserCategoriesByTypeAsync(this.User.GetUserId(), type);
            var addTransactionViewModel = new AddTransactionViewModel();
            addTransactionViewModel.TransactionType = type;
            addTransactionViewModel.CategoriesViewModel = this.mapper.Map<CategoriesViewModel>(categories);
            addTransactionViewModel.AddCategoryViewModel = this.mapper.Map<AddCategoryViewModel>((TransactionType[])Enum.GetValues(typeof(TransactionType)));

            return View(addTransactionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(AddTransactionViewModel addTransactionViewModel)
        {
            var loggedUserId = this.User.GetUserId();
            if (!ModelState.IsValid)
            {
                var categories = await this.categoryService.GetAllUserCategoriesByTypeAsync(loggedUserId, addTransactionViewModel.TransactionType);
                addTransactionViewModel.CategoriesViewModel = this.mapper.Map<CategoriesViewModel>(categories);
                addTransactionViewModel.AddCategoryViewModel = this.mapper.Map<AddCategoryViewModel>((TransactionType[])Enum.GetValues(typeof(TransactionType)));

                return View(addTransactionViewModel);
            }

            await this.transactionService.AddTransactionAsync(addTransactionViewModel.Amount, 
                                                                loggedUserId, 
                                                                addTransactionViewModel.CategoriesViewModel.CategoryId, 
                                                                addTransactionViewModel.Description,
                                                                addTransactionViewModel.TransactionType);
            
            TempData[GlobalConstants.SuccessMessageKey] = this.stringLocalizer["TransactionAddedSuccessfully"].Value;
            return RedirectToAction(nameof(Index), new { type = addTransactionViewModel.TransactionType });
        }

        public async Task<IActionResult> DeleteTransaction(int id, int categoryId)
        {
            bool successful = await this.transactionService.DeleteTransactionAsync(id);
            if (successful)
            {
                TempData[GlobalConstants.SuccessMessageKey] = this.stringLocalizer["TransactionDeletedSuccessfully"].Value;
            }
            else
            {
                TempData[GlobalConstants.WarningMessageKey] = this.stringLocalizer["TransactionDeletedUnsuccessfully"].Value;
            }

            TransactionType type = await this.categoryService.GetTransactionTypeByCategoryIdAsync(categoryId);
            return RedirectToAction(nameof(Index), new { type = type });
        }
    }
}