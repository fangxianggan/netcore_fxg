using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : BaseAttribute
    {
        private string _Fields;
        /// <summary>  
        /// 字段名称  
        /// </summary>  
        public string Fields
        {
            get { return _Fields; }

        }

        private DbType _Dbtype;
        /// <summary>  
        /// 字段类型  
        /// </summary>  
        public DbType Dbtype
        {
            get { return _Dbtype; }

        }

        private int _ValueLength;
        /// <summary>  
        /// 字段值长度  
        /// </summary>  
        public int ValueLength
        {
            get { return _ValueLength; }

        }
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="fields"> 字段名</param>  
        /// <param name="types"> 字段类型</param>  
        /// <param name="i"> 字段值长度</param>  
        public FieldAttribute(string fields, DbType types, int i)
        {

            _Fields = fields;
            _Dbtype = types;
            _ValueLength = i;
        }
    }  
}
