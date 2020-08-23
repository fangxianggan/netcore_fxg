using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel
{
   public class FileUploadCheckChunkViewModel
    {
        public string Md5 { set; get; }

        public string Ext { set; get; }

        public string RootPath { set; get; }
    }
}
