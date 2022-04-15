﻿using System;
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
        public void TestColorItems()
        {
            PowerDirTheme.ColorThemeItem c = original();
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Fg == ConsoleColor.Gray);
            Assert.IsTrue(c.Bg == ConsoleColor.Black);
        }

        private void checkConstructor(PowerDirTheme theme, PowerDirTheme.ColorThemeItem c)
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
            PowerDirTheme theme = new(c.Fg, c.Bg);
            checkConstructor(theme, c);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var c = original();
            PowerDirTheme theme = new(c);
            checkConstructor(theme, c);
        }
    }
#endif
}