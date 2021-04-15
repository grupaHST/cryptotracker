using Cryptotracker.Languages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Cryptotracker.Frontend.Converters.Tests
{
    [TestClass]
    public class LanguageToCultureInfoConverterTests
    {
        [TestMethod]
        public void ConvertTest()
        {
            LanguageToCultureInfoConverter converter = new();

            object conversionResult = converter.Convert(Language.Polski, typeof(CultureInfo), null, null);
            Assert.IsTrue(conversionResult is CultureInfo);
            Assert.AreEqual(new CultureInfo("pl-PL"), conversionResult as CultureInfo);
            
            conversionResult = converter.Convert(null, typeof(CultureInfo), null, null);
            Assert.IsNull(conversionResult);
            
            conversionResult = converter.Convert(Language.English, typeof(CultureInfo), null, null);
            Assert.IsTrue(conversionResult is CultureInfo);
            Assert.AreEqual(new CultureInfo("en-US"), conversionResult as CultureInfo);
        }
    }
}