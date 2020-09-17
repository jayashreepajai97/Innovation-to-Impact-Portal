using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdeaDatabase.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ActionsBaseTests
    {

        //[TestMethod()]
        //public void IsRequestValidTest1()
        //{
        //    DeviceMessage cls = new DeviceMessage();
        //    SetDeviceMessagesResponse response = new SetDeviceMessagesResponse();
        //    ActionsBase action = new ActionsBase();

        //    //TEST 01 NULLS
        //    Assert.IsFalse(action.IsRequestValid((DeviceMessage)null, null));
        //    //TEST 02
        //    Assert.IsFalse(action.IsRequestValid(cls, null));
        //    //test 03
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //}

        //[TestMethod()]
        //public void IsRequestValidTest2()
        //{
        //    DeviceUpdate cls = new DeviceUpdate();
        //    SetDeviceUpdatesResponse response = new SetDeviceUpdatesResponse();

        //    ActionsBase action = new ActionsBase();

        //    //TEST 01 NULLS
        //    Assert.IsFalse(action.IsRequestValid((DeviceUpdate)null, null));
        //    //TEST 02
        //    Assert.IsFalse(action.IsRequestValid(cls, null));
        //    //test 03
        //    Assert.IsFalse(action.IsRequestValid(cls, response));

        //    cls.Size = -1;
        //    cls.SoftwareId = "";
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidUpdateSize));
        //    Assert.IsTrue(response.ErrorList.Count > 0);

        //    cls = new DeviceUpdate();
        //    response = new SetDeviceUpdatesResponse();
        //    cls.Size = 100;
        //    cls.SoftwareId = "100";
        //    cls.SizeUnit = null;
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.Contains(Faults.MissingSizeUnit));

        //    cls = new DeviceUpdate();
        //    response = new SetDeviceUpdatesResponse();
        //    cls.SoftwareId = "100";
        //    cls.Size = 100;
        //    cls.SizeUnit = "aaaaaaa";
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidSizeUnit));

        //    cls = new DeviceUpdate();
        //    response = new SetDeviceUpdatesResponse();
        //    cls.SoftwareId = "100";
        //    cls.SizeUnit = "MB";
        //    cls.Size = 100;
        //    Assert.IsTrue(action.IsRequestValid(cls, response));

        //}

        //[TestMethod()]
        //public void IsRequestValidTest()
        //{
        //    Specification cls = new Specification();
        //    SetDeviceSpecificationsResponse response = new SetDeviceSpecificationsResponse();

        //    ActionsBase action = new ActionsBase();

        //    cls.SystemMemory = 100;
        //    cls.SystemMemoryUnit = "";

        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.Contains(Faults.MissingSystemMemoryUnit));


        //    response = new SetDeviceSpecificationsResponse();
        //    cls.SystemMemory = 100;
        //    cls.SystemMemoryUnit = "aaaa";
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidSystemMemoryUnit));

        //    response = new SetDeviceSpecificationsResponse();
        //    cls.SystemMemory = 100;
        //    cls.SystemMemoryUnit = "MB";
        //    Assert.IsTrue(action.IsRequestValid(cls, response));

        //    response = new SetDeviceSpecificationsResponse();
        //    Assert.IsFalse(action.IsRequestValid(null, response));

        //    response = new SetDeviceSpecificationsResponse();
        //    cls.SystemMemory = null;
        //    Assert.IsTrue(action.IsRequestValid(cls, response));

        //    response = new SetDeviceSpecificationsResponse();
        //    cls.SystemMemory = -1;
        //    Assert.IsTrue(action.IsRequestValid(cls, response));

        //    cls.SystemMemory = 100;
        //    cls.SystemMemoryUnit = "MB";
        //    cls.MemorySlots = new EntitySet<MemorySlot>();
        //    MemorySlot m1 = new MemorySlot();
        //    m1.Slot = 1;
        //    cls.MemorySlots.Add(m1);
        //    MemorySlot m2 = new MemorySlot();
        //    m2.Slot = 2;
        //    cls.MemorySlots.Add(m2);
        //    MemorySlot m3 = new MemorySlot();
        //    m3.Slot = 1;
        //    cls.MemorySlots.Add(m3);
        //    response = new SetDeviceSpecificationsResponse();
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode == Faults.MemorySlotIdAlreadyExists.ReturnCode);

        //    m3.Slot = 3;
        //    response = new SetDeviceSpecificationsResponse();
        //    Assert.IsTrue(action.IsRequestValid(cls, response));

        //    // 2 errors
        //    MemorySlot m4 = new MemorySlot();
        //    cls.MemorySlots.Add(m4);
        //    m1.Slot = 100;
        //    m2.Slot = 200;
        //    m3.Slot = 100;
        //    m4.Slot = 200;
        //    response = new SetDeviceSpecificationsResponse();
        //    Assert.IsFalse(action.IsRequestValid(cls, response));
        //    string status = response.ErrorList.First().StatusText;
        //    Assert.IsTrue(status.Contains("100"));
        //    Assert.IsTrue(status.Contains("200"));

        //}
    }
}