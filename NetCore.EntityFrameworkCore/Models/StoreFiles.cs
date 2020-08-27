using NetCore.EntityFrameworkCore.EntityModels;
using NetCore.EntityFrameworkCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.EntityFrameworkCore.Models
{
    /// <summary>
    /// 存储文件
    /// </summary>
    [Table("StoreFiles")]
    public class StoreFiles: BaseEntity<Guid>
    {

        /// <summary>
        /// 文件名称
        /// </summary>
        [DisplayName("文件名称"), MaxLength(128)]
        public string FileName { set; get; }


        /// <summary>
        /// 文件的相对路径
        /// </summary>
        /// 
        [DisplayName("相对路径"), MaxLength(128)]
        public string RelationFilePath { set; get; }


        /// <summary>
        /// 文件后缀
        /// </summary>
        [DisplayName("文件后缀"), MaxLength(8)]
        public string FileExt { set; get; }


        /// <summary>
        /// 文件大小 b
        /// </summary>
        /// 
        [DisplayName("文件大小")]
        public long FileBytes { set; get; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [DisplayName("文件类型"), MaxLength(16)]
        public string FileType { set; get; }


        



    }
}
