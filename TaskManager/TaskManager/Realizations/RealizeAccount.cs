using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Realizations
{

    
    public interface IAccount
    {

        string UserToDb(USERS u);

    }


    
    public class RealizeAccount : IAccount
    {
        private string m_Message = "";
        public string UserToDb(USERS u)
        {
            using (TaskManagerEntities db = new TaskManagerEntities())
            {

                db.USERS.Add(u);
                db.SaveChanges();
                return "Пользователь успешно зарегистрирован, для подтверждения перейдите на почту";
            }
            return "Ошибка контекста";
        }

        
     



    }
}