using NetCore.Core.RedisUtil;
using NetCore.Services.IServices.I_Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.Services.S_Redis
{
    public class RedisServices : IRedisServices
    {
        private readonly DoRedisList doRedisList;
        private readonly DoRedisString doRedisString;
        private readonly DoRedisHash doRedisHash;

        private string hashId = "RefreshTokenHash";

        public RedisServices()
        {
            doRedisList = new DoRedisList();
            doRedisString = new DoRedisString();
            doRedisHash = new DoRedisHash();
        }
        /// <summary>
        /// 存储list shujv
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SetRefreshTokenValue(string key, string value)
        {
            doRedisHash.SetEntryInHash(hashId, key, value);
            return true;
        }

        /// <summary>
        /// 移除存储的 刷新的token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveRefreshTokenValue(string key)
        {
            doRedisHash.RemoveEntryFromHash(hashId, key);
            return true;
        }

        /// <summary>
        /// 判断存储的key 是否存储
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetIsExistKey(string key)
        {
            return doRedisHash.HashContainsEntry(hashId, key);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueFromHash(string key)
        {
            return doRedisHash.GetValueFromHash(hashId, key);
        }

        public Dictionary<string,string> GetAllEntriesFromHash()
        {
            return doRedisHash.GetAllEntriesFromHash(hashId);
        }

        
    }
}
