﻿using NetCore.EntityFrameworkCore.EntityModels;
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
    /// 用户表
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo : BaseEntity<Guid>
    {

        /// <summary>
        /// 用户编号
        /// </summary>
        /// 
        [DisplayName("用户编号"), MaxLength(32)]
        public string UserCode { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        /// 
        [DisplayName("用户名"), MaxLength(64)]
        public string UserName { set; get; }

        /// <summary>
        /// 昵称
        /// </summary>
        /// 
        [DisplayName("昵称"), MaxLength(64)]
        public string NickName { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码"), MaxLength(64)]
        public string Password { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName("性别")]
        public int Gender { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        /// 
        [DisplayName("手机号"), MaxLength(32)]
        public string PhoneNumber { set; get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// 
        [DisplayName("邮箱"), MaxLength(64)]
        public string Email { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        public int State { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("头标"),MaxLength(200)]
        public string Avatar { set; get; }


    }
}
