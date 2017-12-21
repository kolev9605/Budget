namespace Budget.Web.Areas.Public.Controllers
{
    using Budget.Web.Areas.Public.ViewModels;
    using Budget.Web.Areas.User.Controllers;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Localization;
    using Microsoft.Extensions.Localization;
    using System;
    using System.Diagnostics;

    [Area("Public")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(TransactionController.Index), "Transaction", new { area = "User" });
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeLanguage(string culture)
        {
            var cookieName = CookieRequestCultureProvider.DefaultCookieName;
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            Response.Cookies.Append(cookieName, cookieValue);

            return RedirectToAction("Index", "Home");
        }
    }
}
