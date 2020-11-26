using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.BuliderPattern
{
    /// <summary>
    /// 抽象建造者
    /// </summary>
   public abstract  class Bulider
    {
        public abstract void DoWrite();

        public abstract void DoRead();

        public abstract void DoLook();

        public abstract void DoListen();

    }
}
