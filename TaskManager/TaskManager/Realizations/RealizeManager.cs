using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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

        private SqlConnection m_Connection;

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
            //ADO.NET begin--
            try
            {
                string sqlUpdate = string.Format("UPDATE TASKS SET " +
                                                 "TASKSTATUS = '{0}'"+
                                                 "WHERE TASKID = {1}",
                                                 "Завершен", TaskFromFinish);
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConnection());
                cmdUpdate.ExecuteNonQuery();
                return "Задача завершина";
            }
            catch (Exception)
            {
                return "Задача не завершина";
            }
            //ADO.NET end--

            #region EF Ralization
            //try
            //{
            //    var value = m_db.TASKS.Where(x => x.TASKID.Equals(TaskFromFinish)).FirstOrDefault();
            //    value.TASKSTATUS = "Завершен";
            //    m_db.TASKS.AddOrUpdate(value);
            //    m_db.SaveChanges();
            //    return "Задача завершина";
            //}
            //catch (Exception)
            //{
            //    return "Задача не завершина";
            //}
            #endregion
        }


       

        private string m_TASKSTATUS = "";
        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the evrey task
        /// </summary>
        public void CommonUpdateStatus(int parUSID)
        {


            //ADO.NET begin--
            
            
                //Debug.WriteLine("Открыл базу");





                //string sqlChange = string.Format("UPDATE TASKS SET TASKSTATUS = '{0}' WHERE USID = '{1}' AND TASKSTATUS != '{2}'", m_TASKSTATUS, parUSID, "Завершен");
                //string sqlSel = string.Format("SELECT TASKTERM FROM TASKS WHERE USID = '{0}' AND TASKSTATUS != '{1}'", parUSID, "Завершен");
                //SqlCommand cmdChange = new SqlCommand(sqlChange, sqlConnection());
                //SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());

                //SqlDataReader reader = cmdSel.ExecuteReader();
                //while (reader.Read()) // если есть элемент прочитать
                //{
                //    m_TASKSTATUS = FromingStatus(reader["TASKTERM"].ToString());

                //    // выполнить для текущего элемента комманду cmdChange
                //}

            //ADO.NET end--

            #region EF Ralization
            foreach (TASKS t in m_db.TASKS)
            {
                if (t.TASKSTATUS != "Завершен" && t.USID == parUSID)
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
            #endregion
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="TaskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array TaskOpen(int TaskId)
        {
            string[] setPropertyTask = new string[6];

            string sqlSel = string.Format("SELECT * FROM TASKS WHERE TASKID = {0}", TaskId);
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                setPropertyTask[0] = reader["TITLE"].ToString();
                setPropertyTask[1] = reader["DISCRIPTION"].ToString();
                setPropertyTask[2] = reader["TASKTERM"].ToString();
                setPropertyTask[3] = reader["TASKSTATUS"].ToString();
                setPropertyTask[4] = reader["TAGS"].ToString();
                setPropertyTask[5] = reader["TASKID"].ToString();
            }
            return setPropertyTask;

            #region EF Ralization
            //var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == TaskId);
            //setPropertyTask[0] = value.TITLE;
            //setPropertyTask[1] = value.DISCRIPTION;
            //setPropertyTask[2] = value.TASKTERM;
            //setPropertyTask[3] = value.TASKSTATUS;
            //setPropertyTask[4] = value.TAGS;
            //setPropertyTask[5] = value.TASKID.ToString();
            //return setPropertyTask;
            #endregion
        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string TaskAdd(TASKS model)
        {
            //ADO.NET begin--
            try
            {
                string sqlAdd =
                    string.Format(
                        "INSERT INTO TASKS (TITLE, DISCRIPTION, TASKTERM, USID, TASKSTATUS, TAGS) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        model.TITLE,
                        model.DISCRIPTION,
                        model.TASKTERM,
                        model.USID,
                        model.TASKSTATUS,
                        TagsAdd(model.TAGS));
                SqlCommand cmdAdd = new SqlCommand(sqlAdd, sqlConnection());

                cmdAdd.ExecuteNonQuery();
                return "Задача сохранена";
            }
            catch (Exception)
            {
                return "Ошибка изминения задачи";
            }
            //ADO.NET end--

            #region EF Ralization
            //try
            //{
            //    TASKS TheTask = new TASKS()
            //    {
            //        TITLE = model.TITLE,
            //        DISCRIPTION = model.DISCRIPTION,
            //        TASKTERM = model.TASKTERM,
            //        USID = model.USID,
            //        TASKSTATUS = model.TASKSTATUS,
            //        TAGS = TagsAdd(model.TAGS)
            //    };
            //    m_db.TASKS.Add(TheTask);
            //    m_db.SaveChanges();
            //    return "Задача сохранена";
            //}
            //catch (Exception)
            //{
            //    return "Ошибка изминения задачи";
            //}
            #endregion
        }

        
        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string TaskChange(TASKS model)
        {
            //ADO.NET begin--
            try
            {
                string sqlUpdate = string.Format("UPDATE TASKS SET " +
                                                 "DISCRIPTION = '{0}', " +
                                                 "TASKSTATUS = '{1}', " +
                                                 "TITLE = '{2}', " +
                                                 "TAGS = '{3}', " +
                                                 "TASKTERM = '{4}', " +
                                                 "USID = {5}" +
                                                 "WHERE TASKID = {6}",
                    model.DISCRIPTION,
                    model.TASKSTATUS,
                    model.TITLE,
                    TagsAdd(model.TAGS),
                    model.TASKTERM,
                    model.USID,
                    model.TASKID);
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConnection());
                cmdUpdate.ExecuteNonQuery();
                return "Задача изменена";
            }
            catch (Exception)
            {
                return "Задача не изменена";
            }
            //ADO.NET end--

            #region EF Ralization
            //try
            //{
            //    var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == model.TASKID);
            //    if (value != null)
            //    {
            //        TASKS TheTask = new TASKS()
            //        {
            //            TASKID = model.TASKID,
            //            TITLE = model.TITLE,
            //            DISCRIPTION = model.DISCRIPTION,
            //            TASKTERM = model.TASKTERM,
            //            USID = model.USID,
            //            TASKSTATUS = model.TASKSTATUS,
            //            TAGS = TagsAdd(model.TAGS)
            //        };
            //        m_db.TASKS.AddOrUpdate(TheTask);
            //        m_db.SaveChanges();
            //    }
            //    return "Задача изменена";
            //}
            //catch (Exception)
            //{
            //    return "Задача не изменена";
            //} 
            #endregion
        }

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="TaskId">Id of task</param>
        public void TaskDelete(int TaskId)
        {        
            //ADO.NET begin--
            string sqlDel = string.Format("DELETE TASKS WHERE TASKID = '{0}'", TaskId);
            SqlCommand cmdDel = new SqlCommand(sqlDel, sqlConnection());
            cmdDel.ExecuteNonQuery();
            //ADO.NET end--

            #region EF Ralization
            //var value = m_db.TASKS.FirstOrDefault(c => c.TASKID == TaskId);
            //if (value != null)
            //{
            //    m_db.Set<TASKS>().Remove(value);
            //    m_db.SaveChanges();
            //}
            #endregion
        }

        /// <summary>
        /// Getting of tasks' list
        /// </summary>
        /// <param name="CurId">Authoeized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        public IEnumerable<TASKS> TaskSelect(int CurId)
        {

            //ADO.NET begin--
            string sqlSel = string.Format("SELECT * FROM TASKS WHERE USID = '{0}'", CurId);
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());

            List<TASKS> Query = new List<TASKS>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new TASKS()
                {
                    TASKSTATUS = reader["TASKSTATUS"].ToString(),
                    DISCRIPTION = reader["DISCRIPTION"].ToString(),
                    TASKID = Convert.ToInt32(reader["TASKID"]),
                    TITLE = reader["TITLE"].ToString(),
                    TAGS = reader["TAGS"].ToString(),
                    TASKTERM = reader["TASKTERM"].ToString(),
                    USID = Convert.ToInt32(reader["USID"])

                });
            }
            return Query as IEnumerable<TASKS>;
            //ADO.NET end--

            #region EF Ralization
            //IEnumerable<TASKS> Query = from SelectionTasks in m_db.TASKS
            //     where SelectionTasks.USID == CurId
            //     orderby SelectionTasks.TASKID
            //     select SelectionTasks;

            // return Query;
            #endregion

        }

        /// <summary>
        /// Getting of Tags with inputed Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="TagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<TAGS> GettingTags(string TagKeyword)
        {

            //ADO.NET begin--
            string sqlSel = string.Format("SELECT * FROM TAGS WHERE TITLETAG like '%{0}%'", TagKeyword);
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());

            List<TAGS> Query = new List<TAGS>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new TAGS()
                {
                    TITLETAG = reader["TITLETAG"].ToString(),
                    ID = Convert.ToInt32(reader["ID"])
                });
            }
            return Query as IEnumerable<TAGS>;
            //ADO.NET end--

            #region EF Ralization
            //IEnumerable<TAGS> QueryTags = from t in m_db.TAGS
            //    where t.TITLETAG.Contains(TagKeyword)
            //    orderby t.ID
            //    select t;
            //return QueryTags.ToArray();
            #endregion
        }

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="SafetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        public USERS CurrentUser(string SafetyLogin)
        {
            //ADO.NET begin--
            string sqlSel = string.Format("SELECT * FROM USERS WHERE LOGIN_NAME = '{0}'", SafetyLogin);
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());

            List<USERS> Query = new List<USERS>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new USERS()
                {
                    FIRST_NAME = reader["FIRST_NAME"].ToString(),
                    LAST_NAME = reader["LAST_NAME"].ToString(),
                    EMAIL = reader["EMAIL"].ToString(),
                    LOGIN_NAME = reader["LOGIN_NAME"].ToString(),
                    PASS = reader["PASS"].ToString(),
                    CONFIRM = reader["CONFIRM"].ToString(),
                    USERID = Convert.ToInt32(reader["USERID"])
                });
            }
            return Query[0];
            //ADO.NET end--

            #region EF Ralization
            //return m_db.USERS.Where(x => x.LOGIN_NAME.Equals(SafetyLogin)).FirstOrDefault();
            #endregion
        }


        ///The others Methods///

        /// <summary>
        /// Connecting to DataBase with ADO.NET F.
        /// </summary>
        /// <returns>SqlConnection m_Connection</returns>
        public SqlConnection sqlConnection()
        {
            string conStr = @"Data Source=USADOVOY\ROOOOT;initial catalog=TaskManager;persist security info=True;user id=axon;password=sqlengine;MultipleActiveResultSets=True;";
            SqlConnection m_Connection = new SqlConnection(conStr);

            try
            {
                m_Connection.Open();
                return m_Connection;
            }
            catch (Exception)
            {
                throw;
            }
        }

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