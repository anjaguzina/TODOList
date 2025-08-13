using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOList.Serializer;
using TODOList.Model;

namespace TODOList.Repository
{
    public class NotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/Notification.csv";
        private readonly Serializer<Notification> _serializer;
        private List<Notification> _notifications;

        public NotificationRepository()
        {
            _serializer = new Serializer<Notification>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public Notification GetById(int id)
        {
            return _notifications.FirstOrDefault(l => l.Id == id);
        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }

        /* public Location Save(Location location)
         {
             location.Id = NextId();
             _locations = _serializer.FromCSV(FilePath);
             _locations.Add(location);
             _serializer.ToCSV(FilePath, _locations);
             return location;
         } */
        public Notification Save(Notification notification) //probamo da popravimo LocationId
        {
            notification.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;

        }


        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(c => c.Id) + 1;
        }
    }
}
