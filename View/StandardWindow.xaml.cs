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

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for StandardWindow.xaml
    /// </summary>
    public partial class StandardWindow : Window
    {
        private User loggedInUser;
        public StandardWindow(User user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        private void OpenAddWindow_Click(object sender, RoutedEventArgs e)
        {
            AddTask addWindow = new AddTask();
            addWindow.ShowDialog();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            ShowTasks searchWindow = new ShowTasks();
            searchWindow.ShowDialog();
        }
    }
}
