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
        public ActionResult Index()
        {
           

            // Auto-Updating all user's tasks that is updating of statuses of the every task
            m_Realize.UpdateStatusEachTask(m_Realize.GetCurrentUser(m_Login).UserId);

            //IEnumerable<Tasks> tasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, tagId, dateTime);
            IEnumerable<Tags> tags = m_Realize.GetTags(m_Realize.GetCurrentUser(m_Login).UserId);
            IEnumerable<DateTasks> dates = m_Realize.GetDates(m_Realize.GetCurrentUser(m_Login).UserId);
            int userId = m_Realize.GetCurrentUser(m_Login).UserId;
            return View(new TasksAddChangeSelect()
            {
                //SelectTasks = tasks,
                SelectTags = tags,
                SelectDates = dates,
                CurStatus = m_StatusActive,
                CurLogin = m_Login,
                CurId = userId
            });
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

    }
}
