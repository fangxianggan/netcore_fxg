using NetCore.Core.RedisUtil.Extension;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Core.RedisUtil
{
    public class Test
    {
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


    /// <summary>
    /// 商品没有超卖，300个；
    /// 出现了秒杀应该出现的场景，有人挤进来了但是商品已经秒光了（没有被通知，茫然脸），3个；
    ////还有一些人实际上可以认为根本就没排上队，请求来时已经卖光了（被正式通知），97个；
    ////请求没有丢，300+3+97，共400个。
    /// </summary>
    public class Test2
    {
        List<string> products = new List<string>();

        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //支持读写分离，均衡负载
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5,//“写”链接池链接数
                MaxReadPoolSize = 5,//“写”链接池链接数
                AutoStart = true,
            });
        }
        public void Startup()
        {
            for (int i = 0; i < 300; i++)
            {
                //模拟了300个商品 实际这些商品保存在Redis里更合适 这里是为了少写一些代码了
                products.Add("product_" + i.ToString());
            }

            //这里只是我自己封装的类库，根据配置文件创建了一个PooledRedisClientManager 
            //不要想复杂了 替换成你的创建方式就可以了
            PooledRedisClientManager prcm = CreateManager(new string[] { "127.0.0.1:6379" }, new string[] { "127.0.0.1:6379" });
           
            string lockKey = "秒杀_0706";          //随意写的一个锁定用的key 这个根据业务确定用什么值就可以了 例如说商品的编号
            var clientTemp = prcm.GetClient();
            clientTemp.Set("已售罄", false);       //存一些数值帮你看明白运行效果
            clientTemp.Set("售出数量", 0);
            clientTemp.Set("售罄后请求次数", 0);
            clientTemp.Set("缓存显示已售罄", 0);
            clientTemp.Set("锁定失败", 0);
            clientTemp.Dispose();
            DateTime startTime = DateTime.Now;
            bool isSellOut = false;
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 400; i++)          //多线程方式 模拟很多人抢有限的商品
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    using (var client = (RedisClient)prcm.GetClient())
                    {
                        isSellOut = client.Get<bool>("已售罄");
                        if (isSellOut)
                        {
                            client.Incr("缓存显示已售罄");//真实场景中 此时会在前端页面给用户提示：已经卖光啦
                         }
                         //这个Lock及下面的UnLock是个拓展方法 为了让大家看清楚逻辑 所以当静态方法用了
                         //3 是锁定时间3秒；1 是进行锁定等待时间1秒，这个值越小 锁定失败的几率就会越大。
                         else if (RedisNativeClientExtension.Lock(client, lockKey, 3, 1))
                        {
                            if (products.Count == 0)
                            {
                                 //这个很重要 这里标记已经卖完 就不会再做后面的逻辑了 
                                 //能避免后面几十万的不必要访问（如果是电商的话）
                                 client.Set("已售罄", true);
                                 //秒杀场景中 肯定会有大量用户请求执行到这个环节 所以采用isSellOut
                                 client.Incr("售罄后请求次数");
                            }
                            else
                            {
                                client.Incr("售出数量");
                                products.RemoveAt(0);//这里只是最简单的模拟了生成订单、减少库存量
                             }
                             //As you know,这里要解锁得啦
                             RedisNativeClientExtension.UnLock(client, lockKey);
                        }
                        else
                        {
                            client.Incr("锁定失败");
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());//等待所有人抢完（多线程的知识点这里就不讲了哟） 然后才获取下面的结果
            var 售出数量 = prcm.GetClient().Get<string>("售出数量");               //300
            var 售罄后请求次数 = prcm.GetClient().Get<string>("售罄后请求次数");   //3
            var 缓存显示已售罄 = prcm.GetClient().Get<string>("缓存显示已售罄");   //97
            var 锁定失败 = prcm.GetClient().Get<string>("锁定失败");               //0
        }
    }

}
