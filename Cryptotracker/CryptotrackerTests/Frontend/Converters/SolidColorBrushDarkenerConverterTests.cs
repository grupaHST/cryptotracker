using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace Cryptotracker.Frontend.Converters.Tests
{
    [TestClass]
    public class SolidColorBrushDarkenerConverterTests
    {
        private readonly SolidColorBrushDarkenerConverter _converter = new();

        private const byte R_COLOR_VALUE = 111;
        private const byte G_COLOR_VALUE = 98;
        private const byte B_COLOR_VALUE = 161;

        private readonly SolidColorBrush _brush = new(new()
        {
            R = R_COLOR_VALUE,
            G = G_COLOR_VALUE,
            B = B_COLOR_VALUE
        });

        [TestMethod]
        public void ConvertTest()
        {
            object result = _converter.Convert(_brush, null, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(R_COLOR_VALUE - SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.R);
            Assert.AreEqual(G_COLOR_VALUE - SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.G);
            Assert.AreEqual(B_COLOR_VALUE - SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.B);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            object result = _converter.ConvertBack(_brush, null, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(R_COLOR_VALUE + SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.R);
            Assert.AreEqual(G_COLOR_VALUE + SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.G);
            Assert.AreEqual(B_COLOR_VALUE + SolidColorBrushLightenerConverter.LightenerLevel, (result as SolidColorBrush).Color.B);
        }
    }
}