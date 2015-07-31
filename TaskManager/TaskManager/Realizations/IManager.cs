using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager;

namespace TaskManager.Realizations
{

    public interface IManager
    {
        USERS CurrentUser(string SafetyLogin);
        string TaskAdd(TASKS model);
        Array TaskSelect(int CurId);
        void TaskDelete(int TaskId);
        string TaskChange(TASKS model);
        Array TaskOpen(int TaskId);
        //string TagsAdd(string TagRow);
        IEnumerable<TAGS> GettingTags(string TagKeyword);
        void CommonUpdateStatus();
        string TaskStatusFin(int TaskFromFinish);
    }

    
}
