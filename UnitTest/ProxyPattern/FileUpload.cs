using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ProxyPattern
{
    public class FileUpload : IFileUpload
    {
        public FileUpload()
        {
            
        }
        public void UploadData()
        {
            Console.WriteLine("上传文件！！！");
        }
    }
}
