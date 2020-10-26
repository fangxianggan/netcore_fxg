using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.IServices.I_Redis
{
   public interface IRedisServices
    {
        bool SetRefreshTokenValue(string key, string value);

        bool RemoveRefreshTokenValue(string key);

        bool GetIsExistKey(string key);

        string GetValueFromHash(string key);

    }
}
