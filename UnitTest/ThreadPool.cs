using NetCore.Core.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace UnitTest
{
   public class ThreadPoolClass
    {

        // 用于保存每个线程的计算结果  
        static long[] result = new long[21];


        //注意：由于WaitCallback委托的声明带有参数，  
        //      所以将被调用的Fun方法必须带有参数，即：Fun(object obj)。  
        static void Fun(object obj)
        {
            int n = (int)obj;

            //计算阶乘  
            long fac = 1;
            for (int i = 1; i <= n; i++)
            {
                fac *= i;
            }
            //保存结果  
            result[n] = Convert.ToInt64(fac);
        }
        public void Test1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //向线程池中排入9个工作线程  
            for (int i = 1; i <= 20; i++)
            {
                //QueueUserWorkItem()方法：将工作任务排入线程池。  
                ThreadPool.QueueUserWorkItem(new WaitCallback(Fun), i);
                // Fun 表示要执行的方法(与WaitCallback委托的声明必须一致)。  
                // i   为传递给Fun方法的参数(obj将接受)。  
            }

            //输出计算结果  
            for (int i = 1; i <=20; i++)
            {
                Console.WriteLine("线程{0}: {0}! = {1}", i, result[i]);
            }
            stopwatch.Stop();
            Console.WriteLine("耗时：{0}", stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }


        public void Test2() {
            // 设置线程池中处于活动的线程的最大数目
            // 设置线程池中工作者线程数量为1000，I/O线程数量为1000
          //  ThreadPool.SetMaxThreads(1000, 1000);
          //  Console.WriteLine("Main Thread: queue an asynchronous method");
           // PrintMessage("Main Thread Start");

            // 把工作项添加到队列中，此时线程池会用工作者线程去执行回调方法            
            //ThreadPool.QueueUserWorkItem(asyncMethod);
            asyncWriteFile();
            Console.Read();
        }
        // 方法必须匹配WaitCallback委托
        private static void asyncMethod(object state)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method");
            Console.WriteLine("Asynchoronous thread has worked ");
        }

        #region 异步读取文件模块
        private static void asyncReadFile()
        {
            byte[] byteData = new byte[1024];
            FileStream stream = new FileStream(@"D:\123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //把FileStream对象，byte[]对象，长度等有关数据绑定到FileDate对象中，以附带属性方式送到回调函数
            Hashtable ht = new Hashtable();
            ht.Add("Length", (int)stream.Length);
            ht.Add("Stream", stream);
            ht.Add("ByteData", byteData);

            //启动异步读取,倒数第二个参数是指定回调函数，倒数第一个参数是传入回调函数中的参数
            stream.BeginRead(byteData, 0, (int)ht["Length"], new AsyncCallback(Completed), ht);
            PrintMessage("asyncReadFile Method");
        }

        //实际参数就是回调函数
        static void Completed(IAsyncResult result)
        {
            Thread.Sleep(2000);
            PrintMessage("asyncReadFile Completed Method");
            //参数result实际上就是Hashtable对象，以FileStream.EndRead完成异步读取
            Hashtable ht = (Hashtable)result.AsyncState;
            FileStream stream = (FileStream)ht["Stream"];
            int length = stream.EndRead(result);
            stream.Close();
            string str = Encoding.UTF8.GetString(ht["ByteData"] as byte[]);
            Console.WriteLine(str);
            stream.Close();
        }
        #endregion

        #region 异步写入文件模块
        //异步写入模块
        private static void asyncWriteFile()
        {
            //文件名 文件创建方式 文件权限 文件进程共享 缓冲区大小为1024 是否启动异步I/O线程为true
            FileStream stream = new FileStream(@"D:\123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //这里要注意，如果写入的字符串很小，则.Net会使用辅助线程写，因为这样比较快
            byte[] bytes = Encoding.UTF8.GetBytes("你在他乡还好吗？");
            //异步写入开始，倒数第二个参数指定回调函数，最后一个参数将自身传到回调函数里，用于结束异步线程
            stream.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(Callback), stream);
            PrintMessage("AsyncWriteFile Method");
        }

        static void Callback(IAsyncResult result)
        {
            //显示线程池现状
            Thread.Sleep(2000);
            PrintMessage("AsyncWriteFile Callback Method");
            //通过result.AsyncState再强制转换为FileStream就能够获取FileStream对象，用于结束异步写入
            FileStream stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Flush();
            stream.Close();
            asyncReadFile();
        }
        #endregion


        // 打印线程池信息
        private static void PrintMessage(String data)
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("{0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
                data,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsBackground.ToString(),
                workthreadnumber.ToString(),
                iothreadnumber.ToString());
        }

        public void Test3()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Thread thread = new Thread(()=> {
                int length = 20;
                for (int i = 1; i <= length; i++)
                {
                    Fun(i);
                }
            });
            thread.Start();
            //输出计算结果  
            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine("线程{0}: {0}! = {1}", i, result[i]);
            }
            sw.Stop();
            Console.WriteLine("耗时：{0}", sw.ElapsedMilliseconds);
            Console.ReadKey();

        }
    }
}
