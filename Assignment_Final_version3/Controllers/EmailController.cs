using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Assignment_Final_version3.Models;
using Assignment_Final_version3.Utils;

namespace Assignment_Final_version3.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    SendEmail es = new SendEmail();
                    es.Send(toEmail, subject, contents);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }
    }
}