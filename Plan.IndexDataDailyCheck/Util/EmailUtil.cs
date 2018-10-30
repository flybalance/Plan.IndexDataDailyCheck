using System;
using System.Net.Mail;
using System.Text;

namespace Utils
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class EmailUtil
    {
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="from">邮箱账号</param>
        /// <param name="pwd">邮箱密码</param>
        /// <param name="tomailList">接收邮箱（Ps:多个时以英文“,”分开）</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static bool SendMail(string host, string from, string pwd, string tomailList, string subject, string body, string excelPath)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = host;
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(from, pwd);
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(from);

                foreach (string str in tomailList.Split(','))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        Message.To.Add(str);
                    }
                }
                Message.Subject = subject;
                if (!string.IsNullOrEmpty(excelPath))
                {
                    Message.Attachments.Add(new Attachment(excelPath));
                }
                Message.SubjectEncoding = Encoding.UTF8;
                Message.BodyEncoding = Encoding.UTF8;
                Message.IsBodyHtml = true;
                try
                {
                    Message.Body = body;
                    client.Send(Message);
                    return true;
                }
                catch (Exception ex)
                {
                    Log4NetUtil.WriteErrorLog("错误参数：" + tomailList + "\n" + ex.ToString());
                    return false;
                }
            }
        }
    }
}