namespace Budget.Web.Extensions
{
    using Budget.Web.Common;
    using System;

    public static class HtmlIdExtensions
    {
        public static string ToHtmlString(this HtmlId htmlId)
        {
            string id = Enum.GetName(typeof(HtmlId), htmlId);
            if (id != string.Empty)
            {
                id = char.ToLower(id[0]) + id.Substring(1);
            }

            return id;
        }
    }
}
