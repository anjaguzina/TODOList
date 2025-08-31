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
using TODOList.DTO;
using TODOList.Controller;
using System.Windows.Controls.Primitives;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        private MainController _controller;
        private TaskDTO _taskDTO;
        private Brush _defaultBrushBorder;
        private readonly int _ownerId;
        public AddTask(MainController controller, int ownerId)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _controller = controller;
            _defaultBrushBorder = textBoxTitle.BorderBrush.Clone();
            _ownerId = ownerId;
           
            _taskDTO = new TaskDTO();
            DataContext = _taskDTO;
            datePickerDueDate.SelectedDate = null;
            datePickerDueDate.DisplayDate = DateTime.Now;
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


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableAddButton();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            _taskDTO.UserId = _ownerId;
            if (datePickerDueDate.SelectedDate.HasValue)
            {
                _taskDTO.DueDate = datePickerDueDate.SelectedDate.Value;
            }

            _controller.AddTask(_taskDTO.ToTask());
            _controller.SaveAllToStorage();
            this.Close();
        }

        private void CheckEnableAddButton()
        {
            // Dugme se aktivira samo ako su sva polja popunjena
            bool isTitleFilled = !string.IsNullOrWhiteSpace(textBoxTitle.Text);
            bool isDescriptionFilled = !string.IsNullOrWhiteSpace(textBoxDescription.Text);
            bool isDateSelected = datePickerDueDate.SelectedDate.HasValue;

            buttonAdd.IsEnabled = isTitleFilled && isDescriptionFilled && isDateSelected;
        }

        private void datePickerDueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckEnableAddButton();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            buttonAdd.IsEnabled = InputCheck();
        }


    }
}
