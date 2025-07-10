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
using TODOList.Model;
using TODOList.Repository;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private readonly UserRepository _repository = new UserRepository();

        public AddUser()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Provera da li korisnik sa tim korisničkim imenom već postoji
            if (_repository.GetByUsername(UsernameTextBox.Text.Trim()) != null)
            {
                MessageBox.Show("A user with that username already exists.");
                return;
            }

            // Kreiranje korisnika
            var newUser = new User
            {
                Username = UsernameTextBox.Text.Trim(),
                Password = PasswordTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                Surname = SurnameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Role = UserRole.Standard // Pretpostavljamo da se registruje kao Standard korisnik
            };

            _repository.Save(newUser);
            MessageBox.Show("User added successfully!");

            this.Close();
        }
    }
}
