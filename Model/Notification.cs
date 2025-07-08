using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TODOList.Serializer;

namespace TODOList.Model
{
    //ako ti posle bude bilo neophodno jos nesto od polja tipa CreationDate dodaj posle ili tipa AdditionalComment
    public class Notification: TODOList.Serializer.ISerializable
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int UserId { get; set;  }

        public bool IsActive { get; set; }

        public Notification() { }

        public Notification(string message, bool isactive, int user)
        {
            Message = message;
            UserId = user;
            IsActive = isactive;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), IsActive.ToString(), Message };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            IsActive = bool.Parse(values[2]);
            Message = values[3];
            
        }
    }
}
