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

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public User() { }

        public User(string username, string pass,string name, string surname, string mail, UserRole role) {
            Username = username;
            Password = pass;
            Name = name;
            Surname = surname;
            Email = mail;
            Role = role;
        }

        public string[] ToCSV() {
            string[] csvValues = { Id.ToString(), Username, Password,Name, Surname, Email, Role.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values) {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Name = values[3];
            Surname = values[4];
            Email = values[5];
            Role = Enum.Parse<UserRole>(values[6]);

        }
    }
}
