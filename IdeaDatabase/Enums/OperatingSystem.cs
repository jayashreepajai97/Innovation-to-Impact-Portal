namespace IdeaDatabase.Enums
{
    public enum OS
    {
        XP = 0,
        Vista = 1,
        Windows7 = 2,
        Windows8 = 3,
        Windows81 = 4,
        Windows10 = 5
    }

    public enum OSEdition
    {
        Undefined = 0,
        Ultimate = 1,
        HomeBasic = 2,
        HomePremium = 3,
        Enterprise = 4,
        HomeBasicN = 5,
        Business = 6,
        Starter = 11,
        BusinessN = 16,
        Cloud = 20
    }

    public enum OSArchitecture
    {
        Bit32 = 32,
        Bit64 = 64,
        Unknown = 99
    }

    public enum OSServicePack
    {
        None = 0,
        SP1 = 1,
        SP2 = 2,
        SP3 = 3
    }
}