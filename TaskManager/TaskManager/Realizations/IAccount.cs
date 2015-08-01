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
        /// <summary>
        /// Adding of User to DataBase
        /// </summary>
        /// <param name="Person">Object User</param>
        /// <returns>Massege of success for m_ResultMassage</returns>
        string UserToDb(USERS Person);

        /// <summary>
        /// Confirming of user via own Email
        /// </summary>
        /// <param name="code">Crypt password from user's email</param>
        /// <param name="l">Login of User</param>
        /// <returns>Massege of success for m_ResultMassage</returns>
        string UserConfirm(string code, string l);

        /// <summary>
        /// Conditions are for failed attempt to authorize
        /// </summary>
        /// <param name="model">Model of project for authorize on the site</param>
        /// <returns>Massege of Conditions for m_ResultMassage</returns>
        string[] UserAuthorisation(LogIn model);
    } 
}
