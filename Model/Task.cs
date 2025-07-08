using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TODOList.Serializer;
using TODOList.Repository;

namespace TODOList.Model
{
    public class Task: ISerializable
    {
        public int Id { get; set; }                     
        public string Title { get; set; }                
        public string Description { get; set; }          
        public DateTime DueDate { get; set; }           
        public bool IsCompleted { get; set; }

        public Notification Notification { get; set; }

        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public Task() { }

        public Task(string title, string description, DateTime duedate, bool isdone) {
            Title = title;
            Description = description;
            DueDate = duedate;
            IsCompleted = isdone;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, Description, DueDate.ToString("dd-MM-yyyy HH:mm:ss"),IsCompleted.ToString(), NotificationId.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            Description = values[2];
            DueDate = DateTime.ParseExact(values[3], "dd-MM-yyyy HH:mm:ss", null);
            IsCompleted = bool.Parse(values[4]);

            NotificationId = Convert.ToInt32(values[5]);

            // učitaj iz repozitorijuma
            NotificationRepository notificationRepository = new NotificationRepository();
            Notification = notificationRepository.GetById(NotificationId);
        }
    }
}
