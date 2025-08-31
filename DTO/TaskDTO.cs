using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TODOList.Model;
using TODOList.Observer;

namespace TODOList.DTO
{
    public class TaskDTO: INotifyPropertyChanged
    {

        public TaskDTO()
        {
            
        }

        public TaskDTO(int id, string title, string description, DateTime duedate, bool isCompleted, int userId )
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.dueDate = duedate;
            this.isCompleted = isCompleted;
            this.userId = userId;
        }

        public TaskDTO(Task t)
        {
            id = t.Id;
            title = t.Title;
            description = t.Description;
            dueDate = t.DueDate;
            isCompleted = t.IsCompleted;         
           userId = t.UserId;
            IsToday = t.DueDate.Date == DateTime.Today;
        }

       

        public TaskDTO(TaskDTO t)
        {
            id = t.id;
            title = t.title;
            description = t.description;
            dueDate = t.dueDate;
            isCompleted = t.isCompleted;
            userId= t.UserId;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isToday;
        public bool IsToday
        {
            get => _isToday;
            set
            {
                _isToday = value;
                OnPropertyChanged(nameof(IsToday));
            }
        }
        public bool IsOverdue => DueDate.Date < DateTime.Today;


        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

       
        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                if (value != dueDate)
                {
                    dueDate = value;
                    OnPropertyChanged();
                }
            }
        }

       

        private bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (value != isCompleted)
                {
                    isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        private int userId;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }



        public Task ToTask()
        {
            return new Task(id, title, description, dueDate, isCompleted, userId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
