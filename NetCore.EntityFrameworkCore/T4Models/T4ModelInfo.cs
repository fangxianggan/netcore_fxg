using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace NetCore.EntityFrameworkCore.T4Models
{
    /// <summary>
    /// T4实体模型信息类
    /// </summary>
    public class T4ModelInfo
    {
        /// <summary>
        /// 获取 是否使用模块文件夹
        /// </summary>
        public bool UseModuleDir { get; private set; }

        /// <summary>
        /// 获取 模型所在模块名称
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// 获取 模型名称
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// 获取 模型名称 以驼峰法命名
        /// </summary>
        public string _Name { get; private set; }

        /// <summary>
        /// 获取 模型描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        /// 主键类型名称
        /// </summary>
        public string KeyTypeName { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string KeyName { set; get; }

        public IEnumerable<PropertyInfo> Properties { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="useModuleDir"></param>
        public T4ModelInfo(Type modelType, bool useModuleDir = false)
        {

            var @namespace = modelType.Namespace;
            if (@namespace == null)
            {
                return;
            }
            UseModuleDir = useModuleDir;
            if (UseModuleDir)
            {
                var index = @namespace.LastIndexOf('.') + 1;
                ModuleName = @namespace.Substring(index, @namespace.Length - index);
            }

            Name = modelType.Name;
            _Name = modelType.Name.Substring(0, 1).ToLower() + modelType.Name.Substring(1, modelType.Name.Length - 1);


            PropertyInfo pkProp = modelType.GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();
            //主键名称
            KeyName = pkProp.Name;
            PropertyInfo keyProp = modelType.GetProperty(KeyName);
            KeyType = keyProp.PropertyType;
            KeyTypeName = KeyType.Name;

            var descAttributes = modelType.GetCustomAttributes(typeof(DescriptionAttribute), true);
            Description = descAttributes.Length == 1 ? ((DescriptionAttribute)descAttributes[0]).Description : Name;
            Properties = modelType.GetProperties();


        }


    }
}
