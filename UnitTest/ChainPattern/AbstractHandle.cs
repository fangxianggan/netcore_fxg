using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ChainPattern
{
    /// <summary>
    /// 抽象处理类
    /// </summary>
    public abstract class AbstractHandle
    {
        public string Name { set; get; }

        public decimal[] Amounts { set; get; } = new decimal[2];

        public AbstractHandle NextHandle { set; get; }

        public abstract void HandleData(RequestHandle request);

    }
}
