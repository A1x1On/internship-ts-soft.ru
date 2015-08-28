using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using TaskManager.Realizations;
using WebMatrix.WebData;

namespace TaskManager.Controllers.api
{

    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        /// <summary>
        /// Instance RealizeManager : IManager is
        /// </summary>
        IManager m_Realize = new RealizeManager();

        /// <summary>
        /// Variable is login Authorized user
        /// </summary>
        private string m_Login = WebSecurity.CurrentUserName;




        [Route("GetTasks")]
        public IEnumerable<Tasks> GetTasks()
        {

            //int tagId = 0, string date = ""
            //DateTime dateTime = default(DateTime);
            //if (date != "")
            //{
            //    dateTime = DateTime.Parse(date);
            //}

            List<Tasks> TaskWithTags = new List<Tasks>();


            IEnumerable<Tasks> tasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, 0, default(DateTime));

            IEnumerable<Tags> tags = null;


            foreach (var t in tasks)
            {


                TaskWithTags.Add(new Tasks()
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    TaskTerm = t.TaskTerm,
                    UsId = t.UsId,
                    StatusString = t.StatusString,
                    Tags = m_Realize.GetTagsOfTask(t.TaskId)
                });
   
                
            }



            return TaskWithTags;
        }


       




        /// <summary>
        /// Dynamic opening of detail task on the Index page
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Set of property for Angular act</returns>
        [Route("GetValuesTask")]
        public Array GetValuesTask(int id)
        {
            Array values = m_Realize.GetValuesTask(id);
            return values;
        }

        /// <summary>
        /// Removing of tasks
        /// </summary>
        /// <param name="idTask">Id of task</param>
        /// <returns>Massage of id task on page</returns>
        [Route("DeleteTask")]
        public IEnumerable<Tasks> GetTasks(int idTask)
        {
            string messageDelete = "Задача id " + idTask + " удалена";
            if (!m_Realize.DeleteTask(idTask))
            {
                messageDelete = "Выберите задачу";
            }
            IEnumerable<Tasks> tasks = m_Realize.GetTasks(m_Realize.GetCurrentUser(m_Login).UserId, 0, default(DateTime));
            return tasks;
        }

        /// <summary>
        /// Setting activity of tasks
        /// </summary>
        /// <param name="idTask"></param>
        /// <returns></returns>
        [Route("SetActiveStatus")]
        public string SetActiveStatus(int idTask)
        {
            string status = m_Realize.ChangeStatus(idTask);
            return status;
        }

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        /// <returns>View Index/Manager</returns>
        [Route("SetEndStatus")]
        public string SetEndStatus(int idTask)
        {
            string status = m_Realize.EndActTask(idTask);
            return status;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}