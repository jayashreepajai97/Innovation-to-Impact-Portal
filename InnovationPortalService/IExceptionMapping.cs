
using Responses;

namespace InnovationPortalService
{
    public interface IExceptionMapping
    {
       // void MapHPPGenericFaults(ResponseBase r, genericFaultType[] hppFaults);
        void MapGenericFaults(ResponseBase r, string errXml);
        void MapReturnCode(string xml, ResponseBase r);
    }
}