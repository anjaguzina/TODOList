using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TODOList.Controller;
using TODOList.DTO;
using TODOList.Model;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private readonly MainController controller;
        private UserDTO userDTO;

        public AddUser()
        {
            InitializeComponent();
            controller = new MainController();
            userDTO = new UserDTO();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
      
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Password) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Provera da li korisnik sa tim korisničkim imenom već postoji
            if (controller.GetByUsername(UsernameTextBox.Text.Trim()) != null)
            {
                MessageBox.Show("A user with that username already exists.");
                return;
            }

            userDTO.Username = UsernameTextBox.Text.Trim();
            userDTO.Password = PasswordTextBox.Password.Trim();
            userDTO.Name = NameTextBox.Text.Trim();
            userDTO.Surname = SurnameTextBox.Text.Trim();
            userDTO.Email = EmailTextBox.Text.Trim();
            userDTO.Role = UserRole.Standard; // Pretpostavljamo da je novi korisnik Standard

           
            controller.AddUser(userDTO.ToUser());
            MessageBox.Show("User added successfully!");

            this.Close();
        }
    }
}
