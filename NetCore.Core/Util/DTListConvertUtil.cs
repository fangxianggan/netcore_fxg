
using NetCore.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCore.Core.Util
{

    /// <summary>
    /// DataTable与实体类互相转换
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public static class DTListConvertUtil<T> where T : new()
    {
        public static object ReflectionUtils { get; private set; }

        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public static List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public static List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i], null);
                }

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable(List<T> modelList)
        {
            List<string> propertyNameList = new List<string>();
            Type type = typeof(T);
            PropertyInfo[] properties = ReflectionUtil.GetProperties(type);
            foreach (PropertyInfo property in properties)
            {
                bool isInsert = true;
                object[] propertyAttrs = property.GetCustomAttributes(false);
                foreach (var propertyAttr in propertyAttrs)
                {
                    if (propertyAttr is IgnoreColumnAttribute)
                    {
                        isInsert = false;
                        break;
                    }
                }
                if (isInsert)
                {
                    propertyNameList.Add(property.Name);
                }
            }
            DataTable result = new DataTable();
            var tbAttrObj = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            if (tbAttrObj!=null) {
                var tbAttr = tbAttrObj as TableAttribute;
                if (!string.IsNullOrEmpty(tbAttr.Name))
                    result.TableName = tbAttr.Name;
                else
                    result.TableName = "";
            }
          
            if (modelList.Count > 0)
            {
                foreach (PropertyInfo pi in properties)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            if (pi.PropertyType == typeof(Guid))
                            {
                                result.Columns.Add(pi.Name, pi.PropertyType);
                            }
                            else if (pi.PropertyType.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Guid]")
                            {
                                result.Columns.Add(pi.Name, typeof(Guid));
                            }
                            else
                            {
                                result.Columns.Add(pi.Name);
                            }
                        }
                    }
                }

                for (int i = 0; i < modelList.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(modelList[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(modelList[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        public static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
