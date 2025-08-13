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
            allUsers = controller.GetAllUsers()
                                  .Select(u => new UserDTO(u))
                                  .ToList();

            lvResults.ItemsSource = allUsers;
        }
    }
}
