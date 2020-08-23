using System;
using System.ComponentModel;

namespace NetCore.Core.Util
{
    public static  class TypeUtil
    {
        /// <summary>
        /// 判断类型是否为Nullable类型
        /// </summary>
        /// <param name="type"> 要处理的类型 </param>
        /// <returns> 是返回True，不是返回False </returns>
        public static bool IsNullableType(this Type type)
        {
            return ((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// 通过类型转换器获取Nullable类型的基础类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                NullableConverter nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }

        public static object ConvertForType(object value, Type type)
        {
            switch (type.FullName)
            {
                case "System.String":
                    value = value.ToString();
                    break;
                case "System.Boolean":
                    value = bool.Parse(value.ToString());
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    value = int.Parse(value.ToString());
                    break;
                case "System.Double":
                    value = double.Parse(value.ToString());
                    break;
                case "System.Decimal":
                    value = new decimal(double.Parse(value.ToString()));
                    break;
            }

            return value;
        }
    }
}
