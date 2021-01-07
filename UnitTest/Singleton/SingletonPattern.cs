using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Singleton
{
    /// <summary>
    /// 单例模式  案例
    /// </summary>
    /// 
    [Serializable]
    public class SingletonPattern
    {

        /// <summary>
        ///1. 构造函数私有化 不许外部 实例化
        /// </summary>
        private SingletonPattern()
        {
            Console.WriteLine("构造函数初始化.......");
        }
        /// <summary>
        /// 3. 构建一个静态的私有函数 用于返回实例
        /// </summary>
        private static SingletonPattern _singletonPattern = new SingletonPattern()
        {
            Id = 0,
            Name = "ss",
            Class =new Class {  ClassID=1, ClassName="fff"}
           
        };

     

        /// <summary>
        /// 2. 创建一个静态对外访问的实例  浅克隆
        /// </summary>
        /// <returns></returns>
        public static SingletonPattern CreateInstance()
        {

            //return _singletonPattern;
            //  return (SingletonPattern)_singletonPattern.MemberwiseClone();
            _singletonPattern = (SingletonPattern)_singletonPattern.MemberwiseClone();

            ////1 深度克隆  
            //_singletonPattern.Class = new Class()
            //{
            //    ClassID = _singletonPattern.Class.ClassID,
            //    ClassName = _singletonPattern.Class.ClassName
            //};


            ///2  通过序列化方式 深度克隆
            _singletonPattern = SerializeHelper.DeepClone(_singletonPattern);

            return _singletonPattern;
        }

        /// <summary>
        /// 打印方法
        /// </summary>
        public void PrintData()
        {
            Console.WriteLine($"Id：{this.Id},Name：{this.Name},ClassID：{this.Class.ClassID},ClassName：{this.Class.ClassName}");
        }
        /// <summary>
        /// 属性 id
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 属性 name
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 对象 属性
        /// </summary>
        public Class Class { set; get; }


    }
    [Serializable]
    public class Class
    {
        public int ClassID { set; get; }

        public string ClassName { set; get; }
    }
}

