using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TODOList.Model;

namespace TODOList.DTO
{
   public class UserDTO:INotifyPropertyChanged
    {

        public UserDTO() { }

        public UserDTO(int id, string username, string pass, string name, string surname,string mail, UserRole role)
        {
            this.id = id;
            this.username = username;
            this.name = name;
            this.surname = surname;
            this.email = mail;
            this.password = pass;
            this.role = role;
        }

        public UserDTO(User t)
        {
            id = t.Id;
            username = t.Username;
            name = t.Name;
            surname = t.Surname;
            email = t.Email;
            password = t.Password;
            role = t.Role;
            profileImagePath = t.ProfileImagePath;
        }



        public UserDTO(UserDTO t)
        {
            id = t.Id;
            username = t.Username;
            name = t.Name;
            surname = t.Surname;
            email = t.Email;
            password = t.Password;
            role = t.Role;
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

        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string surname;
        public string Surname
        {
            get => surname;
            set
            {
                if (value != surname)
                {
                    surname = value;
                    OnPropertyChanged();
                }
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        private UserRole role;
        public UserRole Role
        {
            get => role;
            set
            {
                if (value != role)
                {
                    role = value;
                    OnPropertyChanged();
                }
            }
        }
        private string profileImagePath;
        public string ProfileImagePath
        {
            get => profileImagePath;
            set
            {
                if (value != profileImagePath)
                {
                    profileImagePath = value;
                    OnPropertyChanged();
                }
            }
        }
        public User ToUser()
        {
            return new User(id, username, password, name, surname, email, role,profileImagePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
