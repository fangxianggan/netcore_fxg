using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.RequestViewModel.FileUpload
{

    /// <summary>
    /// 分片上传 vue simple-uploader
    /// </summary>
    public class FileUploadReqViewModel
    {

        /// <summary>
        /// 当前块的次序，第一个块是 1，注意不是从 0 开始的。
        /// </summary>
        public int ChunkNumber { set; get; }


        /// <summary>
        /// 文件被分成块的总数。
        /// </summary>
        public int TotalChunks { set; get; }

        /// <summary>
        /// 分块大小，根据 totalSize 和这个值你就可以计算出总共的块数。注意最后一块的大小可能会比这个要大。
        /// </summary>
        public int ChunkSize { set; get; }

        /// <summary>
        /// 当前块的大小，实际大小。
        /// </summary>
        public int CurrentChunkSize { set; get; }

        /// <summary>
        /// 文件总大小
        /// </summary>
        public long TotalSize { set; get; }

        /// <summary>
        /// 唯一标识 md5
        /// </summary>
        public string Identifier { set; get; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// 文件夹上传的时候文件的相对路径属性。
        /// </summary>
        public string RelativePath { set; get; }

        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile File { set; get; }

        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string FileExt { set; get; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { set; get; }

        /// <summary>
        /// 文件分类
        /// </summary>
        public string FileCategory { set; get; }
    }
}
