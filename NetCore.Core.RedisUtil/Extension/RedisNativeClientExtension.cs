using ServiceStack.Redis;
using System;
using System.Threading;

namespace NetCore.Core.RedisUtil.Extension
{
    /// <summary>
    /// RedisNativeClient拓展类
    /// </summary>
    public static class RedisNativeClientExtension
    {
        /// <summary>
        /// 锁定指定的Key
        /// </summary>
        /// <param name="redisClient">RedisClient 对象</param>
        /// <param name="key">要锁定的Key</param>
        /// <param name="expirySeconds">锁定时长 秒</param>
        /// <param name="waitSeconds">锁定等待时长 秒 默认不等待</param>
        /// <returns>是否锁定成功</returns>
        public static bool Lock(this RedisNativeClient redisClient, string key, int expirySeconds = 5, double waitSeconds = 0)
        {
            int waitIntervalMs = 50;//间隔等待时长 毫秒 合理配置一下 应该也会影响性能 间隔时间太长肯定是不行的
            string lockKey = "lock_key:" + key;

            DateTime begin = DateTime.Now;
            while (true)
            {
                if (redisClient.SetNX(lockKey, new byte[] { 1 }) == 1)
                {
                    redisClient.Expire(lockKey, expirySeconds);
                    return true;
                }

                //不等待锁则返回
                if (waitSeconds <= 0)
                    break;

                if ((DateTime.Now - begin).TotalSeconds >= waitSeconds)//等待超时
                    break;

                Thread.Sleep(waitIntervalMs);
            }
            return false;
        }

        /// <summary>
        /// 接触锁定
        /// </summary>
        /// <param name="redisClient">RedisClient 对象</param>
        /// <param name="key">要解锁的Key</param>
        /// <returns></returns>
        public static long UnLock(this RedisNativeClient redisClient, string key)
        {
            string lockKey = "lock_key:" + key;
            return redisClient.Del(lockKey);
        }
    }
}
