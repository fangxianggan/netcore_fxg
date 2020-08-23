using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace NetCore.Core.Util
{

    /// <summary>
    /// 修改之前和修改之后比较修改了那些字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CompareModelUtil<T> where T : class
    {
        public static ObjModel CompareModel(T oldModel, T newModel)
        {
            ObjModel model = new ObjModel();
            List<ValModel> oldList = new List<ValModel>();
            List<ValModel> newList = new List<ValModel>();
            string changeStr = string.Empty;
            PropertyInfo[] properties = oldModel.GetType().GetProperties();
            foreach (PropertyInfo item in properties)
            {
                var attR = item.GetCustomAttribute<DisplayNameAttribute>();
                if (attR != null)
                {
                    var displayName = attR.DisplayName;
                    string name = item.Name;
                    var oldValue = item.GetValue(oldModel) == null ? "" : item.GetValue(oldModel).ToString();
                    var newValue = item.GetValue(newModel) == null ? "" : item.GetValue(newModel).ToString();
                    if (!oldValue.Equals(newValue))
                    {
                        var key = displayName + "(" + name + ")";
                        oldList.Add(new ValModel() { Name = key, Value = oldValue.ToString() });
                        newList.Add(new ValModel() { Name = key, Value = newValue.ToString() });

                        //   var val = "由[" + oldValue + "] 改为[" + newValue + "]";
                    }
                }
            }
            model.OldModelList = oldList;
            model.NewModelList = newList;
            return model;
        }
    }


    public class ObjModel
    {
        /// <summary>
        ///修改之前的数据集合
        /// </summary>
        public List<ValModel> OldModelList { set; get; }

        /// <summary>
        /// 修改之后的数据集合
        /// </summary>
        public List<ValModel> NewModelList { set; get; }
    }

    public class ValModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }


        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
    }
}


