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

        public TaskDTO(string title, string description, DateTime duedate, bool isCompleted )
        {
            this.title = title;
            this.description = description;
            this.dueDate = duedate;
            this.isCompleted = isCompleted;
        }

        public TaskDTO(Task t)
        {
            title = t.Title;
            description = t.Description;
            dueDate = t.DueDate;
            isCompleted = t.IsCompleted;         
           
        }

       

        public TaskDTO(TaskDTO t)
        {
            title = t.title;
            description = t.description;
            dueDate = t.dueDate;
            isCompleted = t.isCompleted;
            
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

       

        public Task ToTask()
        {
            return new Task(id, title, description, dueDate, isCompleted);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
