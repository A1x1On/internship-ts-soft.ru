//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace TaskManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class TASKS
    {
        public int TASKID { get; set; }
        [Required(ErrorMessage = "���������� ��������� ���� '��������'", AllowEmptyStrings = false)]
        public string TITLE { get; set; }
        public string TASKSTATUS { get; set; }
        [Required(ErrorMessage = "���������� ������� 'deadline'", AllowEmptyStrings = false)]
        public string TASKTERM { get; set; }
        [Required(ErrorMessage = "���������� ������� ���� �� ���� '���'", AllowEmptyStrings = false)]
        public string TAGS { get; set; }
        [Required(ErrorMessage = "���������� ��������� ���� '��������'", AllowEmptyStrings = false)]
        public string DISCRIPTION { get; set; }
        public Nullable<int> USID { get; set; }
    
        public virtual USERS USERS { get; set; }
    }
}
