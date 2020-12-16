using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests
{
    [TestClass()]
    public class BelépőTests
    {
        [TestMethod()]
        public void belépheteTest1()
        {
            //ARRANGE
            var belépő = new Belépő();

            //ACT
            var result = belépő.beléphete(new User { aktív = true });

            //ASSERT
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void belépheteTest2()
        {
            //ARRANGE
            var belépő = new Belépő();

            //ACT
            var result = belépő.beléphete(new User { aktív = false });

            //ASSERT
            Assert.IsTrue(result);
        }
    }
}