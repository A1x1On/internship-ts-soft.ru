using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager.Models;

namespace TaskManager.Realizations
{
    public class RealizeAccount : IAccount
    {
        /// <summary>
        /// Instance EntitieDatabase is
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();
        
        /// <summary>
        /// Varible contains mask of password's Crypting
        /// </summary>
        private string m_Alph = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZйцукенгшщзхъфывапролдячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЯЧСМИТЬБЮёЁ1234567890";

        /// <summary>
        /// Varible contains key of Crypting
        /// </summary>
        private string m_Key = "xmck";

        /// <summary>
        /// Adding of User to DataBase
        /// </summary>
        /// <param name="Person">Object User</param>
        /// <returns>Massege of success for m_ResultMassage</returns>
        public string UserToDb(USERS Person)
        {
            var PersonExist = m_db.USERS.Where(x => x.LOGIN_NAME.Equals(Person.LOGIN_NAME)).FirstOrDefault();
            if (PersonExist == null)
            {
                var CryptIn = new Crypt(m_Alph);
                string Encrypt = CryptIn.CryptDecrypt(Person.PASS, m_Key, true);
                string @RealPass = Person.PASS;
                Person.PASS = Encrypt;
                Person.PASSConfirm = Encrypt;
                m_db.USERS.Add(Person);
                m_db.SaveChanges();
                SetLetter(Person.EMAIL, Person.FIRST_NAME, Person.LAST_NAME, Encrypt, Person.LOGIN_NAME, @RealPass);
                return "Пользователь успешно зарегистрирован, для подтверждения перейдите на почту";
            }
            else
            {
                return "Такой пользователь уже существует в базе";
            }
        }

        /// <summary>
        /// Confirming of user via own Email
        /// </summary>
        /// <param name="code">Crypt password from user's email</param>
        /// <param name="l">Login of User</param>
        /// <returns>Massege of success for m_ResultMassage</returns>
        public string UserConfirm(string code, string l)
        {
            var value = m_db.USERS.Where(x => x.LOGIN_NAME == l).FirstOrDefault();
            if (value != null && value.CONFIRM != "confirmed")
            {
                FormsAuthentication.SetAuthCookie(value.LOGIN_NAME, true);
                value.PASS = code;
                value.PASSConfirm = code;
                value.CAPCHA = "00000";
                value.CONFIRM = "confirmed";
                m_db.SaveChanges();
                return "Поздравляем! Ваш профиль подтвержден";
            }
            else
            {
                if (value.PASS == code)
                {
                    FormsAuthentication.SetAuthCookie(value.LOGIN_NAME, true);
                    return "Ваш профиль уже подтвержден";
                }
                return "Ошибка подтверждения";
                
            }
        }

        /// <summary>
        /// Conditions are for failed attempt to authorize
        /// </summary>
        /// <param name="model">Model of project for authorize on the site</param>
        /// <returns>Massege of Conditions for m_ResultMassage</returns>
        public string[] UserAuthorisation(LogIn model)
        {
            string[] dataAuth = new string[2];
            var User = m_db.USERS.Where(a => a.LOGIN_NAME.Equals(model.Login)).FirstOrDefault();
            if (User != null)
            {
                var CryptIn = new Crypt(m_Alph);
                string Decrypt = CryptIn.CryptDecrypt(User.PASS, m_Key, false);

                if (model.Password == Decrypt && User.CONFIRM == "confirmed")
                {
                    FormsAuthentication.SetAuthCookie(User.LOGIN_NAME, true);
                    dataAuth[1] = "true";
                    dataAuth[0] = "Вы авторизированы!";
                    Debug.WriteLine("Зареген");
                }
                else
                {
                    dataAuth[1] = "false";
                    if (User.CONFIRM != "confirmed")
                    {
                        dataAuth[0] = "Ваш email не подтвержден!";
                    }
                    else
                    {
                        dataAuth[0] = "Введен неверный пароль";
                    }
                }
            }
            else
            {
                dataAuth[0] = "Такого пользователя не существует!";
            }
            return dataAuth;
        }

        ///The others Methods///
       
        /// <summary>
        /// Forming and Sending of Email to confirm registred Account
        /// </summary>
        /// <param name="email">Inputed user's email</param>
        /// <param name="FIRST_NAME">Inputed user's first name</param>
        /// <param name="LAST_NAME">Inputed user's last name</param>
        /// <param name="resCrypt">Crypted user's pass</param>
        /// <param name="LOGIN">Inputed user's login</param>
        /// <param name="PASS">Inputed user's password</param>
        public void SetLetter(string email, string FIRST_NAME, string LAST_NAME, string resCrypt, string LOGIN, string PASS)
        {
            SmtpClient Smtp = new SmtpClient("smtp.yandex.ru", 25);
            Smtp.Credentials = new NetworkCredential("A1x1On@yandex.ru", "2engine2");
            Smtp.EnableSsl = true;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("A1x1On@yandex.ru");
            message.To.Add(new MailAddress(email));
            message.IsBodyHtml = true;
            message.Subject = "Подтверждение паролья | Хранилище документов";
            message.Body =
                "<html><body><br><img src=\"http://drunkendial.us/files/2008/09/Ubuntu-Logo-square-170x170.png\" alt=\"Super Game!\">" +
                @" 
                <br>Здравствуйте уважаемый(я) " + FIRST_NAME + " " + LAST_NAME + @" !
                <br>Вы получили это письмо, потому что вы зарегистрировались на http://www.HranilisheDocumentov.РФ.
                <br>Высылаем Вам секретный код для активации вашего профиля.
                <br>                                                                                              
                <br>Код активации:       <b>" + resCrypt + @"</b>
                <br>Ваш логин: " + LOGIN + "<br>Ваш пароль: " + PASS +
                "<br> Пройдите по ссылке для подтверждения: <a href='http://localhost:54723//Account/ConfirMail/?l="+LOGIN+"&code=" +
                resCrypt +
                "'>Клик</a><br><br>Мы будем рады видеть Вас на нашем сайте и желаем Вам удачного дня!</body></html>";
            Smtp.Send(message);
        }
    }
}