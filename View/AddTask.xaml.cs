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
using TODOList.Repository;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        private readonly TaskRepository _repository = new TaskRepository();
        private readonly NotificationRepository _locationRepository = new NotificationRepository();
        private readonly int _ownerId;
        public AddTask()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        //ovo ti je cet izgenerisao kako da iz datepickera preuzmes i ucitas datum unet od strane korisnika
        /*DateTime? selectedDate = DueDatePicker.SelectedDate;

        if (selectedDate.HasValue){
        DateTime dueDate = selectedDate.Value;
        // dalje koristiš dueDate
        }*/

    }
}
