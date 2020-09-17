using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InnovationPortalServiceTests.Filters
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class APIActionsFilterTests
    {
        //[TestMethod]
        //public void LoadActionsFromFileTest_EmptyStringInFile()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        System.IO.Fakes.ShimFile.ExistsString = (info) => true;
        //        System.IO.Fakes.ShimFile.ReadAllTextString = (info) => "";

        //        SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //        {
        //            new AdmSettings { ParamName = "APIActionsListFilePath", StringValue = @"C:\Resources\APIActions\APIActions.txt" },
        //        });
        //        new PrivateType(typeof(APIActionsFilter)).SetStaticField("restrictedActions", null);

        //        Assert.IsNull(APIActionsFilter.RestrictedActions);
        //    }
        //}

        //[TestMethod]
        //public void LoadActionsFromFileTest_FileNotExist()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        System.IO.Fakes.ShimFile.ExistsString = (info) => false;
        //        System.IO.Fakes.ShimFile.ReadAllTextString = (info) => "";

        //        SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //        {
        //            new AdmSettings { ParamName = "APIActionsListFilePath", StringValue = @"C:\Resources\APIActions\APIActions.txt" },
        //        });

        //        new PrivateType(typeof(APIActionsFilter)).SetStaticField("restrictedActions", null);
        //        Assert.IsNull(APIActionsFilter.RestrictedActions);
        //    }
        //}

        //[TestMethod]
        //public void LoadActionsFromFileTest_EmptyJson()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        System.IO.Fakes.ShimFile.ExistsString = (info) => true;
        //        System.IO.Fakes.ShimFile.ReadAllTextString = (info) => "{\"RestrictedMethods\":[]}";

        //        SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //        {
        //            new AdmSettings { ParamName = "APIActionsListFilePath", StringValue = @"C:\Resources\APIActions\APIActions.txt" },
        //        });

        //        new PrivateType(typeof(APIActionsFilter)).SetStaticField("restrictedActions", new RestrictedMethod[] { });

        //        Assert.IsNotNull(APIActionsFilter.RestrictedActions);
        //        Assert.IsTrue(APIActionsFilter.RestrictedActions.Length == 0);
        //    }
        //}

        //[TestMethod]
        //public void LoadActionsFromFileTest_Sucess()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        System.IO.Fakes.ShimFile.ExistsString = (info) => true;
        //        System.IO.Fakes.ShimFile.ReadAllTextString = (info) => "{\"RestrictedMethods\":[{\"ActionName\":\"devices/{DeviceId}/support/{TopicId}\",\"Method\":\"GET\"},{\"ActionName\":\"profile\",\"Method\":\"PUT\"}]}";

        //        SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //        {
        //            new AdmSettings { ParamName = "APIActionsListFilePath", StringValue = @"C:\Resources\APIActions\APIActions.txt" },
        //        });

        //        new PrivateType(typeof(APIActionsFilter)).SetStaticField("restrictedActions", new RestrictedMethod[] { new RestrictedMethod() { ActionName = "devices/{DeviceId}/support/{TopicId}", Method = "GET" }, new RestrictedMethod() { ActionName = "profile", Method = "PUT" } });

        //        Assert.IsNotNull(APIActionsFilter.RestrictedActions);
        //        Assert.IsTrue(APIActionsFilter.RestrictedActions.Length == 2);
        //    }
        //}

        //[TestMethod]
        //public void LoadActionsFromFileTest_NullSetting()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        System.IO.Fakes.ShimFile.ExistsString = (info) => false;
        //        System.IO.Fakes.ShimFile.ReadAllTextString = (info) => "";
        //        new PrivateType(typeof(APIActionsFilter)).SetStaticField("restrictedActions", null);
        //        Assert.IsNull(APIActionsFilter.RestrictedActions);
        //    }
        //}
    }
}
