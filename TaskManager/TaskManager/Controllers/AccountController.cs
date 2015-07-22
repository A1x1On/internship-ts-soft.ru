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
using System.Web.Services.Description;
using SRVTextToImage;
using TaskManager.Realizations;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {
        
        /// <summary>
        /// 
        /// </summary>
        private string m_ResultMassage = "Капча введена неверно";

        /// <summary>
        /// 
        /// </summary>
        private readonly IAccount m_relize = new RealizeAccount();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            return View();
        }

        /// <summary>
        /// 
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
        /// <returns>Redirect to Index of Manager controller having string m_ResultMassage parametre containing result of UserToDb(u) method</returns>
        [HttpPost]
        public ActionResult Registration(USERS u)
        {
             if (this.Session["CapthaImageText"].ToString() == u.CAPCHA)
             {
                 if (ModelState.IsValid)
                 {
                     m_ResultMassage = m_relize.UserToDb(u); //IAccount
                     ModelState.Clear();
                 }
                 else
                 {
                     m_ResultMassage = "Капча верна но данные не корректны";
                 } 
             }
             return RedirectToAction("Index", "Manager", new { m_ResultMassage });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult ConfirMail(string Code)
        {
            if (Code != null)
            {
                m_ResultMassage = m_relize.UserConfirm(Code);
                return RedirectToAction("Index", "Manager", new { m_ResultMassage });
            }
            return View(); 
        }

        /// <summary>
        /// 
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
    }
}
