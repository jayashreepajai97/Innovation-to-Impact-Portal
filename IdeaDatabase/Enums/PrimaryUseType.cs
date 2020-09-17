namespace IdeaDatabase.Enums
{
    public enum PrimaryUseType
    {
        Item002, Item003, Item005, Item006
    }

    public static class PrimaryUseTypeExtension
    {
        public static string HPPValue(this string p)
        {
            switch (p)
            {
                case "Item002":
                    return "002";
                case "Item003":
                    return "003";
                case "Item005":
                    return "005";
                case "Item006":
                    return "006";
            }
            return null;
        }
    }
}