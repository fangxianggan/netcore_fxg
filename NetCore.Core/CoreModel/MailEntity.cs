using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Core.CoreModel
{
    public class MailEntity
    {
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SendName { set; get; }


        /// <summary>
        /// 发送者账号
        /// </summary>
        public string SendAccountName { set; get; }

        /// <summary>
        /// 发送者服务器地址
        /// </summary>
        public string SmtpHost { set; get; }


        /// <summary>
        /// 服务器端口号：例如：25
        /// </summary>
        public int SmtpPort { set; get; }


        /// <summary>
        /// 发送者登录邮箱账号的客户端授权码 邮箱密码
        /// </summary>
        public string AuthenticatePassword { set; get; }

        /// <summary>
        /// 接收者账号
        /// </summary>
        public List<string> ReceiverAccountNameList { set; get; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string MailSubject { set; get; }
        /// <summary>
        /// 文本html(与sendText参数互斥，传此值则 sendText传null)
        /// </summary>
        public string SendHtml { set; get; }
        /// <summary>
        /// 纯文本(与sendHtml参数互斥，传此值则 sendHtml传null)
        /// </summary>
        public string SendText { set; get; }

        /// <summary>
        /// 邮件的附件 
        /// </summary>
        public List<MimePart> AccessoryList { set; get; }

        //默认值设置
        public MailEntity()
        {
            SendName = "fangxianggan1991";
            SendAccountName = "fangxianggan1991@163.com";
            ReceiverAccountNameList = new List<string>() { "1097177647@qq.com", "939011009@qq.com", "936945202@qq.com" };
            AuthenticatePassword = "WSYYXDSPWPYVYRMK";//网易的授权码
            SmtpHost = "smtp.163.com";
            SmtpPort = 25;
            AccessoryList = null;
        }
    }
}
