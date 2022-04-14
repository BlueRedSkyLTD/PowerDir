using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PowerDir.Tests
{
#if DEBUG
    /// <summary>
    /// Available only in Debug test
    /// TODO: to be removed and replaced with escape codes colors
    /// </summary>
    [TestClass]
    public class PowerThemeTest
    {
        private PowerDirTheme.ColorThemeItem original()
        {
            return new PowerDirTheme.ColorThemeItem(ConsoleColor.Gray, ConsoleColor.Black);
        }
        [TestMethod]
        public void colorItems()
        {
            PowerDirTheme.ColorThemeItem c = original();
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Fg == ConsoleColor.Gray);
            Assert.IsTrue(c.Bg == ConsoleColor.Black);
        }
        [TestMethod]
        public void constructor1()
        {
            var c = original();
            PowerDirTheme theme = new PowerDirTheme(c.Fg, c.Bg);
            Assert.IsNotNull(theme);
            var tc = theme.GetOriginalColor();
            Assert.IsTrue(tc.Fg == c.Fg);
            Assert.IsTrue(tc.Bg == c.Bg);
        }

        [TestMethod]
        public void constructor2()
        {
            var c = original();
            PowerDirTheme theme = new PowerDirTheme(c);
            Assert.IsNotNull(theme);
            var tc = theme.GetOriginalColor();
            Assert.IsTrue(tc.Fg == c.Fg);
            Assert.IsTrue(tc.Bg == c.Bg);
        }
    }
#endif
}
