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
           // Uri uri = new Uri("https://cdn-icons-png.flaticon.com/512/847/847969.png");
          //  BitmapImage bitmap = new BitmapImage(uri);
            controller = new MainController();
            taskDTO = new TaskDTO();
          //  ProfileImage.Source = bitmap;
            _userId = userId;
            Tasks = new ObservableCollection<TaskDTO>();
            TasksListBox.ItemsSource = Tasks;
            LoadTasks();
            SetHelloUser();
            var user = controller.GetUserById(_userId);
            if (!string.IsNullOrEmpty(user.ProfileImagePath) && System.IO.File.Exists(user.ProfileImagePath))
            {
                BitmapImage bitmap = new BitmapImage(new Uri(user.ProfileImagePath));
                ProfileImage.Source = bitmap;
            }
            else
            {
                // Default slika
                Uri uri = new Uri("https://cdn-icons-png.flaticon.com/512/847/847969.png");
                BitmapImage bitmap = new BitmapImage(uri);
                ProfileImage.Source = bitmap;
            }

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

                    // Kopiraj sliku u folder aplikacije (npr. "ProfileImages") da uvek bude dostupna
                    string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProfileImages");
                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    string fileName = $"{_userId}_{System.IO.Path.GetFileName(selectedFile)}";
                    string destPath = System.IO.Path.Combine(imagesFolder, fileName);

                    System.IO.File.Copy(selectedFile, destPath, true);

                    // Postavi putanju slike u model korisnika
                    var user = controller.GetUserById(_userId);
                    user.ProfileImagePath = destPath;
                    controller.UpdateUser(user);

                    // Prikaz slike u Image kontrolu
                    BitmapImage bitmap = new BitmapImage(new Uri(destPath));
                    ProfileImage.Source = bitmap;
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

            var allTasks = controller.GetAllTasks().Where(t => t.UserId == _userId).ToList();

            // Popuni Tasks ObservableCollection sa DTO
            foreach (var t in allTasks)
            {
                var dto = new TaskDTO(t);
                dto.IsToday = t.DueDate.Date == DateTime.Today; // ovo je ključno
                Tasks.Add(dto);
            }

            // Filter za paginaciju
            _filteredTasks = new ObservableCollection<TaskDTO>(allTasks.Select(t =>
            {
                var dto = new TaskDTO(t);
                dto.IsToday = t.DueDate.Date == DateTime.Today;
                return dto;
            }));

            TotalNumberOfPages = Math.Max(1, (int)Math.Ceiling((double)_filteredTasks.Count / _maxItemsPerPage));
            CurrentPageNumber = 1;

            ChangeMovePageButtonsVisibility();
            ApplyPaging(this, null);

            // UPDATE BROJAČA
            UpdateTaskCounters();
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
            var pagedTasks = new ObservableCollection<TaskDTO>(
                _filteredTasks
                    .Skip((CurrentPageNumber - 1) * _maxItemsPerPage)
                    .Take(_maxItemsPerPage)
            );

            TasksListBox.ItemsSource = pagedTasks;
        }

        private void SetHelloUser()
        {
            try
            {
                // Dohvati sve korisnike iz kontrolera
                var allUsers = controller.GetAllUsers();

                // Pronađi korisnika po _userId
                var currentUser = allUsers.FirstOrDefault(u => u.Id == _userId);

                if (currentUser != null)
                {
                    // Prikaži ime korisnika u TextBlock-u
                    string fullName = currentUser.Name; // ili $"{currentUser.Name} {currentUser.Surname}" za puno ime
                    textBlockHello.Text = $"Hello, {fullName}!";
                }
                else
                {
                    textBlockHello.Text = "Hello, User";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user info: " + ex.Message);
                textBlockHello.Text = "Hello, User";
            }
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

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Otvori LogInForm
                LogInForm loginWindow = new LogInForm();
                loginWindow.Show();

                // Zatvori MyProfile prozor
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging out: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateTaskCounters()
        {
            var allTasks = controller.GetAllTasks().Where(t => t.UserId == _userId).ToList();
            int allCount = allTasks.Count;
            int todayCount = allTasks.Count(t => t.DueDate.Date == DateTime.Today);
            int overdueCount = allTasks.Count(t => t.DueDate.Date < DateTime.Today);

            AllTasksCount.Text = allCount.ToString();
            TodayTasksCount.Text = todayCount.ToString();
            OverdueTasksCount.Text = overdueCount.ToString();
        }


        private void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskDTO task)
            {
                task.IsCompleted = true;
                controller.UpdateTask(task.ToTask());
                controller.SaveAllToStorage();
                LoadTasks(); // osveži listu
            }
        }

        private void TaskCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskDTO task)
            {
                task.IsCompleted = false;
                controller.UpdateTask(task.ToTask());
                controller.SaveAllToStorage();
                LoadTasks();
            }
        }

    }
}
