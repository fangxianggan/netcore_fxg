using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 枚举转化int
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int ToInt(this System.Enum e)
        {
            return e.GetHashCode();
        }

        /// <summary>
        /// 枚举转化string
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ToString(this System.Enum e)
        {
            return e.ToString();
        }

    }
}
