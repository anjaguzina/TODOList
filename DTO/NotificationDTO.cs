using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TODOList.Model;

namespace TODOList.DTO
{
    public class NotificationDTO:INotifyPropertyChanged
    {
        public NotificationDTO()
        {

        }

        public NotificationDTO( string message, bool isactive, int userId)
        {
            this.message = message;
            this.isActive = isactive;
            this.userId = userId;
        }

        public NotificationDTO(Notification n)
        {
            message = n.Message;
            userId = n.UserId;
            isActive = n.IsActive;

        }



        public NotificationDTO(NotificationDTO n)
        {
            message = n.message;
            userId = n.userId;
            isActive = n.isActive;
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

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                if (value != message)
                {
                    message = value;
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



        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    OnPropertyChanged();
                }
            }
        }



        public Notification ToNotification()
        {
            return new Notification(id, message, isActive, userId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
