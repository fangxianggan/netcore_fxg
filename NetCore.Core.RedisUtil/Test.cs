using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Core.RedisUtil
{
    public class Test {
        static void Main(string[] args)
        {
            string key = "zlh";
            //清空数据库
            RedisBase.Core.FlushAll();
            //给list赋值
            RedisBase.Core.PushItemToList(key, "1");
            RedisBase.Core.PushItemToList(key, "2");
            RedisBase.Core.AddItemToList(key, "3");
            RedisBase.Core.PrependItemToList(key, "0");
            RedisBase.Core.AddRangeToList(key, new List<string>() { "4", "5", "6" });
            #region 阻塞
            //启用一个线程来处理阻塞的数据集合
            new Thread(new ThreadStart(RunBlock)).Start();
            #endregion
            Console.ReadKey();
        }
        public static void RunBlock()
        {
            while (true)
            {
                //如果key为zlh的list集合中有数据，则读出，如果没有则等待2个小时，2个小时中只要有数据进入这里就可以给打印出来，类似一个简易的消息队列功能。
                Console.WriteLine(RedisBase.Core.BlockingPopItemFromList("zlh", TimeSpan.FromHours(2)));
            }
        }

       
    }

    public class Test1
    {
        static void Main(string[] args)
        {
            //清空数据库
            RedisBase.Core.FlushAll();
            //声明事务
            using (var tran = RedisManager.GetClient().CreateTransaction())
            {
                try
                {
                    tran.QueueCommand(p =>
                    {
                        //操作redis数据命令
                        RedisBase.Core.Set<int>("name", 30);
                        long i = RedisBase.Core.IncrementValueBy("name", 1);
                    });
                    //提交事务
                    tran.Commit();
                }
                catch
                {
                    //回滚事务
                    tran.Rollback();
                }
                ////操作redis数据命令
                //RedisManager.GetClient().Set<int>("zlh", 30);
                ////声明锁，网页程序可获得锁效果
                //using (RedisManager.GetClient().AcquireLock("zlh"))
                //{
                //    RedisManager.GetClient().Set<int>("zlh", 31);
                //    Thread.Sleep(10000);
                //}
            }
            Console.ReadKey();
        }
    }
   
}
