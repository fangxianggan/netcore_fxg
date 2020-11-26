using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DesignModel.DecoratorMode.Pay;

namespace UnitTest.DesignModel.DecoratorMode
{
    public class BaseHandle : AbstractBaseDoceator
    {
        private AbstractBaseDoceator _doceator;
        public BaseHandle(AbstractBaseDoceator doceator)
        {
            _doceator = doceator;
        }

        public override void CallBackHandle(decimal money)
        {
            _doceator.CallBackHandle(money);
        }
    }
}
