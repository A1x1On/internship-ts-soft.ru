using System;
using System.Collections.Generic;

namespace TaskManager.Realizations
{
    public interface IManager
    {
        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="taskFromFinish">ID of task</param>
        string TaskStatusFin(int taskFromFinish);

        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the every task
        /// </summary>
        void CommonUpdateStatus(int usId);

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="taskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        Array TaskOpen(int taskId);

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string TaskAdd(Tasks model);

        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string TaskChange(Tasks model);

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="taskId">Id of task</param>
        void TaskDelete(int taskId);

        /// <summary>
        /// Getting of tasks' list
        /// </summary>
        /// <param name="curId">Authorized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        IEnumerable<Tasks> TaskSelect(int curId);

        /// <summary>
        /// Getting of Tags with inputted Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="tagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        IEnumerable<Tags> GettingTags(string tagKeyword);

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="safetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        Users CurrentUser(string safetyLogin);
    } 
}
