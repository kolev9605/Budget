namespace Budget.Web.Infrastructure.Extensions
{
    using Budget.Data.Models.Enums;
    using Microsoft.AspNetCore.Localization;
    using System;
    using System.Linq;
    using System.Security.Claims;

    public static class Extensions
    {
        public static TransactionType GetOpositeTransactionType(this TransactionType type)
            => type == TransactionType.Expense ? TransactionType.Income : TransactionType.Expense;

        public static string ToHtmlString(this HtmlId htmlId)
        {
            string id = Enum.GetName(typeof(HtmlId), htmlId);
            if (id != string.Empty)
            {
                id = char.ToLower(id[0]) + id.Substring(1);
            }

            return id;
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string ToCurrentCultureString(this TransactionType type, ProviderCultureResult providerCulture)
        {
            string result = string.Empty;
            if(providerCulture.Cultures.FirstOrDefault().Value == "bg-BG")
            {
                result = type == TransactionType.Expense ? "Разход" : "Доход";
            }
            else if(providerCulture.Cultures.FirstOrDefault().Value == "en-US")
            {
                result = type.ToString();
            }
            else
            {
                throw new ArgumentException("Provided culture is not supported.");
            }

            return result;
        }
    }
}
