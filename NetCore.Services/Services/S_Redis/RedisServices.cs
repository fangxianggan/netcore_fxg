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

       
        private string _hashId;

        public string hashId
        {
            get
            {
                return _hashId;
            }
            set
            {
                _hashId = value;
            }
        }
       
        public RedisServices()
        {
            //初始化 hashid
            _hashId = "RefreshTokenHash";
            doRedisList = new DoRedisList();
            doRedisString = new DoRedisString();
            doRedisHash = new DoRedisHash();
        }



        #region hash 操作
        public bool SetValueHash(string key, string value)
        {
            doRedisHash.SetEntryInHash(hashId, key, value);
            return true;
        }

        public bool RemoveHash(string key)
        {
            doRedisHash.RemoveEntryFromHash(hashId, key);
            return true;
        }

        public bool IsExistKeyHash(string key)
        {
            return doRedisHash.HashContainsEntry(hashId, key);
        }

        public string GetValueHash(string key)
        {
            return doRedisHash.GetValueFromHash(hashId, key);
        }

        public Dictionary<string, string> GetAllHashById()
        {
            return doRedisHash.GetAllEntriesFromHash(hashId);
        }

        public bool SetValueList(string key, string value)
        {
             doRedisList.LPush(key, value);
            return true;
        }

        public bool RemoveList(string key)
        {
           return doRedisList.PopItemFromList(key)!=""?true:false;
        }

        public List<string> GetValueList(string key)
        {
            return doRedisList.Get(key);
        }

        #endregion

    }
}
