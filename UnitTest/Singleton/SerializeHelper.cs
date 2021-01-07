using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace UnitTest.Singleton
{
    public class SerializeHelper
    {
        public static string Serializable(object target)
        {
            using (MemoryStream steam = new MemoryStream())
            {
                new BinaryFormatter().Serialize(steam, target);
                return Convert.ToBase64String(steam.ToArray());
            }
        }
        public static T Derializable<T>(string target)
        {
            byte[] targetArray = Convert.FromBase64String(target);
            using (MemoryStream steam = new MemoryStream(targetArray))
            {
                return (T)(new BinaryFormatter().Deserialize(steam));
            }
        }
        public static T DeepClone<T>(T t)
        {
            return Derializable<T>(Serializable(t));
        }
    }
}
