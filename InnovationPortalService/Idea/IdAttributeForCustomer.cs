using IdeaDatabase.Validation;
using InnovationPortalService.Idea;

namespace InnovationPortalService.Idea
{
    public abstract class IdAttributeForCustomer : ValidableObject
    {
        [NumberValidation(1, required: true, fromString: true)]
        public string Id { get; set; }
        public int UserID { get; set; }
        public bool ReadOnly { get; set; }
        public RESTAPIDeviceWithDbContext DeviceWithDbContext { get; set; }
    }
}
