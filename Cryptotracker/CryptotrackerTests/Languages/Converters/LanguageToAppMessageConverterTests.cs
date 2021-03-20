using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Cryptotracker.Languages.Converters.Tests
{
    [TestClass]
    public class LanguageToAppMessageConverterTests
    {
        private readonly static LanguageToAppMessageConverter converter = new();

        [TestMethod]
        public void ConvertTest()
        {
            string fakeFunctionName = "FakeFunctionName123456789";
            Assert.IsNull(converter.Convert(Language.English, typeof(string), fakeFunctionName, null));

            string firstFunctionName = typeof(AppMessages).GetMethods().FirstOrDefault(x => x.IsStatic).Name;
            object result = converter.Convert(Language.English, typeof(string), firstFunctionName, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string).Name, result.GetType().Name);
        }
    }
}