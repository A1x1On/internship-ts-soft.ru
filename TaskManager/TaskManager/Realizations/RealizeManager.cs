using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace TaskManager.Realizations
{
    public class RealizeManager : IManager
    {
        /// <summary>
        /// Instance EntitieDatabase is
        /// </summary>
        private TaskManagerEF m_db = new TaskManagerEF();

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        public string TaskStatusFin(int TaskFromFinish)
        {
            //ADO.NET begin--
            try
            {
                string sqlUpdate = "UPDATE Tasks SET StatusId = 1 WHERE TASKID = @TaskFromFinish";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConnection());
                cmdUpdate.Parameters.AddWithValue("@TaskFromFinish", TaskFromFinish);
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
  
        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        public void CommonUpdateStatus(int parUSID)
        {
            //ADO.NET begin--
            using (var sqlConn = sqlConnection())
            {
                string sqlSel = "SELECT TaskTerm, TaskId FROM Tasks WHERE UsId = @parUSID AND StatusId != 1";
                SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConn);
                cmdSel.Parameters.AddWithValue("@parUSID", parUSID);
                using (SqlDataReader reader = cmdSel.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        using (var sqlConnInner = sqlConnection())
                        {
                            string sqlChange = "UPDATE Tasks SET StatusId = @StatusId " +
                                               "WHERE UsId = @parUSID AND StatusId != 1 AND TaskId = @TaskId";
                            SqlCommand cmdChange = new SqlCommand(sqlChange, sqlConnInner);
                            cmdChange.Parameters.AddWithValue("@StatusId", FromingStatus(reader["TaskTerm"].ToString()));
                            cmdChange.Parameters.AddWithValue("@parUSID", parUSID);
                            cmdChange.Parameters.AddWithValue("@TaskId", Convert.ToInt32(reader["TaskId"]));

                            cmdChange.ExecuteNonQuery();
                        }
                    }
                }
            }
            //ADO.NET end--

            #region EF Ralization
            //foreach (TASKS t in m_db.TASKS)
            //{
            //    if (t.TASKSTATUS != "Завершен" && t.USID == parUSID)
            //    {
            //        TASKS tup = new TASKS()
            //        {
            //            TASKSTATUS = FromingStatus(t.TASKTERM),
            //            USID = t.USID,
            //            TASKID = t.TASKID,
            //            DISCRIPTION = t.DISCRIPTION,
            //            TAGS = t.TAGS,
            //            TITLE = t.TITLE,
            //            TASKTERM = t.TASKTERM,
            //            USERS = t.USERS
            //        };
            //        m_db.TASKS.AddOrUpdate(tup);
            //    }
            //}
            //m_db.SaveChanges();
            #endregion
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="TaskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array TaskOpen(int TaskId)
        {
            //ADO.NET begin--
            string[] setPropertyTask = new string[6];
            string sqlSel = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());
            cmdSel.Parameters.AddWithValue("@TaskId", TaskId);

            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                setPropertyTask[0] = reader["Title"].ToString();
                setPropertyTask[1] = reader["Description"].ToString();
                setPropertyTask[2] = reader["TaskTerm"].ToString();
                setPropertyTask[3] = reader["StatusId"].ToString();
                setPropertyTask[4] = reader["Tags"].ToString();
                setPropertyTask[5] = reader["TASKID"].ToString();
            }
            return setPropertyTask;
            //ADO.NET end--

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
        public string TaskAdd(Tasks model)
        {
            //ADO.NET begin--
            Debug.WriteLine("Проверка: " + model.Title + "__" + model.Description + "__" + model.TaskTerm + "__" + model.UsId + "__" + FromingStatus(model.TaskTerm.ToString()) + "__" + TagsAdd(model.Tags));
            try
            {
                string sqlAdd = "INSERT INTO Tasks (Title, Description, TaskTerm, UsId, StatusId, Tags) VALUES (@Title, @Description, @TaskTerm, @UsId, @StatusId, @Tags)";
                SqlCommand cmdAdd = new SqlCommand(sqlAdd, sqlConnection());
                cmdAdd.Parameters.AddWithValue("@Title", model.Title);
                cmdAdd.Parameters.AddWithValue("@Description", model.Description);
                cmdAdd.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                cmdAdd.Parameters.AddWithValue("@UsId", model.UsId);
                cmdAdd.Parameters.AddWithValue("@StatusId", FromingStatus(model.TaskTerm.ToString()));
                cmdAdd.Parameters.AddWithValue("@Tags", TagsAdd(model.Tags));

                cmdAdd.ExecuteNonQuery();
                return "Задача сохранена";
            }
            catch (Exception)
            {
                return "Ошибка добавления задачи";
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
        public string TaskChange(Tasks model)
        {
            //ADO.NET begin--
            try
            {
                string sqlUpdate = "UPDATE Tasks SET Description = @Description, " +
                                   "StatusId = @StatusId, " +
                                   "Title = @Title, " +
                                   "Tags = @Tags, " +
                                   "TaskTerm = @TaskTerm, " +
                                   "UsId = @UsId WHERE TaskId = @TaskId";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConnection());
                cmdUpdate.Parameters.AddWithValue("@Description", model.Description);
                cmdUpdate.Parameters.AddWithValue("@Title", model.Title);
                cmdUpdate.Parameters.AddWithValue("@Tags", TagsAdd(model.Tags));
                cmdUpdate.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                cmdUpdate.Parameters.AddWithValue("@UsId", model.UsId);
                cmdUpdate.Parameters.AddWithValue("@StatusId", model.StatusId);
                cmdUpdate.Parameters.AddWithValue("@TaskId", model.TaskId);

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
            string sqlDel = "DELETE Tasks WHERE TaskId = @TaskId";
            SqlCommand cmdDel = new SqlCommand(sqlDel, sqlConnection());
            cmdDel.Parameters.AddWithValue("@TaskId", TaskId);

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
        /// <param name="CurId">Authorized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        public IEnumerable<Tasks> TaskSelect(int CurId)
        {
            //ADO.NET begin--
            string sqlSel = "SELECT * FROM Tasks WHERE UsId = @UsId";
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());
            cmdSel.Parameters.AddWithValue("@UsId", CurId);
            
            List<Tasks> Query = new List<Tasks>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new Tasks()
                {
                    StatusString = GetStatusString(Convert.ToInt32(reader["StatusId"])),
                    Description = reader["Description"].ToString(),
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    Title = reader["Title"].ToString(),
                    Tags = reader["Tags"].ToString(),
                    TaskTerm = Convert.ToDateTime(reader["TaskTerm"].ToString()),
                    UsId = Convert.ToInt32(reader["UsId"])

                });
            }
            return Query;
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
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="TagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<Tags> GettingTags(string TagKeyword)
        {
            //ADO.NET begin--
            string sqlSel = "SELECT * FROM Tags WHERE TitleTag like @TagKeyword";
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());
            cmdSel.Parameters.AddWithValue("@TagKeyword", "%"+TagKeyword+"%");
            List<Tags> Query = new List<Tags>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new Tags()
                {
                    TitleTag = reader["TitleTag"].ToString(),
                    Id = Convert.ToInt32(reader["Id"])
                });
            }
            return Query;
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
        public Users CurrentUser(string SafetyLogin)
        {
            //ADO.NET begin--
            string sqlSel = "SELECT * FROM Users WHERE LoginName = @LoginName";
            SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConnection());
            cmdSel.Parameters.AddWithValue("@LoginName", SafetyLogin);

            List<Users> Query = new List<Users>();
            SqlDataReader reader = cmdSel.ExecuteReader();
            while (reader.Read())
            {
                Query.Add(new Users()
                {
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    LoginName = reader["LoginName"].ToString(),
                    Pass = reader["Pass"].ToString(),
                    Confirmation = Convert.ToInt32(reader["Confirmation"]),
                    UserId = Convert.ToInt32(reader["UserId"])
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
        /// Getting string status
        /// </summary>
        /// <param name="parStatusId"></param>
        /// <returns>String status</returns>
        private string GetStatusString(int parStatusId)
        {
            return m_db.Statuses.Where(x => x.Id.Equals(parStatusId)).FirstOrDefault().TitleStatus;
        }


        /// <summary>
        /// Variable is for sqlConnection() / connection to db
        /// </summary>
        private SqlConnection m_Connection;

        /// <summary>
        /// Connecting to DataBase with ADO.NET F.
        /// </summary>
        /// <returns>SqlConnection m_Connection</returns>
        public SqlConnection sqlConnection()
        {
            System.Configuration.Configuration rootwebconfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/TaskManager");
            System.Configuration.ConnectionStringSettings constring;
            constring = rootwebconfig.ConnectionStrings.ConnectionStrings["TaskManagerADONET"];

            SqlConnection m_Connection = new SqlConnection(constring.ConnectionString);

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
        /// Amount of string
        /// </summary>
        private int m_CountTag;

        /// <summary>
        /// Adding of tags to DB or not
        /// </summary>
        /// <param name="TagRow">Got string from (text input of class="inputTag")</param>
        /// <returns>String of formed tags</returns>
        public string TagsAdd(string TagRow)
        {
            string m_FinalTag = "";
            Tags m_TagRes;
            TagRow = TagRow.ToLower();
            foreach (Match t in Regex.Matches(TagRow, @"([\b\w\-\w\b]+)"))
            {
                m_TagRes = m_db.Tags.Where(x => x.TitleTag == t.Value).FirstOrDefault();
                if (m_TagRes != null)
                {
                    m_FinalTag = m_FinalTag + ", " + t.Value;
                }
                else
                {
                    m_FinalTag = m_FinalTag + ", " + t.Value;
                    Tags TheTag = new Tags()
                    {
                        TitleTag = t.Value
                    };
                    m_db.Tags.Add(TheTag);
                    m_db.SaveChanges();
                }
            }
            m_CountTag = m_FinalTag.Length - 2;
            return m_FinalTag.Substring(2, m_CountTag);
        }

        /// <summary>
        /// Forming of Status
        /// </summary>
        /// <param name="Userdate">Own user of date from DataBase</param>
        /// <returns>Status of task</returns>
        public int FromingStatus(string Userdate)
        {
            DateTime curDate = DateTime.Now;
            string m_StrDYear = "";
            string m_StrMonth = "";
            string m_StrDay = "";
            string m_DateTask = "";
            string m_DateCurrent = "";

            m_StrDay = Userdate.Substring(0, 2);
            m_StrMonth = Userdate.Substring(4, 2);
            m_StrDYear = Userdate.Substring(6, 4);

            m_DateTask = m_StrDYear + "." + m_StrMonth + "." + m_StrDay;
            m_DateCurrent = curDate.Year + "." + curDate.Month + "." + curDate.Day;
            if (Convert.ToDateTime(m_DateTask) < Convert.ToDateTime(m_DateCurrent))
            {
                return 4; //Потрачено
            }
            if (Convert.ToDateTime(m_DateTask) == Convert.ToDateTime(m_DateCurrent))
            {
                return 3; //Сегодня последний день
            }
            return 2; //Активный
        }
    }
}