using System.Net.Mail;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Wkkim.Blog.Web.Models;
using Wkkim.Blog.Web.Models.ViewModels;
using Wkkim.Blog.Web.Repositories;

namespace Wkkim.Blog.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly string email_id;
        private readonly string email_password;
        private readonly IConfiguration configuration;

        public ContactController(ILogger<ContactController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration; 
            email_id = Environment.GetEnvironmentVariable("EMAIL_ID");
            email_password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendContact(string fullName, string emailAddress, string phoneNo, string message)
        {
            if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(emailAddress) && !string.IsNullOrEmpty(phoneNo) && !string.IsNullOrEmpty(message))
            {
                ContactForm contactUs = new ContactForm()
                {
                    FullName = fullName,
                    EmailAddress = emailAddress,
                    PhoneNo = phoneNo,
                    Message = message
                };

                if (SendEmail(contactUs))
                {
                    return Json(new { status = true, heading = "Congratulation", message = "Contact Form Submitted Successfully...!" });
                }
            }
            return Json(new { status = false, heading = "Oops", message = "Form Not Submitted. An Error Occur...!" });
        }

        public bool SendEmail(ContactForm query)
        {
            // 이메일 설정
            try
            {
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string subject = $"New Contact Request from Wkkim Blog";
                string body = $"Thank you for contacting me.<br>I will personally review this email and respond shortly.<br>Below are the name, email, and message of the person who requested to contact me.<br>Name: {query.FullName}<br>Email: {query.EmailAddress}<br>Phone Number:{query.PhoneNo}<br>Message:{query.Message}";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(email_id);
                    mail.To.Add(email_id);
                    mail.To.Add(query.EmailAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(email_id, "email_password");
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }

            return false;
        }
    }
}
