using System.Collections.Generic;

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
