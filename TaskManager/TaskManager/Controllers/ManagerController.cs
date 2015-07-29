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
        /// View of Review list of Tasks, way adding task
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            int @id = Convert.ToInt32(m_Relize.CurrentUser(WebSecurity.CurrentUserName));
            @ViewBag.CurId = m_Relize.CurrentUser(WebSecurity.CurrentUserName).ToString();
            @ViewBag.CurStatus = "Активный";
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




        [Authorize]
        [HttpPost]
        public ActionResult AddChange(TASKS model)
        {
            if (model.TASKID == 0)
            {
                try
                {
                    m_Relize.TaskAdd(model);
                    m_ResultMassage = "Задача сохранена";
                }
                catch (Exception)
                {
                    m_ResultMassage = "Ошиба сохранения";
                }
            }
            else
            {
                try
                {
                    m_Relize.TaskChange(model);
                    m_ResultMassage = "Задача изменена";
                }
                catch (Exception)
                {
                    m_ResultMassage = "Ошибка изминения задачи";
                }
               

            }
            return RedirectToAction("Index", "Manager", new { m_ResultMassage });
        }



        /// <summary>
        /// Getting list of Tags with inputed Keyword [AJAX]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetTags(string name)
        {
            var tags = m_Relize.GettingTags(name);
            return new JsonResult { Data = tags, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // Removing task
        /// <summary>
        /// Removing tasks [HttpPost] [Ajax]
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int Id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                m_Relize.TaskDelete(Id); 
            }
            return Json(new { result = "Удалил: " + Id }, JsonRequestBehavior.AllowGet);
        }
        
        [Authorize]
        public JsonResult OpenTask(int TaskId)
        {
  
            Debug.WriteLine("Проверка: " + TaskId);
            Array task = m_Relize.TaskOpen(TaskId);
            


            return new JsonResult { Data = task, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        
        

    }
}
