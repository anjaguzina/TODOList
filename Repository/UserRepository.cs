using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOList.Model;
using TODOList.Serializer;

namespace TODOList.Repository
{
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User Save(User user)
        {
            user.Id = NextId();
            _users = _serializer.FromCSV(FilePath);
            _users.Add(user);
            _serializer.ToCSV(FilePath, _users);
            return user;
        }

        public int NextId()
        {
            _users = _serializer.FromCSV(FilePath);
            if (_users.Count < 1)
            {
                return 1;
            }
            return _users.Max(c => c.Id) + 1;
        }
    }
}
