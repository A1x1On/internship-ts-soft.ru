using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Realizations
{

    
    public interface IAccount
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        string UserToDb(USERS u);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        string UserConfirm(string Code);
    }



    
    public class RealizeAccount : IAccount
    {
        /// <summary>
        /// 
        /// </summary>
        private TaskManagerEntities m_db = new TaskManagerEntities();

        /// <summary>
        /// 
        /// </summary>
        private string m_Message = "";
        private string m_Alph = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                "йцукенгшщзхъфывапролдячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЯЧСМ" +
                                "ИТЬБЮёЁ1234567890";
        private string m_Key = "xmck";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public string UserToDb(USERS u)
        {
            var CryptIn = new Crypt(m_Alph);
            string Encrypt = CryptIn.CryptDecrypt(u.PASS, m_Key, true);
            string @RealPass = u.PASS;
            u.PASS = Encrypt;
            u.PASSConfirm = Encrypt;
            m_db.USERS.Add(u);
            m_db.SaveChanges();
            SetLetter(u.EMAIL, u.FIRST_NAME, u.LAST_NAME, Encrypt, u.LOGIN_NAME, @RealPass);
            return "Пользователь успешно зарегистрирован, для подтверждения перейдите на почту";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string UserConfirm(string Code)
        {
            //var CryptIn = new Crypt(m_Alph);
            //string Decrypt = CryptIn.CryptDecrypt(Code, m_Key, false);
            var value = m_db.USERS.FirstOrDefault(c => c.PASS.Contains(Code));
            if (value != null && value.CONFIRM != "confirmed")
            {
                value.PASS = Code;
                value.PASSConfirm = Code;
                value.CAPCHA = "00000";
                value.CONFIRM = "confirmed";
                m_db.SaveChanges();
                return "Поздравляем! Ваш профиль подтвержден";    
            }
            else
            {
                return "Ваш профиль уже подтвержден";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="resCrypt"></param>
        /// <param name="LOGIN"></param>
        /// <param name="PASS"></param>
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
            message.Body = "<html><body><br><img src=\"http://drunkendial.us/files/2008/09/Ubuntu-Logo-square-170x170.png\" alt=\"Super Game!\">" + @" 
            <br>Здравствуйте уважаемый(я) " + FIRST_NAME + " " + LAST_NAME + @" !
            <br>Вы получили это письмо, потому что вы зарегистрировались на http://www.HranilisheDocumentov.РФ.
            <br>Высылаем Вам секретный код для активации вашего профиля.
            <br>                                                                                              
            <br>Код активации:       <b>" + resCrypt + @"</b>
            <br>Ваш логин: " + LOGIN + "<br>Ваш пароль: " + PASS + "<br> Пройдите по ссылке для подтверждения: <a href='http://localhost:54723//Account/ConfirMail/?code=" + resCrypt + "'>Клик</a><br><br>Мы будем рады видеть Вас на нашем сайте и желаем Вам удачного дня!</body></html>";
            Smtp.Send(message);
        }

        










    }
}