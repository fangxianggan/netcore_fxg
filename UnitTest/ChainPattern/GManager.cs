using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ChainPattern
{
    public class GManager : AbstractHandle
    {
       
        public GManager(string name,decimal [] amounts)
        {
            Name = name;
            Amounts[0] = amounts[0];
            Amounts[1] = amounts[1];
        }

        public override void HandleData(RequestHandle request)
        {
            if (request.Amount>=Amounts[0] &&request.Amount<=Amounts[1]) {
                Console.WriteLine("{0}:批给了{1}{2}元",Name,request.Name,request.Amount);
            } else {
                Console.WriteLine("拒绝审批钱");
                //NextHandle.HandleData(request);
            }
        }
    }
}
