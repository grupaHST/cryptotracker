using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cryptotracker.Frontend.Converters.Tests
{
    [TestClass]
    public class BoolNegatorConverterTests
    {
        private readonly BoolNegatorConverter converter = new();

        [TestMethod]
        public void ConvertTest()
        {
            Assert.AreEqual(false, (bool)converter.Convert(true, null, null, null));
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            Assert.AreEqual(true, (bool)converter.ConvertBack(false, null, null, null));
        }
    }
}