using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDir.themes;

namespace PowerDir.Tests
{
#if DEBUG
    /// <summary>
    /// Available only in Debug test
    /// TODO: to be removed and replaced with escape codes colors
    /// </summary>
    [TestClass]
    [Obsolete("To be removed")]
    public class PowerThemeTest
    {
        private PowerDirThemeClassic.ColorThemeItem original()
        {
            return new PowerDirThemeClassic.ColorThemeItem(ConsoleColor.Gray, ConsoleColor.Black);
        }
        [TestMethod]
        public void TestColorItems()
        {
            PowerDirThemeClassic.ColorThemeItem c = original();
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Fg == ConsoleColor.Gray);
            Assert.IsTrue(c.Bg == ConsoleColor.Black);
        }

#pragma warning disable S1172 // Unused method parameters should be removed
        private void checkConstructor(PowerDirThemeClassic theme, PowerDirThemeClassic.ColorThemeItem c)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            Assert.IsNotNull(theme);
            var tc = theme.GetOriginalColor();
            Assert.IsTrue(tc.Fg == c.Fg);
            Assert.IsTrue(tc.Bg == c.Bg);

        }

        [TestMethod]
        public void TestConstructor1()
        {
            var c = original();
            PowerDirThemeClassic theme = new(c.Fg, c.Bg);
            checkConstructor(theme, c);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var c = original();
            PowerDirThemeClassic theme = new(c);
            checkConstructor(theme, c);
        }
    }
#endif
}
