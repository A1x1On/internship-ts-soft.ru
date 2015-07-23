using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager.Realizations;
using WebMatrix.WebData;


namespace TaskManager.Controllers
{
    public class ManagerController : Controller
    {

        private string m_ResultMassage = "Пусто";

        private readonly IManager m_Relize = new RealizeManager();



        /// <summary>
        /// Selecting list of Tasks
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            int @id = Convert.ToInt32(m_Relize.CurrentUser(WebSecurity.CurrentUserName));
            ViewData["Query"] = m_Relize.TaskSelect(@id);
            return View();
        }

        /// <summary>
        /// Adding tasks into the data base
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Add(TASKS model)
        {
            try
            {
                m_Relize.TaskAdd(model);
                m_ResultMassage = "Задача была добавлена!";
            }
            catch (Exception)
            {
                m_ResultMassage = "Ошиба добавления";
            }
            return RedirectToAction("Index", "Manager", new { m_ResultMassage });
        }

    }
}
