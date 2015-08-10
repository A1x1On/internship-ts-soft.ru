using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        /// <param name="taskFromFinish">ID of task</param>
        public string TaskStatusFin(int taskFromFinish)
        {
            //ADO.NET begin--
            try
            {
                string SqlUpdate = "UPDATE Tasks SET StatusId = 1 WHERE TASKID = @TaskFromFinish";
                SqlCommand CmdUpdate = new SqlCommand(SqlUpdate, sqlConnection());
                CmdUpdate.Parameters.AddWithValue("@TaskFromFinish", taskFromFinish);
                CmdUpdate.ExecuteNonQuery();
                return "Задача завершина";
            }
            catch (Exception)
            {
                return "Задача не завершина";
            }
            //ADO.NET end--
        }
  
        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        /// <param name="usId"></param>
        public void CommonUpdateStatus(int usId)
        {
            //ADO.NET begin--
            using (var SqlConn = sqlConnection())
            {
                string SqlSel = "SELECT TaskTerm, TaskId FROM Tasks WHERE UsId = @UsId AND StatusId != 1";
                SqlCommand CmdSel = new SqlCommand(SqlSel, SqlConn);
                CmdSel.Parameters.AddWithValue("@UsId", usId);

                using (SqlDataReader Reader = CmdSel.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        using (var SqlConnInner = sqlConnection())
                        {
                            string SqlChange = "UPDATE Tasks SET StatusId = @StatusId " +
                                               "WHERE UsId = @parUSID AND StatusId != 1 AND TaskId = @TaskId";
                            SqlCommand CmdChange = new SqlCommand(SqlChange, SqlConnInner);

                            CmdChange.Parameters.AddWithValue("@StatusId", FromingStatus(Reader["TaskTerm"].ToString()));
                            CmdChange.Parameters.AddWithValue("@parUSID", usId);
                            CmdChange.Parameters.AddWithValue("@TaskId", Convert.ToInt32(Reader["TaskId"]));
                            CmdChange.ExecuteNonQuery();
                        }
                    }
                }
            }
            //ADO.NET end--
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="taskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array TaskOpen(int taskId)
        {
            //ADO.NET begin--
            string[] SetPropertyTask = new string[6];
            string SqlSel = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
            SqlCommand CmdSel = new SqlCommand(SqlSel, sqlConnection());
            CmdSel.Parameters.AddWithValue("@TaskId", taskId);
            SqlDataReader Reader = CmdSel.ExecuteReader();

            while (Reader.Read())
            {
                SetPropertyTask[0] = Reader["Title"].ToString();
                SetPropertyTask[1] = Reader["Description"].ToString();
                SetPropertyTask[2] = Reader["TaskTerm"].ToString();
                SetPropertyTask[3] = Reader["StatusId"].ToString();
                SetPropertyTask[4] = Reader["Tags"].ToString();
                SetPropertyTask[5] = Reader["TASKID"].ToString();
            }
            return SetPropertyTask;
            //ADO.NET end--
        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string TaskAdd(Tasks model)
        {
            //ADO.NET begin--
            try
            {
                string SqlAdd = "INSERT INTO Tasks (Title, Description, TaskTerm, UsId, StatusId, Tags) VALUES (@Title, @Description, @TaskTerm, @UsId, @StatusId, @Tags)";
                SqlCommand CmdAdd = new SqlCommand(SqlAdd, sqlConnection());

                CmdAdd.Parameters.AddWithValue("@Title", model.Title);
                CmdAdd.Parameters.AddWithValue("@Description", model.Description);
                CmdAdd.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                CmdAdd.Parameters.AddWithValue("@UsId", model.UsId);
                CmdAdd.Parameters.AddWithValue("@StatusId", FromingStatus(model.TaskTerm.ToString()));
                CmdAdd.Parameters.AddWithValue("@Tags", TagsAdd(model.Tags));
                CmdAdd.ExecuteNonQuery();
                return "Задача сохранена";
            }
            catch (Exception)
            {
                return "Ошибка добавления задачи";
            }
            //ADO.NET end--
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
                string SqlUpdate = "UPDATE Tasks SET Description = @Description, " +
                                   "StatusId = @StatusId, " +
                                   "Title = @Title, " +
                                   "Tags = @Tags, " +
                                   "TaskTerm = @TaskTerm, " +
                                   "UsId = @UsId WHERE TaskId = @TaskId";
                SqlCommand CmdUpdate = new SqlCommand(SqlUpdate, sqlConnection());

                CmdUpdate.Parameters.AddWithValue("@Description", model.Description);
                CmdUpdate.Parameters.AddWithValue("@Title", model.Title);
                CmdUpdate.Parameters.AddWithValue("@Tags", TagsAdd(model.Tags));
                CmdUpdate.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                CmdUpdate.Parameters.AddWithValue("@UsId", model.UsId);
                CmdUpdate.Parameters.AddWithValue("@StatusId", model.StatusId);
                CmdUpdate.Parameters.AddWithValue("@TaskId", model.TaskId);
                CmdUpdate.ExecuteNonQuery();
                return "Задача изменена";
            }
            catch (Exception)
            {
                return "Задача не изменена";
            }
            //ADO.NET end--
        }

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="taskId">Id of task</param>
        public void TaskDelete(int taskId)
        {        
            //ADO.NET begin--
            string SqlDel = "DELETE Tasks WHERE TaskId = @TaskId";
            SqlCommand CmdDel = new SqlCommand(SqlDel, sqlConnection());
            CmdDel.Parameters.AddWithValue("@TaskId", taskId);
            CmdDel.ExecuteNonQuery();
            //ADO.NET end--
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
            SqlDataReader Reader = cmdSel.ExecuteReader();

            while (Reader.Read())
            {
                Query.Add(new Tasks()
                {
                    StatusString = GetStatusString(Convert.ToInt32(Reader["StatusId"])),
                    Description = Reader["Description"].ToString(),
                    TaskId = Convert.ToInt32(Reader["TaskId"]),
                    Title = Reader["Title"].ToString(),
                    Tags = Reader["Tags"].ToString(),
                    TaskTerm = Convert.ToDateTime(Reader["TaskTerm"].ToString()),
                    UsId = Convert.ToInt32(Reader["UsId"])

                });
            }
            return Query;
            //ADO.NET end--

        }

        /// <summary>
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="tagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<Tags> GettingTags(string tagKeyword)
        {
            //ADO.NET begin--
            string SqlSel = "SELECT * FROM Tags WHERE TitleTag like @TagKeyword";
            SqlCommand CmdSel = new SqlCommand(SqlSel, sqlConnection());
            CmdSel.Parameters.AddWithValue("@TagKeyword", "%" + tagKeyword + "%");
            List<Tags> Query = new List<Tags>();
            SqlDataReader Reader = CmdSel.ExecuteReader();

            while (Reader.Read())
            {
                Query.Add(new Tags()
                {
                    TitleTag = Reader["TitleTag"].ToString(),
                    Id = Convert.ToInt32(Reader["Id"])
                });
            }
            return Query;
            //ADO.NET end--
       }

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="safetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        public Users CurrentUser(string safetyLogin)
        {
            //ADO.NET begin--
            string SqlSel = "SELECT * FROM Users WHERE LoginName = @LoginName";
            SqlCommand CmdSel = new SqlCommand(SqlSel, sqlConnection());
            CmdSel.Parameters.AddWithValue("@LoginName", safetyLogin);
            List<Users> Query = new List<Users>();
            
            SqlDataReader Reader = CmdSel.ExecuteReader();
            while (Reader.Read())
            {
                Query.Add(new Users()
                {
                    FirstName = Reader["FirstName"].ToString(),
                    LastName = Reader["LastName"].ToString(),
                    Email = Reader["Email"].ToString(),
                    LoginName = Reader["LoginName"].ToString(),
                    Pass = Reader["Pass"].ToString(),
                    Confirmation = Convert.ToInt32(Reader["Confirmation"]),
                    UserId = Convert.ToInt32(Reader["UserId"])
                });
            }
            return Query[0];
            //ADO.NET end--
        }


        ///The others Methods///

        /// <summary>
        /// Getting string status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>String status</returns>
        private string GetStatusString(int statusId)
        {
            return m_db.Statuses.Where(x => x.Id.Equals(statusId)).FirstOrDefault().TitleStatus;
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
            System.Configuration.Configuration Rootwebconfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/TaskManager");
            System.Configuration.ConnectionStringSettings Constring;
            Constring = Rootwebconfig.ConnectionStrings.ConnectionStrings["TaskManagerADONET"];

            SqlConnection m_Connection = new SqlConnection(Constring.ConnectionString);
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
        /// <param name="tagRow">Got string from (text input of class="inputTag")</param>
        /// <returns>String of formed tags</returns>
        public string TagsAdd(string tagRow)
        {
            string FinalTag = "";
            Tags TagRes;
            tagRow = tagRow.ToLower();
            foreach (Match t in Regex.Matches(tagRow, @"([\b\w\-\w\b]+)"))
            {
                TagRes = m_db.Tags.Where(x => x.TitleTag == t.Value).FirstOrDefault();
                if (TagRes != null)
                {
                    FinalTag = FinalTag + ", " + t.Value;
                }
                else
                {
                    FinalTag = FinalTag + ", " + t.Value;
                    Tags TheTag = new Tags()
                    {
                        TitleTag = t.Value
                    };
                    m_db.Tags.Add(TheTag);
                    m_db.SaveChanges();
                }
            }
            m_CountTag = FinalTag.Length - 2;
            return FinalTag.Substring(2, m_CountTag);
        }

        /// <summary>
        /// Forming of Status
        /// </summary>
        /// <param name="userDate">Own user of date from DataBase</param>
        /// <returns>Status of task</returns>
        public int FromingStatus(string userDate)
        {
            DateTime CurDate = DateTime.Now;
            string StrDYear = "";
            string StrMonth = "";
            string StrDay = "";
            string DateTask = "";
            string DateCurrent = "";

            StrDay = userDate.Substring(0, 2);
            StrMonth = userDate.Substring(4, 2);
            StrDYear = userDate.Substring(6, 4);

            DateTask = StrDYear + "." + StrMonth + "." + StrDay;
            DateCurrent = CurDate.Year + "." + CurDate.Month + "." + CurDate.Day;
            if (Convert.ToDateTime(DateTask) < Convert.ToDateTime(DateCurrent))
            {
                return 4; //Потрачено
            }
            if (Convert.ToDateTime(DateTask) == Convert.ToDateTime(DateCurrent))
            {
                return 3; //Сегодня последний день
            }
            return 2; //Активный
        }
    }
}