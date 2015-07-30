using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using SRVTextToImage;
using TaskManager.Models;
using TaskManager.Realizations;
using WebMatrix.WebData;

namespace TaskManager.Controllers
{
    /// <summary>
    /// public class AccountController : Controller
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Message: Error Capcha
        /// </summary>
        private string m_ResultMassage = "Капча введена неверно";

        /// <summary>
        /// Instance RealizeAccount : IAccount is
        /// </summary>
        private readonly IAccount m_Relize = new RealizeAccount();

        /// <summary>
        /// Checking of Authorized User
        /// </summary>
        /// <returns>View Index/Manager or View Index/Account</returns>
        public ActionResult Index()
        {
            if (WebSecurity.CurrentUserName != "")
            {
                return RedirectToAction("Index", "Manager");
            }
            else
            {
                return View();
            }
            
        }

        /// <summary>
        /// Registration View
        /// </summary>
        /// <returns></returns>
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Registration of User [HttpPost]
        /// </summary>
        /// <param name="u">Object of data register</param>
        /// <returns>View Index/Manager having string m_ResultMassage</returns>
        [HttpPost]
        public ActionResult Registration(USERS u)
        {
             if (this.Session["CapthaImageText"].ToString() == u.CAPCHA)
             {
                 if (ModelState.IsValid)
                 {
                     m_ResultMassage = m_Relize.UserToDb(u);
                     ModelState.Clear();
                 }
                 else
                 {
                     m_ResultMassage = "Капча верна но данные не корректны";
                 } 
             }
             return RedirectToAction("Index", "Account", new { m_ResultMassage });
        }

        /// <summary>
        /// Confirming of Account via own Email
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult ConfirMail(string Code)
        {
            if (Code != null)
            {
                return RedirectToAction("Index", "Manager", new { m_ResultMassage = m_Relize.UserConfirm(Code) });
            }
            return View(); 
        }

        /// <summary>
        /// Drawing of Capcha
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")]
        public FileResult GetCapthcaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CapthaImageText"] = CI.GetRandomString(5);
            CI.GenerateImage(this.Session["CapthaImageText"].ToString(), 300, 75, Color.DarkGreen, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        /// <summary>
        /// Login View [HttpPost]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogIn model)
        {
            string[] dataAuth = new string[2];
            if (ModelState.IsValid)
            {
                dataAuth = m_Relize.UserAuthorisation(model);
                if (dataAuth[1] == "true")
                {
                    m_ResultMassage = dataAuth[0];
                    return RedirectToAction("Index", "Manager", new { m_ResultMassage });
                }
                else
                {
                    m_ResultMassage = dataAuth[0];
                    return RedirectToAction("Index", "Account", new { m_ResultMassage });
                }
            }
            return RedirectToAction("Index", "Account");
        }

        /// <summary>
        /// Logout View [HttpPost]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logout()
        {
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "null");
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Account");
        }
    }
}
