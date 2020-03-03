using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;


namespace medicallicanse.Models
{
    public class SendMessageModel
    {
        // отправитель - устанавливаем адрес и отображаемое в письме имя
        public string from { get; set; }

        // кому отправляем
        public string to { get; set; }

        // тема письма
        public string Subject { get; set; }

        // текст письма
        public string Body { get; set; }

        // логин и пароль
        public string login { get; set; }
        public string password { get; set; }
    }
}