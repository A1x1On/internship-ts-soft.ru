using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using TaskManager.Models;

namespace TaskManager.Realizations
{
    public class RealizeManager : IManager
    {
        /// <summary>
        /// Instance EntitieDatabase is
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();

        /// <summary>
        /// Amount of string
        /// </summary>
        private int m_CountTag;

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        public string TaskStatusFin(int TaskFromFinish)
        {
            try
            {
                var value = m_db.TASKS.Where(x => x.TASKID.Equals(TaskFromFinish)).FirstOrDefault();
                value.TASKSTATUS = "Завершен";
                m_db.TASKS.AddOrUpdate(value);
                m_db.SaveChanges();
                return "Задача завершина";
            }
            catch (Exception)
            {
                return "Ошибка! Задача не завершина";
            }
        }

        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the evrey task
        /// </summary>
        public void CommonUpdateStatus()
        {
            foreach (TASKS t in m_db.TASKS)
            {
                if (t.TASKSTATUS != "Завершен")
                {
                    TASKS tup = new TASKS()
                    {
                        TASKSTATUS = FromingStatus(t.TASKTERM),
                        USID = t.USID,
                        TASKID = t.TASKID,
                        DISCRIPTION = t.DISCRIPTION,
                        TAGS = t.TAGS,
                        TITLE = t.TITLE,
                        TASKTERM = t.TASKTERM,
                        USERS = t.USERS
                    };
                    m_db.TASKS.AddOrUpdate(tup); 
                }
            }
            m_db.SaveChanges();
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="TaskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array TaskOpen(int TaskId)
        {
            string[] setPropertyTask = new string[6];
            var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == TaskId);
            setPropertyTask[0] = value.TITLE;
            setPropertyTask[1] = value.DISCRIPTION;
            setPropertyTask[2] = value.TASKTERM;
            setPropertyTask[3] = value.TASKSTATUS;
            setPropertyTask[4] = value.TAGS;
            setPropertyTask[5] = value.TASKID.ToString();
            return setPropertyTask;
        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string TaskAdd(TASKS model)
        {
            try
            {
                TASKS TheTask = new TASKS()
                {
                    TITLE = model.TITLE,
                    DISCRIPTION = model.DISCRIPTION,
                    TASKTERM = model.TASKTERM,
                    USID = model.USID,
                    TASKSTATUS = model.TASKSTATUS,
                    TAGS = TagsAdd(model.TAGS)
                };
                m_db.TASKS.Add(TheTask);
                m_db.SaveChanges();
                return "Задача сохранена";
            }
            catch (Exception)
            {
                return "Ошибка изминения задачи";
            }
        }

        
        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string TaskChange(TASKS model)
        {
            try
            {
                var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == model.TASKID);
                if (value != null)
                {
                    TASKS TheTask = new TASKS()
                    {
                        TASKID = model.TASKID,
                        TITLE = model.TITLE,
                        DISCRIPTION = model.DISCRIPTION,
                        TASKTERM = model.TASKTERM,
                        USID = model.USID,
                        TASKSTATUS = model.TASKSTATUS,
                        TAGS = TagsAdd(model.TAGS)
                    };
                    m_db.TASKS.AddOrUpdate(TheTask);
                    m_db.SaveChanges();
                }
                return "Задача изменена";
            }
            catch (Exception)
            {
                return "Задача изменена";
            } 
        }

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="TaskId">Id of task</param>
        public void TaskDelete(int TaskId)
        {
            var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == TaskId);
            if (value != null)
            {
                m_db.Set<TASKS>().Remove(value);
                m_db.SaveChanges();
            }
        }

        /// <summary>
        /// Getting of tasks' list
        /// </summary>
        /// <param name="CurId">Authoeized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        public IEnumerable<TASKS> TaskSelect(int CurId)
        {
           IEnumerable<TASKS> Query = from SelectionTasks in m_db.TASKS
                where SelectionTasks.USID == CurId
                orderby SelectionTasks.TASKID
                select SelectionTasks;

            return Query;
           
        }

        /// <summary>
        /// Getting of Tags with inputed Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="TagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<TAGS> GettingTags(string TagKeyword)
        {
            IEnumerable<TAGS> QueryTags = from t in m_db.TAGS
                where t.TITLETAG.Contains(TagKeyword)
                orderby t.ID
                select t;
            return QueryTags.ToArray();
        }

        ///The others Methods///

        /// <summary>
        /// Adding of tags to DB or not
        /// </summary>
        /// <param name="TagRow">Got string from (text input of class="inputTag")</param>
        /// <returns>String of formed tags</returns>
        public string TagsAdd(string TagRow)
        {
            string @FinalTag = "";
            TAGS @TagRes;
            TagRow = TagRow.ToLower();
            foreach (Match t in Regex.Matches(TagRow, @"([\b\w\-\w\b]+)"))
            {
                @TagRes = m_db.TAGS.Where(x => x.TITLETAG == t.Value).FirstOrDefault();
                if (@TagRes != null)
                {
                    @FinalTag = @FinalTag + ", " + t.Value;
                }
                else
                {
                    @FinalTag = @FinalTag + ", " + t.Value;
                    TAGS TheTag = new TAGS()
                    {
                        TITLETAG = t.Value
                    };
                    m_db.TAGS.Add(TheTag);
                    m_db.SaveChanges();
                }
            }
            m_CountTag = @FinalTag.Length - 2;
            return @FinalTag.Substring(2, m_CountTag);
        }

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="SafetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        public USERS CurrentUser(string SafetyLogin)
        {
            return m_db.USERS.Where(x => x.LOGIN_NAME.Equals(SafetyLogin)).FirstOrDefault();
        }

        /// <summary>
        /// Froming of Status
        /// </summary>
        /// <param name="Userdate">Own user of date from DataBase</param>
        /// <returns>Status of task</returns>
        public string FromingStatus(string Userdate)
        {
            DateTime curDate = DateTime.Now;
            string m_StrDYear = "";
            string m_StrMonth = "";
            string m_StrDay = "";
            string m_DateTask = "";
            string m_DateCurrent = "";

            m_StrDYear = Userdate.Substring(0, 4);
            m_StrMonth = Userdate.Substring(5, 2);
            m_StrDay = Userdate.Substring(8, 2);

            m_DateTask = m_StrDYear + "." + m_StrMonth + "." + m_StrDay;
            m_DateCurrent = curDate.Year + "." + curDate.Month + "." + curDate.Day;
            if (Convert.ToDateTime(m_DateTask) < Convert.ToDateTime(m_DateCurrent))
            {
                return "Потрачено";
            }
            if (Convert.ToDateTime(m_DateTask) == Convert.ToDateTime(m_DateCurrent))
            {
                return "Сегодня последний день";
            }
            return "Активный";
        }
    }
}