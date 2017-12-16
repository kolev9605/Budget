namespace Budget.Web.Areas.User.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Web.Areas.User.ViewModels;
    using Budget.Web.Common;
    using Budget.Web.Common.ColorGenerator;
    using Budget.Web.Extensions.MvcExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IColorGenerator colorGenerator;

        public TransactionController(
            ICategoryService categoryService,
            ITransactionService transactionService,
            UserManager<User> userManager,
            IMapper mapper,
            IColorGenerator colorGenerator
            )
        {
            this.categoryService = categoryService;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.colorGenerator = colorGenerator;
        }

        public async Task<IActionResult> Index(TransactionType type = TransactionType.Expense)
        {
            var loggedUserId = this.User.GetUserId();
            var transactions = await this.transactionService
                .GetAllByUserIdAndTypeAsync(loggedUserId, type);

            TransactionsViewModel transactionsViewModel = this.mapper.Map<TransactionsViewModel>(transactions);
            transactionsViewModel.ChartViewModel = mapper.Map<ChartViewModel>(transactions);
            transactionsViewModel.TransactionDataViewModel = transactions.Select(t => mapper.Map<TransactionDataViewModel>(t));
            transactionsViewModel.HasTransaction = await this.transactionService.HasTransactionsAsync(loggedUserId);
            transactionsViewModel.Type = type;

            return View(transactionsViewModel);
        }

        public async Task<IActionResult> AddTransaction(TransactionType type)
        {
            var categories = await this.categoryService.GetAllUserCategoriesByTypeAsync(this.User.GetUserId(), type);
            var addTransactionViewModel = this.mapper.Map<AddTransactionViewModel>(categories);
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

            await this.transactionService.AddTransactionAsync(addTransactionViewModel.Amount, loggedUserId, addTransactionViewModel.CategoriesViewModel.CategoryId, addTransactionViewModel.Description);

            TempData[GlobalConstants.SuccessMessageKey] = GlobalConstants.TransactionAddedSuccessfully;
            return RedirectToAction(nameof(Index), new { type = addTransactionViewModel.TransactionType });
        }

        public async Task<IActionResult> DeleteTransaction()
        {
            return NotFound();
        }
    }
}