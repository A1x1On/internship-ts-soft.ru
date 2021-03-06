﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SRVTextToImage;
using TaskManager.Models;
using TaskManager.Realizations;
using WebMatrix.WebData;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Message: Error Captcha
        /// </summary>
        private string m_ResultMessage = "Капча введена неверно";

        /// <summary>
        /// Instance RealizeAccount : IAccount is
        /// </summary>
        private readonly IAccount m_Realize;
        public AccountController(IAccount value)
        {
            m_Realize = value;
        }

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
        /// <returns>View Registration/Account</returns>
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Registration of User [HttpPost]
        /// </summary>
        /// <param name="person">Object of data register</param>
        /// <returns>View Index/Manager</returns>
        [HttpPost]
        public ActionResult Registration(Users person)
        {
            if (this.Session["CapthaImageText"].ToString() == person.Captcha)
             {
                 if (ModelState.IsValid)
                 {
                     m_ResultMessage = m_Realize.InsertUser(person);
                     ModelState.Clear();
                 }
                 else
                 {
                     m_ResultMessage = "Капча верна но данные не корректны";
                 } 
             }
            return RedirectToAction("Index", "Account", new { m_ResultMessage });
        }

        /// <summary>
        /// Confirming of Account via own Email
        /// </summary>
        /// <param name="code">Crypt password from user's email</param>
        /// <param name="l">Login of User</param>
        /// <returns>View Index/Manager</returns>
        public ActionResult ConfirMail(string code, string l)
        {
            string state = m_Realize.SetConfirmation(code, l);
            return RedirectToAction("Index", "Manager", new { m_ResultMassage = state });
        }

        /// <summary>
        /// Drawing of Captcha
        /// </summary>
        /// <returns>image of captcha</returns>
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
        /// <param name="model">Model of project for authorize on the site</param>
        /// <returns>View Index/Manager or View Index/Account</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogIn model)
        {
            string[] dataAuth = new string[2];
            if (ModelState.IsValid)
            {
                dataAuth = m_Realize.AuthUser(model);
                if (dataAuth[1] == "true")
                {
                    m_ResultMessage = dataAuth[0];
                    return RedirectToAction("Index", "Manager", new { m_ResultMessage });
                }
                else
                {
                    m_ResultMessage = dataAuth[0];
                    return RedirectToAction("Index", "Account", new { m_ResultMessage });
                }
            }
            return RedirectToAction("Index", "Account");
        }

        /// <summary>
        /// Logout View [HttpPost]
        /// </summary>
        /// <returns>View Index/Account</returns>
        [HttpPost]
        public ActionResult Logout()
        {
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "null");
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Account");
        }
    }
}
