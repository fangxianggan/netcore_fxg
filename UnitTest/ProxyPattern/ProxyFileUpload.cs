using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ProxyPattern
{
    public class ProxyFileUpload : IFileUpload
    {
        //直接去实例化 我们要控制的对象业务
        private FileUpload file;

        //我们处理的一个类
        private ValidateFile validate;

        public ProxyFileUpload()
        {
            validate = new ValidateFile();
            file = new FileUpload();
        }
        public void UploadData()
        {
            validate.ValidateData();
            file.UploadData();
        }


    }
}
