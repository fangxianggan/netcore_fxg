using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ChainPattern
{
    /// <summary>
    /// 经理处理审批金额
    /// </summary>
    public class Manager : AbstractHandle
    {
       
        public Manager(string name,decimal []amounts)
        {
            Name = name;
            Amounts[0] = amounts[0];
            Amounts[1] = amounts[1];
        }

        public override void HandleData(RequestHandle request)
        {
            if (request.Amount >= Amounts[0] && request.Amount <= Amounts[1])
            {
                Console.WriteLine("{0}:批了{1}{2}元", Name, request.Name, request.Amount);
            }
            else
            {
               NextHandle.HandleData(request);
            }
        }
    }
}
