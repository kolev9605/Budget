namespace Budget.Common
{
    public static class Helpers
    {
        public static IEnumerable<string> GetListFromEnum<T>()
            where T : struct, IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException("Type T must be an enum type.");
            }

            return Enum.GetNames(type);
        }
    }
}
