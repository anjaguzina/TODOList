using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TODOList.Serializer;

namespace TODOList.Model
{
    public enum UserRole{
        Admin,
        Standard
    }
    public class User: TODOList.Serializer.ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; } 
        
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public User() { }

        public User(string username, string pass, UserRole role) {
            Username = username;
            Password = pass;
            Role = role;
        }

        public string[] ToCSV() {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values) {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Role = Enum.Parse<UserRole>(values[3]);

        }
    }
}
