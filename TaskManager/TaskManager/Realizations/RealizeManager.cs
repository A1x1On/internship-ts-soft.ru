using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TaskManager.Realizations
{
    public class RealizeManager : IManager
    {
        /// <summary>
        /// Instance EntitieDatabase is
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();

        /// <summary>
        /// SELECT @@IDENTITY from sql of request
        /// </summary>
        private int m_IdCurrentTask;

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="taskFromFinish">ID of task</param>
        public string EndActTask(int taskFromFinish)  
        {
            using (var sqlConn = ConnectToDb())
            {
                try
                {
                    string sqlUpdate = "UPDATE Tasks SET StatusId = 1 WHERE TASKID = @TaskFromFinish";
                    SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConn);
                    cmdUpdate.Parameters.AddWithValue("@TaskFromFinish", taskFromFinish);
                    cmdUpdate.ExecuteNonQuery();
                    return "Задача завершина";
                }
                catch (Exception)
                {
                    return "Задача не завершина";
                }
            }
        }
  
        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        /// <param name="usId"></param>
        public void UpdateStatusEachTask(int usId)
        {
            using (var sqlConn = ConnectToDb())
            {
                string sqlSel = "SELECT TaskTerm, TaskId FROM Tasks WHERE UsId = @UsId AND StatusId != 1";

                SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConn);
                cmdSel.Parameters.AddWithValue("@UsId", usId);

                using (SqlDataReader reader = cmdSel.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        using (var sqlConnInner = ConnectToDb())
                        {
                            string sqlChange = "UPDATE Tasks SET StatusId = @StatusId " +
                                               "WHERE UsId = @UsId AND StatusId != 1 AND TaskId = @TaskId";
                            SqlCommand cmdChange = new SqlCommand(sqlChange, sqlConnInner);

                            cmdChange.Parameters.AddWithValue("@StatusId", FormStatus(reader["TaskTerm"].ToString()));
                            cmdChange.Parameters.AddWithValue("@UsId", usId);
                            cmdChange.Parameters.AddWithValue("@TaskId", reader.GetInt32(reader.GetOrdinal("TaskId")));
                            cmdChange.ExecuteNonQuery();
                        }
                    }
                }               
            }
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="taskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array GetValuesTask(int taskId)
        {  
            var list = new List<string>();
            using (var sqlConn = ConnectToDb())
            {
                string tags = "";
                string sqlTags = "SELECT * FROM Tags AS t JOIN CrossTasksTags AS c ON (t.Id = c.TagsId) WHERE c.TaskId = @taskId";
                using (SqlCommand cmdSel = new SqlCommand(sqlTags, sqlConn))
                {
                    cmdSel.Parameters.AddWithValue("@TaskId", taskId);
                    using (SqlDataReader reader = cmdSel.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tags = tags + ", "+reader["TitleTag"].ToString();
                        }
                    }
                }
                string sqlSel = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
                using (SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConn))
                {
                    cmdSel.Parameters.AddWithValue("@TaskId", taskId);

                    using (SqlDataReader reader = cmdSel.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["Title"].ToString());
                            list.Add(reader["Description"].ToString());
                            list.Add(reader["TaskTerm"].ToString());
                            list.Add(reader["StatusId"].ToString());
                            list.Add(tags);
                            list.Add(reader["TaskId"].ToString());
                        }
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string InsertTask(Tasks model)
        {
            using (var sqlConn = ConnectToDb())
            {
                try
                {
                    string sqlAdd = "INSERT INTO Tasks (Title, Description, TaskTerm, UsId, StatusId) VALUES (@Title, @Description, @TaskTerm, @UsId, @StatusId) SELECT @@IDENTITY";
                    SqlCommand cmdAdd = new SqlCommand(sqlAdd, sqlConn);

                    cmdAdd.Parameters.AddWithValue("@Title", model.Title);
                    cmdAdd.Parameters.AddWithValue("@Description", model.Description);
                    cmdAdd.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                    cmdAdd.Parameters.AddWithValue("@UsId", model.UsId);
                    cmdAdd.Parameters.AddWithValue("@StatusId", FormStatus(model.TaskTerm.ToString()));

                    using (SqlDataReader reader = cmdAdd.ExecuteReader())
                    {
                        reader.Read();
                        m_IdCurrentTask = int.Parse(reader[0].ToString());  
                    }

                    InsertTags(model.Tags, m_IdCurrentTask, 0);
                    
                    return "Задача сохранена ";
                }
                catch (Exception)
                {
                    return "Ошибка добавления задачи";
                }
            }
        }

        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string UpdateTask(Tasks model)
        {
            using (var sqlConn = ConnectToDb())
            {
                try
                {
                    string sqlUpdate = "UPDATE Tasks SET Description = @Description, " +
                                       "StatusId = @StatusId, " +
                                       "Title = @Title, " +
                                       "TaskTerm = @TaskTerm, " +
                                       "UsId = @UsId WHERE TaskId = @TaskId";
                    SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConn);

                    cmdUpdate.Parameters.AddWithValue("@Description", model.Description);
                    cmdUpdate.Parameters.AddWithValue("@Title", model.Title);
                    cmdUpdate.Parameters.AddWithValue("@TaskTerm", model.TaskTerm);
                    cmdUpdate.Parameters.AddWithValue("@UsId", model.UsId);
                    cmdUpdate.Parameters.AddWithValue("@StatusId", model.StatusId);
                    cmdUpdate.Parameters.AddWithValue("@TaskId", model.TaskId);
                    cmdUpdate.ExecuteNonQuery();

                    RemoveTags(model.Tags, model.TaskId);
                    InsertTags(model.Tags, model.TaskId, 1);

                    return "Задача изменена";
                }
                catch (Exception)
                {
                    return "Задача не изменена";
                }
            }
        }

        /// <summary>
        /// Removing tag from CrossTasksTags(Association)
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="taskId"></param>
        public void RemoveTags(string tags, int taskId)
        {
            var listTagId = m_db.CrossTasksTags.Where(x => x.TaskId == taskId).Select(x => new { x.TagsId }).ToList();
            int resId;
            CrossTasksTags forDel;
            tags = tags.ToLower();
            foreach (Match t in Regex.Matches(tags, @"([\b\w\-\w\b]+)"))
            {
                resId = m_db.Tags.Where(x => x.TitleTag == t.Value).FirstOrDefault().Id;
                listTagId.Remove(listTagId.Find(x => x.TagsId.Equals(resId)));
            }
            foreach (var i in listTagId)
            {
                forDel = m_db.CrossTasksTags.FirstOrDefault(d => d.TagsId == i.TagsId);
                m_db.Set<CrossTasksTags>().Remove(forDel);
            }
            m_db.SaveChanges();
        }
        
        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="taskId">Id of task</param>
        public bool DeleteTask(int taskId)              
        {
            try
            {
                using (var sqlCross = ConnectToDb())
                {
                    string sqlDel = "DELETE CrossTasksTags WHERE TaskId = @TaskId";
                    using (SqlCommand cmdCrossDel = new SqlCommand(sqlDel, sqlCross))
                    {
                        cmdCrossDel.Parameters.AddWithValue("@TaskId", taskId);
                        cmdCrossDel.ExecuteNonQuery();
                    }
                }
                using (var sqlConn = ConnectToDb())
                {
                    string sqlDel = "DELETE Tasks WHERE TaskId = @TaskId";
                    using (SqlCommand cmdTaskDel = new SqlCommand(sqlDel, sqlConn))
                    {
                        cmdTaskDel.Parameters.AddWithValue("@TaskId", taskId);
                        cmdTaskDel.ExecuteNonQuery(); 
                    }   
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Getting of tasks
        /// </summary>
        /// <param name="curId">Authorized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        public IEnumerable<Tasks> GetTasks(int curId, int tagId, string date)
        {
            using (var sqlConn = ConnectToDb())
            {
                using (SqlCommand cmdSel = new SqlCommand())
                {
                    StringBuilder sbWhere = new StringBuilder();
                    StringBuilder sbTagAndOther = new StringBuilder();
                    sbWhere.Append("SELECT * FROM Tasks WHERE UsId = @UsId");
                    sbTagAndOther.Append("SELECT * FROM Tasks AS t");

                    if (tagId != 0)
                    {
                        sbTagAndOther.Append(" JOIN CrossTasksTags AS c ON (t.TaskId = c.TaskId) WHERE t.UsId = @UsId and c.TagsId = @TagsId");
                        sbWhere = sbTagAndOther;
                        cmdSel.Parameters.AddWithValue("@TagsId", tagId);
                    }
                    if (date != "")
                    {
                        sbTagAndOther.Append(" WHERE t.UsId = @UsId and TaskTerm = @TaskTerm");
                        sbWhere = sbTagAndOther;
                        cmdSel.Parameters.Add("@TaskTerm", SqlDbType.Date).Value = date;
                    }

                    cmdSel.Parameters.AddWithValue("@UsId", curId);
                    cmdSel.Connection = sqlConn;
                    cmdSel.CommandText = sbWhere.ToString();

                    List<Tasks> query = new List<Tasks>();
                    using (SqlDataReader reader = cmdSel.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            query.Add(new Tasks()
                            {
                                StatusString = GetStatusString(reader.GetInt32(5)),
                                Description = reader["Description"].ToString(),
                                TaskId = reader.GetInt32(0),
                                Title = reader["Title"].ToString(),
                                TaskTerm = reader.GetDateTime(2),
                                UsId = reader.GetInt32(4)

                            });
                        }
                    }
                    return query;
                }
            }
        }

        /// <summary>
        /// Getting of tags
        /// </summary>
        /// <param name="CurId"></param>
        /// <returns></returns>
        public IEnumerable<Tags> GetTags(int CurId)
        {
            using (var sqlConn = ConnectToDb())
            {
                List<Tags> query = new List<Tags>();
                List<int> listTagsId;

                string sqlCrossTags = "SELECT DISTINCT TagsId FROM CrossTasksTags AS c JOIN Tasks AS t ON (c.TaskId = t.TaskId) WHERE t.UsId = @UsId";
                using (SqlCommand cmdCrossTags = new SqlCommand(sqlCrossTags, sqlConn))
                {
                    cmdCrossTags.Parameters.AddWithValue("@UsId", CurId);
                    using (SqlDataReader readerCrossTags = cmdCrossTags.ExecuteReader())
                    {
                        listTagsId = (from IDataRecord r in readerCrossTags select (int) r["TagsId"]).ToList();
                        readerCrossTags.Close();
                    }
                }
                string sqlSelTags = "SELECT * FROM Tags WHERE Id = @Id";
                using (SqlCommand cmdTags = new SqlCommand(sqlSelTags, sqlConn))
                {
                    cmdTags.Parameters.AddWithValue("@Id", 0);
                    foreach (var id in listTagsId)
                    {
                        cmdTags.Parameters["@id"].Value = id;
                        using (SqlDataReader readerTags = cmdTags.ExecuteReader())
                        {       
                            if (readerTags.Read())
                            {
                                query.Add(new Tags()
                                {
                                    Id = readerTags.GetInt32(0),
                                    TitleTag = readerTags["TitleTag"].ToString()
                                });
                            }
                        }
                    }    
                }
                
                return query;
            }
        }
  

        /// <summary>
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="tagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<Tags> GetTags(string tagKeyword)  
        {
            using (var sqlConn = ConnectToDb())
            {
                string sqlSel = "SELECT * FROM Tags WHERE TitleTag like @TagKeyword";
                SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConn);
                cmdSel.Parameters.AddWithValue("@TagKeyword", "%" + tagKeyword + "%");
                List<Tags> query = new List<Tags>();

                using (SqlDataReader reader = cmdSel.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        query.Add(new Tags()
                        {
                            TitleTag = reader["TitleTag"].ToString(),
                            Id = reader.GetInt32(0)
                        });
                    }
                    return query;
                }
            }
        }

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="safetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        public Users GetCurrentUser(string safetyLogin)
        {
            using (var sqlConn = ConnectToDb())
            {
                string sqlSel = "SELECT * FROM Users WHERE LoginName = @LoginName";
                SqlCommand cmdSel = new SqlCommand(sqlSel, sqlConn);
                cmdSel.Parameters.AddWithValue("@LoginName", safetyLogin);
                using (SqlDataReader reader = cmdSel.ExecuteReader())
                {
                    reader.Read();
                    return new Users()
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        LoginName = reader["LoginName"].ToString(),
                        Pass = reader["Pass"].ToString(),
                        Confirmation = reader.GetInt32(6),
                        UserId = reader.GetInt32(0)
                    };
                    
                }
            }
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
        public SqlConnection ConnectToDb()
        {
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/TaskManager");
            System.Configuration.ConnectionStringSettings constring;
            constring = rootWebConfig.ConnectionStrings.ConnectionStrings["TaskManagerADONET"];
            SqlConnection m_Connection = new SqlConnection(constring.ConnectionString);
            m_Connection.Open();
            return m_Connection;   
        }

        /// <summary>
        /// Amount of string
        /// </summary>
        private int m_CountTag;

        /// <summary>
        /// Adding of tags to DB or not
        /// </summary>
        /// <param name="tagRow">Got string from (text input of class="inputTag")</param>
        public void InsertTags(string tagRow, int taskId, int way)
        {
            Tags tagRes;
            CrossTasksTags cross = null;
            int TagsId;
          
            tagRow = tagRow.ToLower();
            foreach (Match t in Regex.Matches(tagRow, @"([\b\w\-\w\b]+)"))
            {
                tagRes = m_db.Tags.Where(x => x.TitleTag == t.Value).FirstOrDefault(); // search next tag of Match list

                if (way == 0) // Condition for insert of task
                {
                    if (tagRes == null)
                    {
                        Tags theTag = new Tags() { TitleTag = t.Value };
                        m_db.Tags.Add(theTag);
                        m_db.SaveChanges();

                        TagsId = theTag.Id;

                        cross = new CrossTasksTags() { TaskId = taskId, TagsId = TagsId };
                    }
                    else
                    {
                        cross = new CrossTasksTags() { TaskId = taskId, TagsId = tagRes.Id };
                    }
                    m_db.CrossTasksTags.Add(cross);
                }
                else // Condition for update of task
                {
                    bool addAndAssociate = false; 
                    bool associate = true;

                    if (tagRes != null) // Tag found in table tags
                    {
                        if (m_db.CrossTasksTags
                            .Where(x => x.TagsId == tagRes.Id && x.TaskId == taskId)
                            .FirstOrDefault() != null)
                        {
                            associate = false;
                        }
                    }
                    else
                    {
                        addAndAssociate = true;
                        associate = false;
                    }


                    if (addAndAssociate) // add and associate tag 
                    {
                        Tags theTag = new Tags() {TitleTag = t.Value};
                        m_db.Tags.Add(theTag);
                        m_db.SaveChanges();

                        TagsId = theTag.Id;
                        cross = new CrossTasksTags() { TaskId = taskId, TagsId = TagsId };
                        m_db.CrossTasksTags.Add(cross);
                    }
                    else if (associate) // just associate tag 
                    {
                        cross = new CrossTasksTags() { TaskId = taskId, TagsId = tagRes.Id };
                        m_db.CrossTasksTags.Add(cross);
                    }
                }

                m_db.SaveChanges(); 
            }
        }

        /// <summary>
        /// Forming of Status
        /// </summary>
        /// <param name="userDate">Own user of date from DataBase</param>
        /// <returns>Status of task</returns>
        public int FormStatus(string userDate)      
        {
            DateTime curDate = DateTime.Now;
            string strDYear = "";
            string strMonth = "";
            string strDay = "";
            string dateTask = "";
            string dateCurrent = "";

            strDay = userDate.Substring(0, 2);
            strMonth = userDate.Substring(4, 2);
            strDYear = userDate.Substring(6, 4);

            dateTask = strDYear + "." + strMonth + "." + strDay;
            dateCurrent = curDate.Year + "." + curDate.Month + "." + curDate.Day;
            if (Convert.ToDateTime(dateTask) < Convert.ToDateTime(dateCurrent))
            {
                return 4; //Потрачено
            }
            if (Convert.ToDateTime(dateTask) == Convert.ToDateTime(dateCurrent))
            {
                return 3; //Сегодня последний день
            }
            return 2; //Активный
        }
    }
}