using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class LogIn
    {
        [Required(ErrorMessage = "Пожалуйста введите логин", AllowEmptyStrings = false)]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Пароль должен быть состоять как минимум из 2-х символов!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите пароль", AllowEmptyStrings = false)]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Пароль должен быть состоять как минимум из 3-х символов!")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        

    }
}