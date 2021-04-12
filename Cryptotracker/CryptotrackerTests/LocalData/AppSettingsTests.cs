using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cryptotracker.LocalData.Tests
{
    [TestClass]
    public class AppSettingsTests
    {
        [TestMethod]
        public void EqualsTest()
        {
            AppSettings firstStruct = new()
            {
                Language = "English",
                BaseColorScheme = "Base",
                ColorScheme = "Yellow",
            };

            AppSettings secondStruct = firstStruct;

            Assert.AreEqual(firstStruct, secondStruct);
            Assert.IsTrue(firstStruct == secondStruct);

            secondStruct.ColorScheme = "Green";
            Assert.IsFalse(firstStruct == secondStruct);
            Assert.IsTrue(firstStruct != secondStruct);
        }
    }
}