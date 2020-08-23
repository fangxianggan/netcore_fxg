using System;
using System.Reflection;

namespace NetCore.Core.Util
{
    public static class ReflectionUtil
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static FieldInfo[] GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public static void SetPropertyValue(object obj, PropertyInfo property, object value)
        {
            //创建Set委托
            var setter = DynamicMethodCompilerUtil.CreateSetHandler(obj.GetType(), property);

            //先获取该私有成员的数据类型
            var type = property.PropertyType;

            //通过数据类型转换
            value = TypeUtil.ConvertForType(value, type);

            //将值设置到对象中
            setter(obj, value);
        }

        public static object GetPropertyValue(object obj, PropertyInfo property)
        {
            //创建Set委托
            var getter = DynamicMethodCompilerUtil.CreateGetHandler(obj.GetType(), property);

            //获取属性值
            return getter(obj);
        }

        public static void SetFieldValue(object obj, FieldInfo field, object value)
        {
            //创建Set委托
            var setter = DynamicMethodCompilerUtil.CreateSetHandler(obj.GetType(), field);

            //先获取该私有成员的数据类型
            var type = field.FieldType;

            //通过数据类型转换
            value = TypeUtil.ConvertForType(value, type);

            //将值设置到对象中
            setter(obj, value);
        }

        public static object GetFieldValue(object obj, FieldInfo field)
        {
            //创建Set委托
            var getter = DynamicMethodCompilerUtil.CreateGetHandler(obj.GetType(), field);

            //获取字段值
            return getter(obj);
        }
    }
}
