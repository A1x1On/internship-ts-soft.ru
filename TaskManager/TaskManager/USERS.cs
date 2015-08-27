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
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        public Users()
        {
            this.Tasks = new HashSet<Tasks>();
        }

        public int UserId { get; set; }
        [Required(ErrorMessage = "���������� ������� ���", AllowEmptyStrings = false)]
        [Display(Name = "���")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "���������� ������� �������", AllowEmptyStrings = false)]
        [Display(Name = "�������")]
        public string LastName { get; set; }
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "��������� Email �� ��������")]
        [Required(ErrorMessage = "���������� ������� Email", AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "���������� ������� �����", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "����� ������ ����� ������� 4 �������")]
        [Display(Name = "�����")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "���������� ������� ������", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "������ ������ �������� ��� ������� �� 3-� ��������")]
        [Display(Name = "������")]
        public string Pass { get; set; }
        [Required(ErrorMessage = "���������� ������� ������������� ������", AllowEmptyStrings = false)]
        [Compare("Pass", ErrorMessage = "������ �� ���������")]
        [DataType(DataType.Password)]
        [Display(Name = "�������������")]
        public string PassConfirmation { get; set; }
        public int Confirmation { get; set; }
        [Required(ErrorMessage = "���������� ������� �����", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "����� ������ ����� ������� 5 ��������")]
        [Display(Name = "�����")]
        public string Captcha { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
