using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DIPDesign.IService;

namespace UnitTest.DIPDesign.Service
{
    public class EmailService : INotify
    {
        public void Notify()
        {
            Console.WriteLine("这是{0}通知","邮件");
        }
    }
}
