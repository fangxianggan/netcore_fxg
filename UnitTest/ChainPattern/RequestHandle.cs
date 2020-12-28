using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ChainPattern
{
    /// <summary>
    /// 请求者
    /// </summary>
    public class RequestHandle
    {
        public string Name { set; get; }
        public decimal Amount { set; get; }
        public RequestHandle(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
