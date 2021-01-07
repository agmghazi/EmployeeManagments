using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using EmployeeManagments.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace EmployeeManagments.Controllers
{
    public class MailController : Controller
    {
        private readonly IDNTCaptchaValidatorService _validatorService;

        public MailController(IDNTCaptchaValidatorService validatorService)
        {
            _validatorService = validatorService;
        }

        [HttpGet]
        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMail(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    {
                        this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                        return View();
                    }
                }

                using (MailMessage message = new MailMessage("gis.csharp@gmail.com", model.To))
                {
                    message.Subject = model.Subject;
                    message.IsBodyHtml = true;
                    message.Body = model.Body;

                    MailAddress copy = new MailAddress(model.CC);
                    message.CC.Add(copy);

                    if (model.Attachment == null)
                    {
                        ModelState.AddModelError("", "");
                    }
                    else if (model.Attachment.Length > 0)
                    {
                        string filename = Path.GetFileName(model.Attachment.FileName);
                        message.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), filename));
                    }
                    else
                    {
                        ModelState.AddModelError("", "");
                    }
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential credential = new NetworkCredential("gis.csharp@gmail.com", "Dev**27med20100");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credential;
                        smtp.Port = 587;
                        smtp.Send(message);
                        ViewBag.Message = "Email Sent Successfully";
                    }
                }
            }
            else
            {
                return View();
            }

            return View(model);
        }

    }
}
