namespace Budget.Web.Common
{
    public class GlobalConstants
    {
        public const string SuccessMessageKey = "SuccessMessage";
        public const string InfoMessageKey = "InfoMessage";
        public const string DangerMessageKey = "DangerMessage";
        public const string WarningMessageKey = "WarningMessage";

        public const string TransactionAddedSuccessfully = "You have successfully added a transaction!";
        public const string CategoryRemovedSuccessfully = "You have successfully removed selected category!";
        public const string CategoryHasTransactions = "You can not remove the selected category, because there are active transactions with it.";

        public const string CategoryAddedSuccessfully = "You have successfully added a category!";
        public const string CategoryAddedUnsuccessfully = "Something went wrong adding the category! :/";
    }
}
