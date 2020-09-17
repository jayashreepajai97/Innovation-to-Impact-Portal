using System;
using Responses;
using System.Linq;
using IdeaDatabase.Utils;
using IdeaDatabase.Validation;
using System.Web.Http.Controllers;
using InnovationPortalService.Idea;

namespace InnovationPortalService.Filters
{
    //public enum IdValidationAttributeEnum
    //{
    //    IdeaId = 0
    //}

    //public class IdAttributeValidationFromUri : Attribute, IUriValidationAttribute
    //{
    //    private bool readOnly = false;
    //    private IdValidationAttributeEnum AttributeName = IdValidationAttributeEnum.DeviceId;

    //    public IdAttributeValidationFromUri(bool readOnly, IdValidationAttributeEnum AttributeName = IdValidationAttributeEnum.DeviceId)
    //    {
    //        this.readOnly = readOnly;
    //        this.AttributeName = AttributeName;
    //    }

    //    public bool Validate(HttpActionContext actionContext, ResponseBase response)
    //    {
    //        object req = actionContext.ActionArguments.Where(x => x.Key == AttributeName.ToString()).Select(x => x.Value).FirstOrDefault();
    //        if (req == null)
    //        {
    //            response.ErrorList.Add(new RequiredValidationFault(AttributeName.ToString()));
    //            return false;
    //        }
    //        else
    //        {
    //            var Controller = actionContext.ControllerContext.Controller as InnovationPortalService.Controllers.RESTAPIControllerBase;

    //            IdAttributeForCustomer deviceIdReq = IdRequestFactory.GetObject(AttributeName, Controller.UserID, req.ToString(), readOnly);
    //            ValidableObject vo = deviceIdReq;
    //            if (!vo.IsValid(response))
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                // store the DeviceId in the controller base
    //                Controller.DeviceWithDbContext = deviceIdReq.DeviceWithDbContext;
                   
    //            }
    //        }

    //        return true;
    //    }

    //    private static class IdRequestFactory
    //    {
    //        public static IdAttributeForCustomer GetObject(IdValidationAttributeEnum AttributeName, int UserID, string req, bool readOnly)
    //        {
    //            IdAttributeForCustomer idReq = null;

    //            switch (AttributeName)
    //            {
    //                case IdValidationAttributeEnum.DriveId:
    //                    idReq = new RESTAPIDriveForCustomer()
    //                    {
    //                        Id = req,
    //                        UserID = UserID,
    //                        ReadOnly = readOnly
    //                    };
    //                    break;
    //                case IdValidationAttributeEnum.BatteryId:
    //                    idReq = new RESTAPIBatteryForCustomer()
    //                    {
    //                        Id = req,
    //                        UserID = UserID,
    //                        ReadOnly = readOnly
    //                    };
    //                    break;
    //                case IdValidationAttributeEnum.DeviceId:
    //                    idReq = new RESTAPIDeviceForCustomer()
    //                    {
    //                        Id = req,
    //                        UserID = UserID,
    //                        ReadOnly = readOnly
    //                    };
    //                    break;
    //                default:
    //                    break;
    //            }

    //            return idReq;
    //        }
    //    }
    //}
}