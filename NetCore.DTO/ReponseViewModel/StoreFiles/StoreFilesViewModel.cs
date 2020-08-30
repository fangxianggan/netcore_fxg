using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.StoreFiles
{
   public class StoreFilesViewModel
    {
        public Guid ID { set; get; }

        public string FileName { set; get; }

        public string FileType { set; get; }

        public long FileBytes { set; get; }

        public DateTime UploadTime { set; get; }

        public string Uploader { set; get; }

        public string FileCategory { set; get;}
    }
}
