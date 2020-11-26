using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.a
{
   class ProtectedClass
    {
        public ProtectedClass()
        {
            aa = "123123333";
        }
        protected string aa { set; get; }
    }

    class ProtectedA:ProtectedClass
    {
        public void aaTest()
        {
            Console.WriteLine(aa);
        }
    }
}
