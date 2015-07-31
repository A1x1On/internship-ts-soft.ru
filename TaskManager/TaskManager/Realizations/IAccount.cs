using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Models;

namespace TaskManager.Realizations
{
    public interface IAccount
    {
        string UserToDb(USERS u);
        string UserConfirm(string Code);
        string[] UserAuthorisation(LogIn model);
    } 
}
