using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.BuliderPattern
{
    public class GoodStudent : Bulider
    {
        public GoodStudent(string name,string str)
        {
            Name = name;
            Str = str;

        }
        public string Name { set; get; }

        public string Str { set; get; }


        public override void DoListen()
        {
            Console.WriteLine("{0}:完全听懂了{1}",Name,Str);
        }

        public override void DoLook()
        {
            Console.WriteLine("{0}:完全看了{1}",Name,Str);
        }

        public override void DoRead()
        {
            Console.WriteLine("{0}:很快的读了{1}",Name,Str);
        }

        public override void DoWrite()
        {
            Console.WriteLine("{0}:很正确的写了{1}",Name,Str);
        }
    }
}
