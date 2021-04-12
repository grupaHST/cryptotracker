using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cryptotracker.LocalData.Tests
{
    [TestClass]
    public class LocalAppSettingsTests
    {
        [TestMethod]
        public void EqualsTest()
        {
            LocalAppSettings firstStruct = new()
            {
                Language = "English",
                BaseColorScheme = "Base",
                ColorScheme = "Yellow",
            };

            LocalAppSettings secondStruct = firstStruct;

            Assert.AreEqual(firstStruct, secondStruct);
            Assert.IsTrue(firstStruct == secondStruct);

            secondStruct.ColorScheme = "Green";
            Assert.IsFalse(firstStruct == secondStruct);
            Assert.IsTrue(firstStruct != secondStruct);
        }
    }
}