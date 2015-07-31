using System.Web.Mvc;
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
        /// Varible is login Authorized user
        /// </summary>
        private string m_Login = WebSecurity.CurrentUserName;

        /// <summary>
        /// Varible is Task Status like "Active status"
        /// </summary>
        private string m_StatusActive = "Активный";
        
        /// <summary>
        /// View of Review list of Tasks, way adding of task
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            // Auto-Updating all user's tasks that is updating of statuses of the evrey task
            m_Relize.CommonUpdateStatus();

            // Some info for current View
            @ViewBag.CurId = m_Relize.CurrentUser(m_Login).USERID;
            @ViewBag.CurLogin = m_Login;
            @ViewBag.CurStatus = m_StatusActive;

            // Getting USERID for his Tasks to display
            ViewData["Query"] = m_Relize.TaskSelect(m_Relize.CurrentUser(m_Login).USERID);
            return View();
        }

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Finish(int TaskFromFinish)
        {
            return RedirectToAction("Index", "Manager", new { m_ResultMassage = m_Relize.TaskStatusFin(TaskFromFinish) });
        }

        /// <summary>
        /// Adding and Changing of tasks
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult AddChange(TASKS model)
        {
            if (model.TASKID == 0)
            {
                m_ResultMassage = m_Relize.TaskAdd(model);
            }
            else
            {
                m_ResultMassage = m_Relize.TaskChange(model);
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
            return new JsonResult { Data = m_Relize.GettingTags(name), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Removing of tasks [HttpPost] [Ajax]
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult Delete(int idTask)
        {
            m_Relize.TaskDelete(idTask);
            return new JsonResult { Data = "Удалил: " + idTask, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Dinamic opening of detail task on the Index page
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult OpenTask(int TaskId)
        {
            return new JsonResult { Data = m_Relize.TaskOpen(TaskId), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
