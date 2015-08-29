using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using TaskManager.Models;

namespace TaskManager.Realizations
{
    public class RealizeManager : IManager
    {
        /// <summary>
        /// Instance EntitieDatabase is
        /// </summary>
        private TManagerEntities m_db = new TManagerEntities();

        /// <summary>
        /// SELECT @@IDENTITY from sql of request
        /// </summary>
        private int m_IdCurrentTask;

        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="tasId">ID of task</param>
        public string EndActTask(int taskId)
        {
            try
            {
                var task = m_db.Tasks.FirstOrDefault(x => x.TaskId.Equals(taskId));
                task.StatusId = 1;
                m_db.Tasks.AddOrUpdate(task);
                m_db.SaveChanges();

                return "Задача завершина";
            }
            catch (Exception)
            {
                return "Задача не завершина";
            }
        }

        /// <summary>
        /// Cancel inactive status
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string BeginActTask(int taskId)
        {
            try
            {
                var task = m_db.Tasks.FirstOrDefault(x => x.TaskId == taskId);
                task.StatusId = 2;
                m_db.Tasks.AddOrUpdate(task);
                m_db.SaveChanges();
                return "Задача активна";
            }
            catch (Exception)
            {
                return "Ошибка изменения статуса на 'Активна'";
            }
        }

        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        /// <param name="usId"></param>
        public void UpdateStatusEachTask(int usId)
        {
            foreach (Tasks t in m_db.Tasks)
            {
                if (t.StatusId != 1 && t.UsId == usId)
                {
                    Tasks task = new Tasks()
                    {
                        StatusId = FormStatus(t.TaskTerm.ToString()),
                        UsId = t.UsId,
                        TaskId = t.TaskId,
                        Description = t.Description,
                        Tags = t.Tags,
                        Title = t.Title,
                        TaskTerm = t.TaskTerm,
                        Users = t.Users
                    };
                    m_db.Tasks.AddOrUpdate(task);
                }
            }
            m_db.SaveChanges();
        }

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info
        /// </summary>
        /// <param name="taskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        public Array GetValuesTask(int taskId)
        {
            var list = new List<string>();
            string tags = "";
            var tagsCross = m_db.Tags.Join(
                m_db.CrossTasksTags,
                t => t.Id,
                c => c.TagsId,
                (t, c) => new { Tags = t, CrossTasksTags = c })
                .Where(x => x.CrossTasksTags.TaskId == taskId);

            foreach (var q in tagsCross)
            {
                tags = tags + ", " + q.Tags.TitleTag;
            }

            var task = m_db.Tasks.FirstOrDefault(x => x.TaskId == taskId);
            list.Add(task.Title);
            list.Add(task.Description);
            list.Add(task.TaskTerm.ToString());
            list.Add(task.StatusId.ToString());
            list.Add(tags);
            list.Add(taskId.ToString());
            return list.ToArray();
        }

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string InsertTask(Tasks model)
        {
            try
            {
                Tasks task = new Tasks()
                {
                    Title = model.Title,
                    Description = model.Description,
                    TaskTerm = model.TaskTerm,
                    UsId = model.UsId,
                    StatusId = FormStatus(model.TaskTerm.ToString()),
                };
                m_db.Tasks.Add(task);
                m_db.SaveChanges();
                InsertTags(model.Tags, task.TaskId, 0);
                return "Задача сохранена";
            }
            catch (Exception)
            {
                return "Ошибка добавления задачи";
            }
        }

        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        public string UpdateTask(Tasks model)
        {
            try
            {
                var value = m_db.Tasks.FirstOrDefault(c => c.TaskId == model.TaskId);
                if (value != null)
                {
                    Tasks task = new Tasks()
                    {
                        TaskId = model.TaskId,
                        Title = model.Title,
                        Description = model.Description,
                        TaskTerm = model.TaskTerm,
                        UsId = model.UsId,
                        StatusId = model.StatusId,
                    };
                    m_db.Tasks.AddOrUpdate(task);
                    m_db.SaveChanges();

                    RemoveTags(model.Tags, model.TaskId);
                    InsertTags(model.Tags, model.TaskId, 1);
                }
                return "Задача изменена";
            }
            catch (Exception)
            {
                return "Задача не изменена";
            }
        }

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="taskId">Id of task</param>
        public bool DeleteTask(int taskId)
        {
            try
            {
                var crossTasks = m_db.CrossTasksTags.Where(c => c.TaskId == taskId);
                foreach (var cross in crossTasks)
                {
                    m_db.Set<CrossTasksTags>().Remove(cross);
                }
                m_db.SaveChanges();

                var task = m_db.Tasks.FirstOrDefault(c => c.TaskId == taskId);
                m_db.Set<Tasks>().Remove(task);
                m_db.SaveChanges();

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
        /// <returns>List of tasks</returns>
        public IEnumerable<Tasks> GetTasks(int curUserId, int tagId, DateTime dateTime)
        {
            List<Tasks> tasksAsList = new List<Tasks>();
            IQueryable<Tasks> query = null;

            // Without filter
            query = from tasks in m_db.Tasks
                    where tasks.UsId == curUserId
                    select tasks;

            // By tag
            if (tagId != 0)
            {
                query = m_db.Tasks.Join(
                    m_db.CrossTasksTags,
                    t => t.TaskId,
                    c => c.TaskId,
                    (t, c) => new { Tasks = t, CrossTasksTags = c })
                    .Where(x => x.CrossTasksTags.TagsId == tagId && x.Tasks.UsId == curUserId)
                    .Select(p => p.Tasks);
            }

            // By date
            if (dateTime != default(DateTime))
            {
                query = m_db.Tasks.Join(
                    m_db.CrossTasksTags,
                    t => t.TaskId,
                    c => c.TaskId,
                    (t, c) => new { Tasks = t, CrossTasksTags = c })
                    .Where(x => x.Tasks.TaskTerm == dateTime && x.Tasks.UsId == curUserId)
                    .Select(p => p.Tasks);
            }

            foreach (var q in query)
            {
                tasksAsList.Add(new Tasks()
                {
                    StatusString = GetStatusString(q.StatusId.Value),
                    Description = q.Description,
                    TaskId = q.TaskId,
                    Title = q.Title,
                    TaskTerm = q.TaskTerm,
                    UsId = q.UsId

                });
            }
            return tasksAsList.Count != 0 ? tasksAsList : query.ToList();
        }

        /// <summary>
        /// Getting of tags
        /// </summary>
        /// <param name="CurId"></param>
        /// <returns></returns>
        public IEnumerable<Tags> GetTags(int curId)
        {
            List<Tags> tagsAsList = new List<Tags>();
            Tags tag;

            var query = from tasks in m_db.Tasks
                        join cross in m_db.CrossTasksTags
                        on tasks.TaskId equals cross.TaskId
                        where tasks.UsId == curId
                        select new { tasks, cross };

            foreach (var c in query.DistinctBy(x=>x.cross.TagsId))
            {
                tag = m_db.Tags.Where(x => x.Id == c.cross.TagsId).First();
                tagsAsList.Add(new Tags()
                {
                    Id = tag.Id,
                    TitleTag = tag.TitleTag
                });
            }

            return tagsAsList;
        }

        /// <summary>
        /// Getting of dates
        /// </summary>
        /// <param name="CurId"></param>
        /// <returns></returns>
        public IEnumerable<DateTasks> GetDates(int curId)
        {
            List<DateTasks> datesAsList = new List<DateTasks>();
            int i = 0;

            var query = from tasks in m_db.Tasks.Distinct()
                        where tasks.UsId == curId
                        select tasks;
            foreach (var q in query.DistinctBy(x => x.TaskTerm))
            {
                i++;
                datesAsList.Add(new DateTasks(){ Id = i, Date = q.TaskTerm });
            }
            return datesAsList;
        }

        /// <summary>
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="tagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        public IEnumerable<Tags> GetTags(string tagKeyword)
        {
            List<Tags> query = new List<Tags>();
            var tags = m_db.Tags.Where(x => x.TitleTag.Contains(tagKeyword));
            foreach (var t in tags)
            {
                query.Add(new Tags()
                {
                    TitleTag = t.TitleTag,
                    Id = t.Id
                });
            }
            return query;
        }

        /// <summary>
        /// Getting of tags for each task on the page of list Tasks
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string GetTagsOfTask(int taskId)
        {
            string tags = "";
            var query = m_db.Tags.Join(
                m_db.CrossTasksTags,
                t => t.Id,
                c => c.TagsId,
                (t, c) => new {Tags = t, CrossTasksTags = c})
                .Where(x => x.CrossTasksTags.TaskId == taskId)
                .Select(p => p.Tags);

            foreach (var q in query)
            {
                tags = tags + ", " + q.TitleTag;
            }
            return tags.Substring(1);
        }

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="safetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        public Users GetCurrentUser(string safetyLogin)
        {
            var user = m_db.Users.FirstOrDefault(x => x.LoginName == safetyLogin);
            return new Users()
            {
                FirstName = user.LoginName,
                LastName = user.LastName,
                Email = user.Email,
                LoginName = user.LoginName,
                Pass = user.Pass,
                Confirmation = user.Confirmation,
                UserId = user.UserId
            };
        }


        ///The others Methods///

        /// <summary>
        /// Getting string status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>String status</returns>
        private string GetStatusString(int statusId)
        {
            var state = m_db.Statuses.FirstOrDefault(x => x.Id.Equals(statusId));
            return state.TitleStatus;
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
                tagRes = m_db.Tags.FirstOrDefault(x => x.TitleTag == t.Value); // search next tag of Match list
                if (way == 0) // Condition for insert of tag
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
                        if (m_db.CrossTasksTags.FirstOrDefault(x => x.TagsId == tagRes.Id && x.TaskId == taskId) != null)
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
                        Tags theTag = new Tags() { TitleTag = t.Value };
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
            foreach (Match t in Regex.Matches(tags, @"([ \b\w\- \w\b] +)"))
            {
                resId = m_db.Tags.FirstOrDefault(x => x.TitleTag == t.Value).Id;
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
