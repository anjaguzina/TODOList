using TODOList.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TODOList.Model;
using TODOList.Controller;
using TODOList.DTO;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LogInForm : Window
    {
        private MainController controller;
        private UserDTO userDTO;


        public LogInForm()
        {
            InitializeComponent();
           
           // _repository = new UserRepository();
            userDTO = new UserDTO();
            DataContext = userDTO;
            controller = new MainController();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e) {
            AddUser addWindow = new AddUser();
            addWindow.ShowDialog();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Username = userDTO.Username;
            User user = controller.GetByUsername(Username);

            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    switch (user.Role)
                    {
                        case UserRole.Admin:
                            ViewAllUsers viewAllUsersWindow = new ViewAllUsers();
                            viewAllUsersWindow.ShowDialog();
                            break;

                        case UserRole.Standard:
                            MyProfile myWindow = new MyProfile(user.Id);
                            myWindow.ShowDialog();
                            break;

                        default:
                            MessageBox.Show("User role not supported.");
                            break;
                    }

                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }

    }
}
