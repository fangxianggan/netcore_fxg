using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.IServices.I_Redis
{
    public interface IRedisServices
    {

        #region hash存储操作
        string hashId { set; get; }
        bool SetValueHash(string key, string value);

        bool RemoveHash(string key);

        bool IsExistKeyHash(string key);

        string GetValueHash(string key);

        Dictionary<string, string> GetAllHashById();

        #endregion

        #region list操作

        bool SetValueList(string key,string value);

        bool RemoveList(string key);

        List<string> GetValueList(string key);

        #endregion




    }
}
