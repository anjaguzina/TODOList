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
    /// Interaction logic for ViewAllUsers.xaml
    /// </summary>
    public partial class ViewAllUsers : Window
    {
        private readonly UserRepository _repository;
        private List<User> _allUsers;
        public ViewAllUsers()
        {
            InitializeComponent();
            _repository = new UserRepository();
            LoadAllUsers();
        }

        private void LoadAllUsers()
        {
            _allUsers = _repository.GetAll();
            lvResults.ItemsSource = _allUsers;
        }
    }
}
