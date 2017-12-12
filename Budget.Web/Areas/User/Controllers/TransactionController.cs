namespace Budget.Web.Areas.User.Controllers
{
    using AutoMapper;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Web.Areas.User.ViewModels;
    using Budget.Web.Common;
    using Budget.Web.Common.ColorGenerator;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IColorGenerator colourGenerator;

        public TransactionController(
            ICategoryService categoryService,
            ITransactionService transactionService,
            UserManager<User> userManager,
            IMapper mapper,
            IColorGenerator colourGenerator
            )
        {
            this.categoryService = categoryService;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.colourGenerator = colourGenerator;
        }

        public async Task<IActionResult> Index(TransactionType type = TransactionType.Expense)
        {
            var user = await this.userManager.FindByEmailAsync(this.User.Identity.Name);
            if (user == null)
            {
                return BadRequest();
            }

            var transactions = await this.transactionService
                .GetAllByUserIdAndTypeAsync(user.Id, type);

            ChartViewModel chartViewModel = mapper.Map<ChartViewModel>(transactions);

            return View(chartViewModel);
        }

        public async Task<IActionResult> AddTransaction(TransactionType type)
        {
            var user = await this.userManager.FindByEmailAsync(this.User.Identity.Name);
            if (user == null)
            {
                return BadRequest();
            }

            var categories = await this.categoryService.GetAllUserCategoriesByTypeAsync(user.Id, type);
            var addTransactionViewModel = this.mapper.Map<AddTransactionViewModel>(categories);
            addTransactionViewModel.UserId = user.Id;

            return View(addTransactionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(AddTransactionViewModel addTransactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addTransactionViewModel);
            }

            await this.transactionService.AddTransactionAsync(addTransactionViewModel.Amount, addTransactionViewModel.UserId, addTransactionViewModel.CategoryId);

            TempData[GlobalConstants.SuccessMessageKey] = GlobalConstants.TransactionAddedSuccessfully;

            return RedirectToAction(nameof(Index), new { type = this.categoryService.GetTransactionTypeByCategoryIdAsync(addTransactionViewModel.CategoryId) });
        }
    }
}