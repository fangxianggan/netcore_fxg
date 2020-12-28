using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ProxyPattern
{
   public class TestRun
    {
        public TestRun()
        {
         
            ProxyFileUpload proxyFile = new ProxyFileUpload();
            proxyFile.UploadData();
        }
    }
}
