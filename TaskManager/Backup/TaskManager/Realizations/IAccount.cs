using TaskManager.Models;

namespace TaskManager.Realizations
{
    public interface IAccount
    {
        /// <summary>
        /// Adding of User to DataBase
        /// </summary>
        /// <param name="Person">Object User</param>
        /// <returns>Massage of success for m_ResultMassage</returns>
        string InsertUser(Users Person);

        /// <summary>
        /// Confirming of user via own Email
        /// </summary>
        /// <param name="code">Crypt password from user's email</param>
        /// <param name="l">Login of User</param>
        /// <returns>Massage of success for m_ResultMassage</returns>
        string SetConfirmation(string code, string l);

        /// <summary>
        /// Conditions are for failed attempt to authorize
        /// </summary>
        /// <param name="model">Model of project for authorize on the site</param>
        /// <returns>Massage of Conditions for m_ResultMassage</returns>
        string[] AuthUser(LogIn model);
    } 
}
