namespace Budget.Web.Areas.Public.Controllers
{
    using Budget.Web.Areas.User.Controllers;
    using Microsoft.AspNetCore.Mvc;
    
    [Area("Public")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(TransactionController.Index), "Transaction", new { area = "user" });
            }

            return View();
        }
    }
}
