using System;
using System.Web.Http;
using IdeaDatabase.Enums;
using System.Web.Http.Cors;
using IdeaDatabase.DataContext;
using InnovationPortalService.Filters;
using InnovationPortalService.Idea;

namespace InnovationPortalService.Controllers
{
    [SwaggerEnableAttribute]
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    [CustomAuthorizationFilter]
    public class RESTAPIControllerBase : ApiController
    {
        public int UserID { get; set; }
        public string SessionToken { get; set; }
        public string CallerId { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public RESTAPIDeviceWithDbContext DeviceWithDbContext { get; set; }   
        public int AlertIdValidated { get; set; }
        public string Client { get; set; }
        public RESTAPIPlatform? ClientPlatform { get; set; }
        public string Token { get; set; }      
        public DateTime? LoginDate { get; set; }
    }
}
