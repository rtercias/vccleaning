using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using VCCleaning;
using System.Configuration;

namespace VCCleaning.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() { return View(); }
        public ActionResult Residential() { return View(); }       
        public ActionResult Office() { return View(); }
        public ActionResult AreaRugs() { return View(); }
        public ActionResult Upholstery() { return View(); }
        public ActionResult PetProblems() { return View(); }
        public ActionResult GreenCleaning() { return View(); }
        public ActionResult GetQuote() { return View(); }
        public ActionResult AboutUs() { return View(); }
        public ActionResult ContactUs() { return View(); }

        [RecaptchaValidator]
        [JsonExceptionFilter]
        public JsonResult SendEmail(Quote quote, bool captchaValid)
        {
            string message = "";
            //validate
            if (quote.Email == null || !quote.Email.Contains("@"))
            {
                throw new ArgumentException("Invalid email address.");
            }

            //is recaptcha valid
            if (!captchaValid)
            {
                throw new ArgumentException("Word test failed.");
            }

            
            string phoneNumber = ConfigurationManager.AppSettings["phoneNumber"].ToString();
            string quoteEmail = ConfigurationManager.AppSettings["quoteEmail"].ToString();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mail.From = new MailAddress(quote.Email, quote.Name);
                mail.To.Add(quoteEmail);
                mail.Subject = string.Format("New quote request: {0}", quote.Name);
                mail.Body += "You received a new quote request from your Valley Carpet Cleaning website.\r\n";
                mail.Body += string.Format("Name: {0}\r\n", quote.Name);
                mail.Body += string.Format("Phone: {0}\r\n", quote.Phone);
                mail.Body += string.Format("Email: {0}\r\n", quote.Email);
                mail.Body += string.Format("Service: {0}\r\n", quote.Service);
                mail.Body += string.Format("Time Frame: {0}\r\n", quote.TimeFrame);
                mail.Body += string.Format("Description: {0}\r\n", quote.Description);
                smtp.Send(mail);
                message = "Thank you! We will contact you as soon as possible.";
            }
            catch
            {
                message = string.Format("We were unable to send the email. Please contact {0} directly.", phoneNumber);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}
