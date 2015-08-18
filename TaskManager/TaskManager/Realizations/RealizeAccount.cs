﻿using System.Linq;
using System.Net;
using System.Net.Mail;
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
        /// Variable contains mask of password's encryption
        /// </summary>
        private string m_Alph = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZйцукенгшщзхъфывапролдячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЯЧСМИТЬБЮёЁ1234567890";

        /// <summary>
        /// Variable contains key of encryption
        /// </summary>
        private string m_Key = "xmck";

        /// <summary>
        /// Adding of User to DataBase
        /// </summary>
        /// <param name="person">Object User</param>
        /// <returns>Massage of success for m_ResultMassage</returns>
        public string InsertUser(Users person)
        {
            var personExist = m_db.Users.Where(x => x.LoginName.Equals(person.LoginName)).FirstOrDefault();
            if (personExist == null)
            {
                var cryptIn = new Crypt(m_Alph);
                string encrypt = cryptIn.CryptDecrypt(person.Pass, m_Key, true);
                string realPass = person.Pass;
                person.Pass = encrypt;
                person.Confirmation = 0;
                person.PassConfirmation = encrypt;
                m_db.Users.Add(person);
                m_db.SaveChanges();
                SendLetter(person.Email, person.FirstName, person.LastName, encrypt, person.LoginName, realPass);
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
        /// <returns>Massage of success for m_ResultMassage</returns>
        public string SetConfirmation(string code, string l)
        {
            var value = m_db.Users.Where(x => x.LoginName == l).FirstOrDefault();
            if (value != null && value.Confirmation != 1)
            {
                FormsAuthentication.SetAuthCookie(value.LoginName, true);
                value.Pass = code;
                value.PassConfirmation = code;
                value.Captcha = "00000";
                value.Confirmation = 1;
                m_db.SaveChanges();
                return "Поздравляем! Ваш профиль подтвержден";
            }
            else
            {
                if (value.Pass == code)
                {
                    FormsAuthentication.SetAuthCookie(value.LoginName, true);
                    return "Ваш профиль уже подтвержден";
                }
                return "Ошибка подтверждения";
                
            }
        }

        /// <summary>
        /// Conditions are for failed attempt to authorize
        /// </summary>
        /// <param name="model">Model of project for authorize on the site</param>
        /// <returns>Massage of Conditions for m_ResultMassage</returns>
        public string[] AuthUser(LogIn model)
        {
            string[] dataAuth = new string[2];
            var User = m_db.Users.Where(a => a.LoginName.Equals(model.Login)).FirstOrDefault();
            if (User != null)
            {
                var cryptIn = new Crypt(m_Alph);
                string decrypt = cryptIn.CryptDecrypt(User.Pass, m_Key, false);

                if (model.Password == decrypt && User.Confirmation == 1)
                {
                    FormsAuthentication.SetAuthCookie(User.LoginName, true);
                    dataAuth[1] = "true";
                    dataAuth[0] = "Вы авторизированы!";
                }
                else
                {
                    dataAuth[1] = "false";
                    if (User.Confirmation != 1)
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
        /// Forming and Sending of Email to confirm registered Account
        /// </summary>
        /// <param name="email">Inputted user's email</param>
        /// <param name="firstName">Inputted user's first name</param>
        /// <param name="lastName">Inputted user's last name</param>
        /// <param name="resCrypt">Encrypted user's pass</param>
        /// <param name="login">Inputted user's login</param>
        /// <param name="password">Inputted user's password</param>
        public void SendLetter(string email, string firstName, string lastName, string resCrypt, string login, string password)
        {
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
            smtp.Credentials = new NetworkCredential("A1x1On@yandex.ru", "2engine2");
            smtp.EnableSsl = true;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("A1x1On@yandex.ru");
            message.To.Add(new MailAddress(email));
            message.IsBodyHtml = true;
            message.Subject = "Подтверждение паролья | Хранилище документов";
            message.Body =
                "<html><body><br><img src=\"http://drunkendial.us/files/2008/09/Ubuntu-Logo-square-170x170.png\" alt=\"Super Game!\">" +
                @" 
                <br>Здравствуйте уважаемый(я) " + firstName + " " + lastName + @" !
                <br>Вы получили это письмо, потому что вы зарегистрировались на http://www.TaskManager.РФ.
                <br>Высылаем Вам секретный код для активации вашего профиля.
                <br>                                                                                              
                <br>Код активации:       <b>" + resCrypt + @"</b>
                <br>Ваш логин: " + login + "<br>Ваш пароль: " + password +
                "<br> Пройдите по ссылке для подтверждения: <a href='http://localhost:54723//Account/ConfirMail/?l="+login+"&code=" +
                resCrypt +
                "'>Клик</a><br><br>Мы будем рады видеть Вас на нашем сайте и желаем Вам удачного дня!</body></html>";
            smtp.Send(message);
        }
    }
}