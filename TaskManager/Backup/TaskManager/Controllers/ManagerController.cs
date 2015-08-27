using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskManager.Models;
using TaskManager.Realizations;
using WebMatrix.WebData;

namespace TaskManager.Controllers
{
    public class ManagerController : Controller
    {
        /// <summary>
        /// Instance RealizeManager : IManager is
        /// </summary>
        private readonly IManager m_Realize;
        public ManagerController(IManager value)
        {
            m_Realize = value;
        }

        /// <summary>
        /// Variable is for User errors on Views
        /// </summary>
        private string m_ResultMessage = "Пусто";

        /// <summary>
        /// Variable is login Authorized user
        /// </summary>
        private string m_Login = WebSecurity.CurrentUserName;

        /// <summary>
        /// Variable is Task Status like "Active status"
        /// </summary>
        private string m_StatusActive = "Активный";
        
        /// <summary>
        /// View of Review list of Tasks, way adding of task
        /// </summary>
        /// <returns>View Index/Account</returns>
        [Authorize]
        public ActionResult Index(int tagId = 0, string date = "")
        {
            DateTime dateTime = default(DateTime);
            if (date != "")
            {
                dateTime = DateTime.Parse(date);
            }
            
            // Auto-Updating all user's tasks that is updating of statuses of the every task
            m_Realize.UpdateStatusEachTask(m_Realize.GetCurrentUser(m_Login).UserId);
            return View(new TasksAddChangeSelect()
            {
                SelectTasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, tagId, dateTime),
                SelectTags = m_Realize.GetTags(m_Realize.GetCurrentUser(m_Login).UserId),
                CurStatus = m_StatusActive,
                CurLogin = m_Login,
                CurId = m_Realize.GetCurrentUser(m_Login).UserId
            });
        }

        [Authorize]
        public JsonResult FiltrTasksByTag(int tagId)
        {
            IEnumerable<Tasks> tasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, tagId, default(DateTime));
            return new JsonResult { Data = tasks, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        /// <returns>View Index/Manager</returns>
        [Authorize]
        public JsonResult SetEndStatus(int idTask)
        {
            string status = m_Realize.EndActTask(idTask);
            //return RedirectToAction("Index", "Manager", new { m_ResultMassage = status });
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonResult SetActiveStatus(int idTask)
        {
            string status = m_Realize.ChangeStatus(idTask);
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


       


        /// <summary>
        /// Adding and Changing of tasks
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>View Index/Manage</returns>
        [Authorize]
        [HttpPost]
        public ActionResult AddChange(TasksAddChangeSelect model)
        {
            if (model.AddChange.TaskId == 0)
            {
                m_ResultMessage = m_Realize.InsertTask(model.AddChange);
            }
            else
            {
                m_ResultMessage = m_Realize.UpdateTask(model.AddChange);
            }
            return RedirectToAction("Index", "Manager", new { m_ResultMessage });
        }

        /// <summary>
        /// Getting list of Tags with inputted Keyword [AJAX]
        /// </summary>
        /// <param name="name">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        [Authorize]
        public JsonResult GetTags(string name)
        {
            IEnumerable<Tags> tags = m_Realize.GetTags(name);
            return new JsonResult { Data = tags, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Removing of tasks [HttpPost] [Ajax]
        /// </summary>
        /// <param name="idTask">Id of task</param>
        /// <returns>Massage of id task on page</returns>
        [Authorize]
        public JsonResult Delete(int idTask)
        {
            string messageDelete = "Задача id " + idTask + " удалена";
            if (m_Realize.DeleteTask(idTask) == false)
            {
                messageDelete = "Выберите задачу";
            }
            var tasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, 0, default(DateTime));
            return new JsonResult { Data = tasks, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Dynamic opening of detail task on the Index page
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Set of property for Angular act</returns>
        [Authorize]
        public JsonResult OpenTask(int taskId)
        {
            Array values = m_Realize.GetValuesTask(taskId);
            return new JsonResult { Data = values, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public ActionResult Tasks()
        {
            return View();

        }

    }
}
