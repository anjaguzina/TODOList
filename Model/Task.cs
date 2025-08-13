using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using TODOList.Serializer;
using TODOList.DAO;

namespace TODOList.Model
{
    public class Task: ISerializable, IAccess<Task>
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

        public Task(int id, string title, string description, DateTime duedate, bool isdone,int userId) {
            Id = id;
            Title = title;
            Description = description;
            DueDate = duedate;
            IsCompleted = isdone;
            UserId = userId;
        }

        public void Copy(Task obj)
        {
            Id = obj.Id;
           Title = obj.Title;
            Description = obj.Description;
            DueDate = obj.DueDate;
            IsCompleted = obj.IsCompleted;
            //Notification.Copy(obj.Notification);
            UserId=obj.UserId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, Description, DueDate.ToString("dd-MM-yyyy"),IsCompleted.ToString(), UserId.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Title = values[1];
            Description = values[2];
            DueDate = DateTime.ParseExact(values[3], "dd-MM-yyyy", null);
            IsCompleted = bool.Parse(values[4]);

            UserId = Convert.ToInt32(values[5]);

        }
    }
}
