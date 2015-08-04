using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Models
{
    public class TasksAddChangeSelect
    {

        public TASKS AddChange { get; set; }
        public  IEnumerable<TASKS> SelecTasks { get; set; }
        public string CurStatus { get; set; }
        public string CurLogin { get; set; }
        public int CurId { get; set; }



    }
}
