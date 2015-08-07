using System;
using System.Collections.Generic;

namespace TaskManager.Realizations
{
    public interface IManager
    {
        /// <summary>
        /// Finishing of task
        /// </summary>
        /// <param name="TaskFromFinish">ID of task</param>
        string TaskStatusFin(int TaskFromFinish);

        /// <summary>
        /// Auto-Updating all user's tasks that is updating statuses of the evrey task
        /// </summary>
        void CommonUpdateStatus(int parUSID);

        /// <summary>
        /// Opening of task on page or the other words review of task's Detail info 
        /// </summary>
        /// <param name="TaskId">ID of task</param>
        /// <returns>Set of property for Angular act</returns>
        Array TaskOpen(int TaskId);

        /// <summary>
        /// Adding a task into the data base
        /// </summary>
        /// <param name="model">Object of task who ready to add database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string TaskAdd(TASKS model);

        /// <summary>
        /// Changing of task
        /// </summary>
        /// <param name="model">Object of task who ready to change from database</param>
        /// <returns>Result to m_ResultMassage in controller</returns>
        string TaskChange(TASKS model);

        /// <summary>
        /// Removing of task
        /// </summary>
        /// <param name="TaskId">Id of task</param>
        void TaskDelete(int TaskId);

        /// <summary>
        /// Getting of tasks' list
        /// </summary>
        /// <param name="CurId">Authoeized user</param>
        /// <returns>List of tasks for _PartialSelectionTasks.cshtml</returns>
        IEnumerable<TASKS> TaskSelect(int CurId);

        /// <summary>
        /// Getting of Tags with inputed Keyword and angular service if such tag exists in DB
        /// </summary>
        /// <param name="TagKeyword">Key word from (text input of class="inputTag")</param>
        /// <returns>list of received tags</returns>
        IEnumerable<TAGS> GettingTags(string TagKeyword);

        /// <summary>
        /// Getting of User id
        /// </summary>
        /// <param name="SafetyLogin">WebSecurity.CurrentUserName</param>
        /// <returns>Object USER</returns>
        USERS CurrentUser(string SafetyLogin);
    } 
}
