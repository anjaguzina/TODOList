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

namespace TODOList.View
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Window
    {
        private readonly TaskRepository repository;
        private List<TODOList.Model.Task> tasks;
        public MyProfile()
        {
            InitializeComponent();
            Uri uri = new Uri("https://cdn-icons-png.flaticon.com/512/847/847969.png");
            BitmapImage bitmap = new BitmapImage(uri);
            ProfileImage.Source = bitmap;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e) {
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
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
            EditTask editTask = new EditTask();
            editTask.ShowDialog();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            // 1. Provera da li je nešto selektovano u ListBox-u
            TODOList.Model.Task selectedTask = TasksListBox.SelectedItem as TODOList.Model.Task;
            if (selectedTask == null)
            {
                MessageBox.Show("Please select a task to delete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Dijalog za potvrdu brisanja
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //TaskRepository repository = new TaskRepository();
                    repository.Delete(selectedTask); // Briše iz CSV-a
                    LoadTasks(); // Osvežava prikaz u ListBox-u
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting task: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadTasks()
        {
            //TaskRepository repository = new TaskRepository();
            //List<Task> tasks = repository.GetAll(); // Pretpostavka: učitava sve taskove iz CSV-a

            // Ako taskovi treba da budu filtrirani po korisniku:
            // tasks = tasks.Where(t => t.UserId == currentUser.Id).ToList();

            TasksListBox.ItemsSource = null;
            TasksListBox.ItemsSource = tasks;
        }

    }
}
