using System;
using System.Collections;
using System.Runtime.Serialization;

namespace PeerGrade_7.Models
{

    public class Users : IComparable
    {
        /// <summary>
        /// Метод сравнения.
        /// </summary>
        /// <param name="per"></param>
        /// <returns></returns>
        public int CompareTo(object per)
        {
            try
            {
                if (per is not Users user) return 1;
                return Email.CompareTo((user.Email));
            }
            catch
            {
                return -1;
            }

        }
        //Пользователи обладают двумя свойствами – string UserName, string Email.

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public Users() { Email = "noname@mail.ru"; UserName = "error"; }

        /// <summary>
        ///Конструктор 2.
        /// </summary>
        /// <param name="email"> почта </param>
        /// <param name="userName"> имя </param>
        public Users(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }



    }
}
