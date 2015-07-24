using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace TaskManager.Realizations
{
    public interface IManager
    {
        int CurrentUser(string SafetyLogin);
        void TaskAdd(TASKS model);
        Array TaskSelect(int CurId);
        void TaskDelete(int TaskId);
        TASKS TaskChange(int TaskId);
        string TagsAdd(string TagRow);
        IEnumerable<TAGS> GettingTags(string TagKeyword);
    }

    public class RealizeManager : IManager
    {
        /// <summary>
        /// 
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();



        





        /// <summary>
        /// Getting task with TaskId for forward Changing task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public TASKS TaskChange(int TaskId)
        {
            var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == TaskId);
            return value;
        }

        /// <summary>
        /// Removing task
        /// </summary>
        /// <param name="TaskId"></param>
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
                TAGS = TagsAdd(model.TAGS)
            };
            m_db.TASKS.Add(TheTask);
            m_db.SaveChanges();
        }

        private int m_CountTag;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TagRow"></param>
        /// <returns></returns>
        public string TagsAdd(string TagRow)
        {
            string @FinalTag = "";
            TAGS @TagRes;
            TagRow = TagRow.ToLower();

            foreach (Match t in Regex.Matches(TagRow, @"([\b\w\-\w\b]+)"))
            {
                Debug.WriteLine("Уже есть: " + t.Value);

                @TagRes = m_db.TAGS.Where(x => x.TITLETAG == t.Value).FirstOrDefault();
                if (@TagRes != null)
                {
                    @FinalTag = @FinalTag + ", " + t.Value;
                }
                else
                {
                    Debug.WriteLine("Добавил: " + t.Value);
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
        /// Getting list of Tags with inputed Keyword 
        /// </summary>
        /// <param name="TagKeyword"></param>
        /// <returns></returns>
        public IEnumerable<TAGS> GettingTags(string TagKeyword)
        {
            IEnumerable<TAGS> QueryTags = from t in m_db.TAGS
                where t.TITLETAG.Contains(TagKeyword)
                orderby t.ID
                select t;
            return QueryTags.ToArray();
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