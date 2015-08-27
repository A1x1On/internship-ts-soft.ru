//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tasks
    {
        public Tasks()
        {
            this.CrossTasksTags = new HashSet<CrossTasksTags>();
        }
    
        public int TaskId { get; set; }
        public string Title { get; set; }
        public System.DateTime TaskTerm { get; set; }
        public string Tags { get; set; }
        public string StatusString { get; set; }
        public string Description { get; set; }
        public Nullable<int> UsId { get; set; }
        public Nullable<int> StatusId { get; set; }
    
        public virtual Statuses Statuses { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<CrossTasksTags> CrossTasksTags { get; set; }
    }
}
