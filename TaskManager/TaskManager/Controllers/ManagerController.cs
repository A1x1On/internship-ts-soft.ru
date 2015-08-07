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
        private readonly IManager m_Relize;
        public ManagerController(IManager value)
        {
            m_Relize = value;
        }

        /// <summary>
        /// Variable is for User errors on Views
        /// </summary>
        private string m_ResultMassage = "Пусто";

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
            m_Relize.CommonUpdateStatus(m_Relize.CurrentUser(m_Login).USERID);

            // Some info for current View and model TASKS(void) and IEnumerable<TASKS> for _PartialSelectionTasks
            return View(new TasksAddChangeSelect()
            {
                SelecTasks = m_Relize.TaskSelect(m_Relize.CurrentUser(m_Login).USERID),
                CurStatus = m_StatusActive,
                CurLogin = m_Login,
                CurId = m_Relize.CurrentUser(m_Login).USERID
            });
        }

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        /// <returns>View Index/Manager</returns>
        [Authorize]
        [HttpPost]
        public ActionResult Finish(int TaskFromFinish)
        {
            return RedirectToAction("Index", "Manager", new { m_ResultMassage = m_Relize.TaskStatusFin(TaskFromFinish) });
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
            if (model.AddChange.TASKID == 0)
            {
                m_ResultMassage = m_Relize.TaskAdd(model.AddChange);
            }
            else
            {
                m_ResultMassage = m_Relize.TaskChange(model.AddChange);
            }
            return RedirectToAction("Index", "Manager", new { m_ResultMassage });
        }

        /// <summary>
        /// Getting list of Tags with inputted Keyword [AJAX]
        /// </summary>
        /// <param name="name">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        [Authorize]
        public JsonResult GetTags(string name)
        {
            return new JsonResult { Data = m_Relize.GettingTags(name), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Removing of tasks [HttpPost] [Ajax]
        /// </summary>
        /// <param name="idTask">Id of task</param>
        /// <returns>Massage of id task on page</returns>
        [Authorize]
        public JsonResult Delete(int idTask)
        {
            m_Relize.TaskDelete(idTask);
            return new JsonResult { Data = "Удалил: " + idTask, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Dynamic opening of detail task on the Index page
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns>Set of property for Angular act</returns>
        [Authorize]
        public JsonResult OpenTask(int TaskId)
        {
            return new JsonResult { Data = m_Relize.TaskOpen(TaskId), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
