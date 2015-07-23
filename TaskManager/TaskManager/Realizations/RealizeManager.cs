using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TaskManager.Realizations
{
    public interface IManager
    {
        int CurrentUser(string SafetyLogin);
        void TaskAdd(TASKS model);
        Array TaskSelect(int CurId);
    }

    public class RealizeManager : IManager
    {
        /// <summary>
        /// 
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();


        /// <summary>
        /// Getting list of tasks
        /// </summary>
        /// <param name="CurId"></param>
        /// <returns></returns>
        public Array TaskSelect(int CurId)
        {
            var Query = from SelectionTasks in m_db.TASKS
                where SelectionTasks.USID == CurId
                orderby SelectionTasks.TASKID
                select SelectionTasks;
            return Query.ToArray();

        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void TaskAdd(TASKS model)
        {
            TASKS TheTask = new TASKS()
            {
                TITLE  = model.TITLE,
                DISCRIPTION = model.DISCRIPTION,
                TASKTERM = model.TASKTERM,
                USID = model.USID,
                TASKSTATUS = model.TASKSTATUS,
                TAGS = model.TAGS
            };
            m_db.TASKS.Add(TheTask);
            m_db.SaveChanges();
        }

        /// <summary>
        /// Getting User id
        /// </summary>
        /// <param name="SafetyLogin"></param>
        /// <returns></returns>
        public int CurrentUser(string SafetyLogin)
        {
            return m_db.USERS.Where(x => x.LOGIN_NAME.Equals(SafetyLogin)).FirstOrDefault().USERID;
        }
    }
}