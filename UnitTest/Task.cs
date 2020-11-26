using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class TaskClass
    {

        public void Test()
        {
            Task.Run(() =>
            {

            });

            //带返回值
            Task.Delay(2000).ContinueWith(t=> {

            });

           

        }
    }
}
