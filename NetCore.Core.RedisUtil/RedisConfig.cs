using Microsoft.Extensions.Configuration;
using NetCore.Core.Extensions;
using NetCore.DTO.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.RedisUtil
{
    public static class RedisConfig
    {

        public static IConfiguration _configuration;
        public static void InitConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static string _WriteServerConStr = string.Empty;
        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public static string WriteServerConStr
        {
            get
            {
                if (string.IsNullOrEmpty(_WriteServerConStr))
                {
                    _WriteServerConStr = _configuration["ConnectionRedis:WriteServerConStr"];
                }
                return _WriteServerConStr;
            }
        }

        private static string _ReadServerConStr = string.Empty;
        /// <summary>
        ///  可读的Redis链接地址
        /// </summary>
        public static string ReadServerConStr
        {
            get
            {
                if (string.IsNullOrEmpty(_ReadServerConStr))
                {
                    _ReadServerConStr = _configuration["ConnectionRedis:ReadServerConStr"];
                }
                return _ReadServerConStr;
            }
        }


        private static int _DB = 0;

        public static int DB
        {
            get
            {
                _DB = _configuration["ConnectionRedis:DB"].ToInt();
                return _DB;
            }
        }

        private static int _MaxWritePoolSize = 5;

        public static int MaxWritePoolSize
        {
            get
            {
                _MaxWritePoolSize = _configuration["ConnectionRedis:MaxWritePoolSize"].ToInt();
                return _MaxWritePoolSize;
            }
        }
        /// <summary>
        ///最大读链接数
        /// </summary>
        /// 
        private static int _MaxReadPoolSize = 5;

        public static int MaxReadPoolSize
        {
            get
            {
                _MaxReadPoolSize = _configuration["ConnectionRedis:MaxReadPoolSize"].ToInt();
                return _MaxReadPoolSize;
            }
        }

        /// <summary>
        /// 自动重启
        /// </summary>

        private static bool _AutoStart = true;
        public static bool AutoStart
        {
            get
            {
                _AutoStart = _configuration["ConnectionRedis:AutoStart"].ToBool();
                return _AutoStart;
            }
        }


        /// <summary>
        ///本地缓存到期时间，单位:秒
        /// </summary>
        /// 
        private static int _LocalCacheTime = 36000;

        public static int LocalCacheTime
        {
            get
            {
                _LocalCacheTime = _configuration["ConnectionRedis:LocalCacheTime"].ToInt();
                return _LocalCacheTime;
            }
        }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>

        private static bool _RecordeLog = true;
        public static bool RecordeLog
        {
            get
            {
                _RecordeLog = _configuration["ConnectionRedis:RecordeLog"].ToBool();
                return _RecordeLog;
            }
        }

       

    }
}
