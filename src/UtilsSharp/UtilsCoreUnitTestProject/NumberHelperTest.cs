using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsCore;

namespace UtilsCoreUnitTestProject
{
    [TestClass]
    public class NumberHelperTest
    {

        [TestMethod]
        public void ToCnNumber()
        {
            var str =NumberHelper.ToCnNumber(-8,true);
        }

        [TestMethod]
        public void ToCnMoney()
        {
            var str = NumberHelper.ToCnMoney(1000006);
        }

    }
}
