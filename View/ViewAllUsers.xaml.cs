using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TODOList.Controller;
using TODOList.DTO;
using TODOList.Model;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for ViewAllUsers.xaml
    /// </summary>
    public partial class ViewAllUsers : Window
    {
        private readonly MainController controller;
        private List<UserDTO> allUsers;

        public ViewAllUsers()
        {
            InitializeComponent();
            controller = new MainController();
            LoadAllUsers();
        }

        private void LoadAllUsers()
        {
            // Dohvati sve korisnike iz kontrolera
            allUsers = controller.GetAllUsers()
                                 .Select(u => new UserDTO(u))
                                 .ToList();

            // Poveži sa ItemsControl iz novog XAML-a
            UsersItemsControl.ItemsSource = allUsers;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // Otvori LoginForm i zatvori ovu formu
            LogInForm loginForm = new LogInForm();
            loginForm.Show();
            this.Close();
        }
    }
}
