using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PeerGrade_7.Models
{
    [DataContract]
    public class Messages
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="recieverId"></param>
        /// <param name="senderId"></param>
        public Messages(string subject, string message, string recieverId, string senderId)
        {
            Subject = subject;
            Message = message;
            ReceiverId = recieverId;
            SenderId = senderId;
        }

        //Сообщения обладают четырьмя свойствами:
        //Subject, Message,SenderId, ReceiverId.

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "recievedId")]
        public string ReceiverId { get; set; }

        [DataMember(Name = "senderId")]
        public string SenderId { get; set; }
    }
}
