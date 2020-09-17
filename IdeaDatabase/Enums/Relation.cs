namespace IdeaDatabase.Enums
{
    public enum Relation
    {
        NoneRel                 = 0,
        EnitlementRel           = 1 << 1,
        UpdatesRel              = 1 << 2,
        MessagesRel             = 1 << 3,
        SpecificationRel        = 1 << 4,
        SoftwareRel             = 1 << 5,
        StorageRel              = 1 << 6,
        InternetAndSecurityRel  = 1 << 7,
        AudioDevicesRel         = 1 << 8,
        BrowsersRel             = 1 << 9,
        ConnectionsRel          = 1 << 10,
        PropertiesRel           = 1 << 11,
        DrivesRel               = 1 << 12,
        GraphicsDevicesRel      = 1 << 13,
        MemorySlotsRel          = 1 << 14,
        AccessoriesRel          = 1 << 15,
        TPAEventsRel            = 1 << 16,
        ResourcesRel            = 1 << 17,
        CDAXRel                 = 1 << 18,
        AllRel                  = ~0
    }
}