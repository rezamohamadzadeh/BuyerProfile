using Service;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirmation",
                $"Please Enter the Link: <a href='{HtmlEncoder.Default.Encode(link)}'>Link</a>");
        }
        public static Task SendDownladLinkAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Download Link",
                $"Please Enter the Link: <a href='{HtmlEncoder.Default.Encode(link)}'>Link</a>");
        }
        public static Task SendDownladLinkAsync(this IEmailSender emailSender, string email, string link,string title,string text)
        {
            return emailSender.SendEmailAsync(email, $"{title}",
                $"{text} <br> Please Enter the Link: <a href='{HtmlEncoder.Default.Encode(link)}'>Link</a>");
        }
    }
}
