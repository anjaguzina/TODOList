using TODOList.Model;
using TODOList.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TODOList.Model;
using TODOList.Repository;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LogInForm : Window
    {
        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LogInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e) {
            AddUser addWindow = new AddUser();
            addWindow.ShowDialog();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);

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
