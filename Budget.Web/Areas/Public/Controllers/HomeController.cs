namespace Budget.Web.Areas.Public.Controllers
{
    using Budget.Web.Areas.Public.ViewModels;
    using Budget.Web.Areas.User.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [Area("Public")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(TransactionController.Index), "Transaction", new { area = "User" });
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
