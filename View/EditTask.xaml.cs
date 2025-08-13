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
using TODOList.Observer;
using TODOList.Controller;
using TODOList.DTO;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        private MainController _controller;
        private TaskDTO _taskDTO;
        private Brush _defaultBrushBorder;
      
        public EditTask(MainController controller, TaskDTO taskOld)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _controller = controller;
            _defaultBrushBorder = textBoxTitle.BorderBrush.Clone();
           
            _taskDTO = new TaskDTO(taskOld);
            DataContext = _taskDTO;
           // controller.publisher.Subscribe(this);
        }

        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach (var grid in stackPanel.Children.OfType<Grid>())
            {
                foreach (var control in grid.Children)
                {
                    if (control is not TextBox)
                        continue;

                    TextBox textBox = (TextBox)control;
                    if (textBox.Text == string.Empty)
                    {
                        BorderBrushToRed(textBox);
                        validInput = false;
                    }
                    else
                    {
                        BorderBrushToDefault(textBox);
                    }
                }
            }

            return validInput;
        }

        private void BorderBrushToRed(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(1.5);
        }

        private void BorderBrushToDefault(Control control)
        {
            control.BorderBrush = _defaultBrushBorder;
            control.BorderThickness = new Thickness(1);
        }

        private bool InputCheck()
        {
            bool validInput = EmptyTextBoxCheck();

            if (!datePickerDueDate.SelectedDate.HasValue)
            {
                BorderBrushToRed(datePickerDueDate);
                validInput = false;
            }
            else
            {
                DateTime selectedDate = datePickerDueDate.SelectedDate.Value;
                if (selectedDate < DateTime.Now.Date)
                {
                    BorderBrushToRed(datePickerDueDate);
                    validInput = false;
                }
                else
                    BorderBrushToDefault(datePickerDueDate);
            }


            return validInput;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (InputCheck())
                buttonAdd.IsEnabled = true;
            else
                buttonAdd.IsEnabled = false;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void datePickerDueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonAdd.IsEnabled = InputCheck();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            buttonAdd.IsEnabled = InputCheck();
        }

        private void UpdateTask(object sender, RoutedEventArgs e)
        {
           _controller.UpdateTask(_taskDTO.ToTask());
            _controller.SaveAllToStorage();
            Close();
        }
    }
}
