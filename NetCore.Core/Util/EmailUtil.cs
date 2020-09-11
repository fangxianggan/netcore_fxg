using MailKit.Security;
using MimeKit;
using NetCore.Core.CoreModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Core.Util
{
    /// <summary>
    ///     邮件工具
    /// </summary>
    public class EmailUtil
    {


        //SendMail("发送人名称", "xxxxxxx@163.com", "smtp.163.com", 25, "xxxxxxxxx授权码", new List<string> { { "xxxxxx@qq.com" } }, "邮件主题", @" <p>邮件文本</p> ", null, accessoryList: new List<MimePart>()
        //    {
        //        {
        //            new MimeKit.MimePart("audio","mp4")
        //            {
        //                ContentObject = new MimeKit.ContentObject(File.OpenRead("C:\\Users\\lyj\\Desktop\\图片\\下载 (5).mp4"), MimeKit.ContentEncoding.Default),
        //                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
        //                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
        //                FileName = Path.GetFileName("C:\\Users\\lyj\\Desktop\\图片\\下载 (5).mp4")
        //            }
        //      },
        //        {
        //            new MimeKit.MimePart("audio","mp4")
        //            {
        //                ContentObject = new MimeKit.ContentObject(File.OpenRead("C:\\Users\\lyj\\Desktop\\图片\\下载 (3).mp4"), MimeKit.ContentEncoding.Default),
        //                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
        //                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
        //                FileName = Path.GetFileName("C:\\Users\\lyj\\Desktop\\图片\\下载 (3).mp4")
        //            }
        //        },
        //        {
        //            new MimeKit.MimePart("image","jpg")
        //            {
        //                ContentObject = new MimeKit.ContentObject(File.OpenRead("C:\\Users\\lyj\\Desktop\\图片\\11timg.jpg"), MimeKit.ContentEncoding.Default),
        //                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
        //                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
        //                FileName = Path.GetFileName("C:\\Users\\lyj\\Desktop\\图片\\11timg.jpg")
        //            }
        //        }
        //});

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="mail"></param>
        public static  void SendMail(MailEntity mail)
        {
            try {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(mail.SendName, mail.SendAccountName));
                var mailboxAddressList = new List<MailboxAddress>();
                mail.ReceiverAccountNameList.ForEach(f =>
                {
                    mailboxAddressList.Add(new MailboxAddress(f, f));
                });
                message.To.AddRange(mailboxAddressList);

                message.Subject = mail.MailSubject;

                var alternative = new Multipart("alternative");
                if (!string.IsNullOrWhiteSpace(mail.SendText))
                {
                    alternative.Add(new TextPart("plain")
                    {
                        Text = mail.SendText
                    });
                }

                if (!string.IsNullOrWhiteSpace(mail.SendHtml))
                {
                    alternative.Add(new TextPart("html")
                    {
                        Text = mail.SendHtml
                    });
                }
                var multipart = new Multipart("mixed");
                multipart.Add(alternative);
                if (mail.AccessoryList != null)
                {
                    mail.AccessoryList?.ForEach(f =>
                    {
                        multipart.Add(f);
                    });
                }
                message.Body = multipart;
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(mail.SmtpHost, mail.SmtpPort, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(mail.SendAccountName, mail.AuthenticatePassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            } catch (Exception ex) {

                LogUtil.Error(ex.Message);
            }
           
        }

    }
}
