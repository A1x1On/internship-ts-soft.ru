﻿using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Realizations
{
    public interface IManager
    {
        /// <summary>
        /// Turn off activity of task(status)
        /// </summary>
        /// <param name="taskId">ID of task</param>
        string EndActTask(int taskId);

        /// <summary>
        /// Getting of tags for each task on the page of list Tasks
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        string GetTagsOfTask(int taskId);

        // Turn on activity of task(status)
        string BeginActTask(int taskId);

        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        void UpdateStatusEachTask(int usId);

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="taskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        Array GetValuesTask(int taskId);

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string InsertTask(Tasks model);

        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string UpdateTask(Tasks model);

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="taskId">Id of task</param>
        bool DeleteTask(int taskId);     

        /// <summary>
        /// Getting of tasks' list
        /// </summary>
        /// <param name="curId">Authorized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        IEnumerable<Tasks> GetTasks(int curId, int tagId, DateTime dateTime);

        /// <summary>
        /// Getting of tags
        /// </summary>
        /// <param name="curId"></param>
        /// <returns></returns>
        IEnumerable<Tags> GetTags(int curId);

        /// <summary>
        /// Getting of dates
        /// </summary>
        /// <param name="curId"></param>
        /// <returns></returns>
        IEnumerable<DateTasks> GetDates(int curId);

        /// <summary>
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="tagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        IEnumerable<Tags> GetTags(string tagKeyword);   

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="safetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        Users GetCurrentUser(string safetyLogin);
    } 
}
