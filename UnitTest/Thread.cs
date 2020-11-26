using NetCore.Core.Util;
using System;
using System.Threading;

namespace UnitTest
{
    public class ThreadClass
    {

        /// <summary>
        /// 
        /// </summary>
        public void Test1()
        {
            Thread thread = new Thread(()=> {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString()+ "这是第一个线程");
                int length = 10000;
                int sum = 0;
                for (int i = 0; i < length; i++)
                {
                   
                    sum += i;
                }
                Thread.Sleep(1000);
                Console.WriteLine("计算结果1:{0}",sum);
                
            });
            thread.Start();
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Test2() {
            int sum = 0;
            int length = 100;
            for (int i = 0; i < length; i++)
            {
                sum += i;
            }
            Console.WriteLine("计算结果2：{0}",sum);
        }

        public void QianTaiThread()
        {
            Thread thread = new Thread(()=> {
                Thread.Sleep(3000);
                int length = 1000;
                int sum = 0;
                for (int i = 0; i < length; i++)
                {
                    sum += i;
                }

                LogUtil.Info(sum.ToString());

              
              
                //Console.WriteLine("{0}",sum);
                //Console.ReadKey();
            });
            //设置后台线程
            thread.IsBackground = true;
            thread.Start();
                 
        }

    }
}
