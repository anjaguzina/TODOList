using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using TODOList.Model;
using TODOList.Controller;
using TODOList.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows.Threading;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Window
    {
        private List<TODOList.Model.Task> tasks;
        private MainController controller;
        private int _userId;
        private TaskDTO taskDTO;    
        private ObservableCollection<TaskDTO> _tasks = new ObservableCollection<TaskDTO>();
        private ObservableCollection<TaskDTO> _filteredTasks;
        private int _currentPageNumber = 1;
        private int _maxItemsPerPage = 5;
        private int _totalNumberOfPages = 1;
        private bool _isAscending = true;
        public ObservableCollection<TaskDTO> Tasks { get; set; }
        public MyProfile(int userId)
        {
            InitializeComponent();
            Uri uri = new Uri("https://cdn-icons-png.flaticon.com/512/847/847969.png");
            BitmapImage bitmap = new BitmapImage(uri);
            controller = new MainController();
            taskDTO = new TaskDTO();
            ProfileImage.Source = bitmap;
            _userId = userId;
            Tasks = new ObservableCollection<TaskDTO>();
            TasksListBox.ItemsSource = Tasks;
            LoadTasks();
        }
        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                labelCurrentPage.Content = $"{_currentPageNumber} / {_totalNumberOfPages}";
            }
        }

        public int TotalNumberOfPages
        {
            get { return _totalNumberOfPages; }
            set
            {
                _totalNumberOfPages = value;
                labelCurrentPage.Content = $"{_currentPageNumber} / {_totalNumberOfPages}";
            }
        }


        private void AddTask_Click(object sender, RoutedEventArgs e) {
            AddTask addTask = new AddTask(controller,_userId);
            addTask.ShowDialog();
            LoadTasks();
        }

        private void ImportImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Profile Image",
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(selectedFile);
                    bitmap.EndInit();

                    ProfileImage.Source = bitmap; // ProfileImage je ime <Image> kontrole u XAML-u
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e) {
            var selectedTask = TasksListBox.SelectedItem as TaskDTO; // ili Task model, zavisi šta koristiš
            if (selectedTask == null)
            {
                MessageBox.Show("Please select a task to edit.");
                return;
            }

            EditTask editTask = new EditTask(controller, selectedTask);
            editTask.ShowDialog();

            LoadTasks(); // osveži prikaz nakon izmene
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            // Uzmi selektovani TaskDTO iz ListBox-a
            TaskDTO selectedTaskDTO = TasksListBox.SelectedItem as TaskDTO;
            if (selectedTaskDTO == null)
            {
                MessageBox.Show("Please select a task to delete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrda brisanja
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    controller.DeleteTask(selectedTaskDTO.Id);
                    controller.SaveAllToStorage();
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting task: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }

        private void LoadTasks()
        {
            Tasks.Clear();

            var allTasks = controller.GetAllTasks(); // ili kako već dobijaš listu taskova
            var userTasks = allTasks.Where(t => t.UserId == _userId);
            foreach (var t in userTasks)
            {
                Tasks.Add(new TaskDTO(t));
            }
            _filteredTasks = new ObservableCollection<TaskDTO>(
    userTasks.Select(t => new TaskDTO(t))
);

            TotalNumberOfPages = Math.Max(1, (int)Math.Ceiling((double)_filteredTasks.Count / _maxItemsPerPage));
            CurrentPageNumber = 1;

            ChangeMovePageButtonsVisibility();
            ApplyPaging(this, null);
        }

        private void MoveToLeftPage(object sender, RoutedEventArgs e)
        {
            CurrentPageNumber = Math.Max(1, CurrentPageNumber - 1);

            ChangeMovePageButtonsVisibility();

            ApplyPaging(sender, e);
        }

        private void MoveToRightPage(object sender, RoutedEventArgs e)
        {
            CurrentPageNumber = Math.Min(TotalNumberOfPages, CurrentPageNumber + 1);

            ChangeMovePageButtonsVisibility();

            ApplyPaging(sender, e);
        }

        private void ChangeMovePageButtonsVisibility()
        {
            if (TotalNumberOfPages == 1)
            {
                buttonLeftPage.IsEnabled = false;
                buttonLeftPage.Opacity = 0.5;

                labelCurrentPage.IsEnabled = false;
                labelCurrentPage.Opacity = 0.5;

                buttonRightPage.IsEnabled = false;
                buttonRightPage.Opacity = 0.5;
            }
            else if (CurrentPageNumber == TotalNumberOfPages)
            {
                buttonLeftPage.IsEnabled = true;
                buttonLeftPage.Opacity = 1.0;

                labelCurrentPage.IsEnabled = true;
                labelCurrentPage.Opacity = 1.0;

                buttonRightPage.IsEnabled = false;
                buttonRightPage.Opacity = 0.5;
            }
            else if (CurrentPageNumber == 1 && TotalNumberOfPages > 1)
            {
                buttonLeftPage.IsEnabled = false;
                buttonLeftPage.Opacity = 0.5;

                labelCurrentPage.IsEnabled = true;
                labelCurrentPage.Opacity = 1.0;

                buttonRightPage.IsEnabled = true;
                buttonRightPage.Opacity = 1.0;
            }
            else
            {
                buttonLeftPage.IsEnabled = true;
                buttonLeftPage.Opacity = 1.0;

                labelCurrentPage.IsEnabled = true;
                labelCurrentPage.Opacity = 1.0;

                buttonRightPage.IsEnabled = true;
                buttonRightPage.Opacity = 1.0;
            }
        }

        private void ApplyPaging(object sender, RoutedEventArgs e)
        {
            if (_filteredTasks == null || !_filteredTasks.Any())
                return;

            // Prikaži samo stavke za trenutnu stranicu
            var pagedTasks = _filteredTasks
                .Skip((CurrentPageNumber - 1) * _maxItemsPerPage)
                .Take(_maxItemsPerPage)
                .ToList();

            TasksListBox.ItemsSource = pagedTasks;
        }

        private void SortByDueDate_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredTasks == null || !_filteredTasks.Any())
                return;

            if (_isAscending)
            {
                _filteredTasks = new ObservableCollection<TaskDTO>(
                    _filteredTasks.OrderBy(t => t.DueDate)
                );
            }
            else
            {
                _filteredTasks = new ObservableCollection<TaskDTO>(
                    _filteredTasks.OrderByDescending(t => t.DueDate)
                );
            }

            _isAscending = !_isAscending; // obrni smer za sledeći klik
            CurrentPageNumber = 1; // resetuj na prvu stranicu nakon sortiranja
            ChangeMovePageButtonsVisibility();
            ApplyPaging(this, null);
        }
    }
}
