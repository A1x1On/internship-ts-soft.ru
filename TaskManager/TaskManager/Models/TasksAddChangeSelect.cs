using System.Collections.Generic;

namespace TaskManager.Models
{
    public class TasksAddChangeSelect
    {
        public Tasks AddChange { get; set; }
        public IEnumerable<Tags> SelectTags { get; set; }
        public IEnumerable<Tasks> SelectTasks { get; set; }
        public string CurStatus { get; set; }
        public string CurLogin { get; set; }
        public int CurId { get; set; }
    }
}
