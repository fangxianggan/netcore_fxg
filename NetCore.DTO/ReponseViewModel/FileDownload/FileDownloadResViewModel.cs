using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ReponseViewModel.FileDownload
{
    public class FileDownloadResViewModel
    {
        public long From { set; get; }
        public long To { set; get; }
        public bool IsPartial { set; get; }
        public long Length { set; get; }
    }

    public class GenerateMD5ToBurstResViewModel
    {
        /// <summary>
        /// 唯一标识 md5
        /// </summary>
        public string Identifier { set; get; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// 文件总大小
        /// </summary>
        public long TotalSize { set; get; }

        /// <summary>
        /// 文件路径地址
        /// </summary>
        public string FilePathUrl { set; get; }

        /// <summary>
        /// 文件拆分片
        /// </summary>
        public List<FileRangeViewModel> FileRanges { set; get; }
    }

    public class FileRangeViewModel
    {
        /// <summary>
        /// 分片序号
        /// </summary>
        public int SliceNumber { set; get; }

        /// <summary>
        /// 分片小大
        /// </summary>
        public int ChunkSize { set; get; }

        /// <summary>
        /// 分片范围 
        /// </summary>
        public string Range { set; get; }


    }
}
