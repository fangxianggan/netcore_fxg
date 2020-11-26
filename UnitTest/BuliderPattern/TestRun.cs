using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.BuliderPattern
{
    public class TestRun
    {
        public TestRun()
        {

            string name = "小米";
            string str = "hhdhddggaagagagdggdgdgd";
            GoodStudent goodStudent = new GoodStudent(name, str);

            Factory factory = new Factory(goodStudent);
            factory.Construct();


        }
    }
}
