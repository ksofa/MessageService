using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerGrade_7.Models
{
    /// <summary>
    /// Класс для генерации.
    /// </summary>
    public class RandomGen
    {

        public static Random rnd = new Random();


        /// <summary>
        /// Генерируем сообщение.
        /// </summary>
        /// <returns></returns>
        public string RandomMessage()
        {
            StringBuilder mail = new();

            int length = rnd.Next(2, 20);

            //генерируем.
            for (int i = 0; i < length; i++)
            {
                mail.Append((char)rnd.Next('a', 'z'));
            }

            return mail.ToString();
        }

       

        /// <summary>
        /// Генерируем email.
        /// </summary>
        /// <returns></returns>
        public string RandomEmail()
        {
            StringBuilder email = new();
            int length = rnd.Next(4, 11);
            //генерируем.
            for (int i = 0; i < length; i++)
            {
                email.Append((char)rnd.Next('a', 'z'));
            }
            email.Append("@mail.com");
            return email.ToString();
        }

        /// <summary>
        /// Генерируем имя.
        /// </summary>
        /// <returns></returns>
        public string RandomUserName()
        {
            StringBuilder username = new();
            int length = rnd.Next(4, 7);
            //генерируем.
            for (int i = 0; i < length; i++)
            {
                username.Append((char)rnd.Next('a', 'z'));
            }
            return username.ToString();
        }

        /// <summary>
        /// Generate random subject.
        /// </summary>
        /// <returns></returns>
        public string RandomSubject()
        {
            StringBuilder subject = new();

            int length = rnd.Next(2, 25);
            //генерируем.
            for (int i = 0; i < length; i++)
            {
                subject.Append((char)rnd.Next('A', 'Z'));
            }

            return subject.ToString();
        }
    }
}
