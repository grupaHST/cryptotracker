using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptotrackerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(true, true, $"{nameof(TestMethod1)} success!");
        }
    }
}
