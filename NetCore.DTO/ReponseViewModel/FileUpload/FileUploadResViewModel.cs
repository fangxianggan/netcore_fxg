using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.FileUpload
{
    /// <summary>
    /// 分片上传 vue simple-uploader  
    /// </summary>
    public class FileUploadResViewModel
    {

        public string FileName { set; get; }

        public string Identifier { set; get; }
        /// <summary>
        /// 是否需要合并
        /// </summary>
        public bool NeedMerge { set; get; }

        /// <summary>
        /// 已经上传过的分片文件  断点续传
        /// </summary>
        public List<int> Uploaded { set; get; }

        /// <summary>
        ///是否秒传  
        /// </summary>
        public bool SecondTransmission { set; get; }


        /// <summary>
        /// 文件地址 用于秒传
        /// </summary>
        public string FilePathUrl { set; get; }

        /// <summary>
        /// 文件状态  0 新文件 1 传输过程中发生错误 已经有分片文件 可以续传  2 表示秒传 已经存在该文件
        /// </summary>
        public int FState { set; get; }

    }
}
