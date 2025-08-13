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
using Microsoft.Win32;
using System.IO;
using TODOList.Repository;
using TODOList.Model;
using TODOList.Controller;
using TODOList.DTO;
using System.Collections.ObjectModel;

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Window
    {
        private readonly TaskRepository repository;
        private List<TODOList.Model.Task> tasks;
        private MainController controller;
        private int _userId;
        private TaskDTO taskDTO;
        public ObservableCollection<TaskDTO> Tasks { get; set; }
        public MyProfile(int userId)
        {
            InitializeComponent();
            Uri uri = new Uri("https://cdn-icons-png.flaticon.com/512/847/847969.png");
            BitmapImage bitmap = new BitmapImage(uri);
            controller = new MainController();
            repository = new TaskRepository();
            taskDTO = new TaskDTO();
            ProfileImage.Source = bitmap;
            _userId = userId;
            Tasks = new ObservableCollection<TaskDTO>();
            TasksListBox.ItemsSource = Tasks;
            LoadTasks();
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
                    // Pozovi kontroler da obriše task po ID-ju
                    controller.DeleteTask(selectedTaskDTO.Id);
                    controller.SaveAllToStorage();
                    // Osveži prikaz taskova
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting task: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
        }

    }
}
