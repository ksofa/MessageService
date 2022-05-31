using Microsoft.AspNetCore.Mvc;
using PeerGrade_7.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace PeerGrade_7.Controllers
{
    /// <summary>
    /// Контроллер.
    /// </summary>
    [Route("[controller]")]
    public class MailController : Controller
    {
        //вместо бд. здесь списки юзеров и сообщений.
        /// <summary>
        /// Свойство пользователей.
        /// </summary>
        public static List<Users> users = new List<Users>();
        /// <summary>
        /// Св-во сообщений
        /// </summary>
        public static List<Messages> messages = new List<Messages>();
        //генератор.
        /// <summary>
        /// для генерации.
        /// </summary>
        public static System.Random rnd = new System.Random();

        /// <summary>
        /// Json.
        /// </summary>
        public MailController()
        {
            try
            {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Users.json"))
                {
                    string path_User = AppDomain.CurrentDomain.BaseDirectory + "Users.json";
                    DataContractJsonSerializer serUser = new DataContractJsonSerializer(typeof(List<Users>));

                    using (FileStream stream = new FileStream(path_User, FileMode.OpenOrCreate))
                    {
                        users = (List<Users>)serUser.ReadObject(stream);
                    }

                }
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Message.json"))
                {
                    string path_Mail = AppDomain.CurrentDomain.BaseDirectory + "Message.json";
                    DataContractJsonSerializer pathMail = new DataContractJsonSerializer(typeof(List<Messages>));

                    using (FileStream stream = new FileStream(path_Mail, FileMode.OpenOrCreate))
                    {
                        messages = (List<Messages>)pathMail.ReadObject(stream);
                    }
                }
            }
            catch
            {
                POST();
            }
        }
        /// <summary>
        /// Проверка на уникальность.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool ExistEmail(string email)
        {
            bool flag = true;
            foreach (var person in users)
            {
                if (person.Email == email)
                {
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// Запись в json.
        /// </summary>
        private void GetData()
        {
            //пути.
            string pathMessage = AppDomain.CurrentDomain.BaseDirectory + "Message.json";
            string pathUser = AppDomain.CurrentDomain.BaseDirectory + "Users.json";

            //сериализация.
            DataContractJsonSerializer serUser = new DataContractJsonSerializer(typeof(List<Users>));
            DataContractJsonSerializer serMessage = new DataContractJsonSerializer(typeof(List<Messages>));

            //запись в потоки.
            using (FileStream stream = new FileStream(pathUser, FileMode.OpenOrCreate))
            {
                serUser.WriteObject(stream, users);
            }
            using (FileStream stream = new FileStream(pathMessage, FileMode.OpenOrCreate))
            {
                serMessage.WriteObject(stream, messages);
            }
        }


        /// <summary>
        /// Возвращает всех пользователей и сообщения.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]

        public IEnumerable<Object> GetAll()
        {
            List<object> getlist = new();
            users.Sort();

            foreach (var user in users)
            {
                getlist.Add(user);
            }
            foreach (var mail in messages)
            {
                getlist.Add(mail);
            }

            return getlist;
        }

        /// <summary>
        /// Возвращает всех пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public IEnumerable<Object> GetAllUsers()
        {
            try
            {
                users.Sort();
                return users;
            }
            catch
            {
                return users;
            }
        }

        /// <summary>
        /// Кол-во пол-лей из позиции.
        /// </summary>
        /// <param name="limit">Кол-во пользователей</param>
        /// <param name="position">Позиция</param>
        /// <returns></returns>
        [HttpGet("GetUsers/{limit}/{position}")]
        public IActionResult GetLimit(int limit, int position)
        {
            try
            {
                var cur = new List<Users>();
                //проверка.
                if ((position < 0) || (limit <= 0))
                {
                    return BadRequest("Error(");
                }

                for (int i = position; i < users.Count; i++)
                {
                    if ((position < users.Count) && (limit != 0))
                    {
                        cur.Add(users[i]);
                        limit--;
                    }
                }
                return Ok(cur);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Пользователь по email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{Email}")]
        public IActionResult GetEmail(string email)
        {
            var user = users.SingleOrDefault(u => u.Email == email);
            //проверка.
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Обр. от отправителя и получателя.
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="received">получатель</param>
        /// <returns></returns>
        [HttpGet("GetMail/{sender}/{received}")]
        public IActionResult GetMail(string sender, string received)
        {
            try
            {
                List<Messages> mails = new();

                foreach (var message  in messages)
                {
                    if (message.SenderId == sender && message.ReceiverId == received)
                    {
                        mails.Add(message);
                    }
                }

                if (mails.Count == 0)
                {
                    return NotFound();
                }

                return Ok(mails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///Сообщ. получателя.
        /// </summary>
        /// <param name="received"></param>
        /// <returns></returns>
        [HttpGet("GetMailReciever/{received}")]
        public IActionResult GetMailByReciever(string received)
        {
            try
            {

                List<Messages> recMail = new();

                foreach (var mail in messages)
                {
                    if (mail.ReceiverId == received)
                    {
                        recMail.Add(mail);
                    }
                }

                if (recMail.Count == 0)
                {
                    return BadRequest();
                }

                return Ok(recMail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Сооб. от отправителя.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        [HttpGet("GetSenderMail/{sender}")]
        public IActionResult GetSenderMail(string sender)
        {
            try
            {
                List<Messages> mailSen = new();

                foreach (var mail in messages)
                {
                    if (mail.SenderId == sender)
                    {
                        mailSen.Add(mail);
                    }
                }

                if (mailSen.Count == 0)
                {
                    return BadRequest();
                }

                return Ok(mailSen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Инициализация списка пользователей и списка.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult POST()
        {
            try
            {

                var rndG = new RandomGen();

                List<object> cur = new();

                for (int i = 0; i < 6; i++)
                {
                    Users user = new Users(rndG.RandomEmail(), rndG.RandomUserName());
                    users.Add(user);

                    Messages mail = new Messages(rndG.RandomSubject(), rndG.RandomMessage(), users[rnd.Next(0, users.Count)].Email, users[rnd.Next(0, users.Count)].Email);

                    messages.Add(mail);
                    cur.Add(user);
                    cur.Add(mail);
                }

                //записать.
                GetData();

                return Json(cur);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([Required] Users user)
        {
            try
            {
                if (ExistEmail(user.Email))
                {
                    users.Add(user);

                    GetData();

                    return Ok(user);
                }

                return BadRequest("Error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Отправка.
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost("Send")]
        public IActionResult Send(Messages mail)
        {
            try
            {
                if (mail.SenderId != null && mail.ReceiverId != null && !ExistEmail(mail.SenderId) && !ExistEmail(mail.ReceiverId))
                {
                    messages.Add(mail);
                    GetData();
                    return Json(mail);
                }
                return BadRequest("Error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
