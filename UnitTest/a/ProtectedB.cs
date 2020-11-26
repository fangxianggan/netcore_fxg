using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.a
{
    class ProtectedB:ProtectedClass
    {
        public string cc { set; get; } 
        public ProtectedB()
        {
           // aa = "bbbbb";
            cc = aa;
        }
    }
}
