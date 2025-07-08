using System;
using System.Collections.Generic;
using System.Linq;

using TODOList.Serializer;
using TODOList.Model;


namespace TODOList.Repository
{
    public class TaskRepository
    {
        private const string FilePath = "../../../Resources/Data/tasks.csv";

        private readonly Serializer<Task> _serializer;

        private List<Task> _tasks;
        private readonly NotificationRepository _notificationRepository = new NotificationRepository();
        public TaskRepository()
        {
            _serializer = new Serializer<Task>();
            _tasks = _serializer.FromCSV(FilePath);
            _notificationRepository = new NotificationRepository();
            foreach (var task in _tasks)
            {
                task.Notification = _notificationRepository.GetById(task.NotificationId); //da popunii tasks.csv sa notifiactionId-jevima ako bude trebalo
            }
        }

        public List<Task> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Task Save(Task task)
        {
            task.Id = NextId();
            _tasks = _serializer.FromCSV(FilePath);
            _tasks.Add(task);
            _serializer.ToCSV(FilePath, _tasks);
            return task;
        }

        public int NextId()
        {
            _tasks = _serializer.FromCSV(FilePath);
            if (_tasks.Count < 1)
            {
                return 1;
            }
            return _tasks.Max(c => c.Id) + 1;
        }

        public void Delete(Task task)
        {
            _tasks = _serializer.FromCSV(FilePath);
            Task founded = _tasks.Find(c => c.Id == task.Id);
            _tasks.Remove(task);
            _serializer.ToCSV(FilePath, _tasks);
        }

        public Task Update(Task task)
        {
            _tasks = _serializer.FromCSV(FilePath);
            Task current = _tasks.Find(c => c.Id == task.Id);
            int index = _tasks.IndexOf(task);
            _tasks.Remove(task);
            _tasks.Insert(index, task);       
            _serializer.ToCSV(FilePath, _tasks);
            return task;
        }

        public List<Task> GetByUserId(int userId)   //resila sam da po id-ju vlasnika trazim smestaje
        {
            _tasks = _serializer.FromCSV(FilePath);


            return _tasks.FindAll(c => c.UserId == userId);
        }
    }
}
