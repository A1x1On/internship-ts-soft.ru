using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TaskManager.Realizations;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private string m_ResultMassage = "";
        public ActionResult Index()
        {
            return View();
        }

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
            RealizeAccount reg = new RealizeAccount();
            
             //if (this.Session["CapthaImageText"].ToString() == CaptchaText.ToString())
            //{
                if (ModelState.IsValid)
                {  
                    m_ResultMassage = reg.UserToDb(u);
                    ModelState.Clear();

                }
                
          //  }
                return RedirectToAction("Index", "Manager", new { m_ResultMassage });
  
        }


    }
}
