using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.DataAccessHandler;
using WpfApp.Model;
using WpfApp.ViewModel.Commands;
using WpfApp.Views;

namespace WpfApp.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<UserModel> _usersData { get; set; }
        public ObservableCollection<UserModel> UsersData { get => _usersData; set { _usersData = value; OnPropertyChanged(); } }

        private string _mostSubjects { get; set; }
        public string MostSubjects { get => _mostSubjects; set { _mostSubjects = value; OnPropertyChanged(); } }

        private string _overThirty { get; set; }
        public string OverThirty { get => _overThirty; set { _overThirty = value; OnPropertyChanged(); } }

        private int _noFromBudapest { get; set; }
        public int NoFromBudapest { get => _noFromBudapest; set { _noFromBudapest = value; OnPropertyChanged(); } }

        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand AddFileCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }

        private TextFileAccess textFileAccess;

        public MainWindowViewModel()
        {
            textFileAccess = new TextFileAccess();
            OverThirty = string.Empty;
            NoFromBudapest = 0;
            AddUserCommand = new RelayCommand(obj => { addUser(); });
            DeleteUserCommand = new RelayCommand(obj => { deleteUser(obj); });
            AddFileCommand = new RelayCommand(obj => { setFile(); });
        }

        private void setFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";

            if (dialog.ShowDialog() == true)
            {
                textFileAccess.SetFilePath(dialog.FileName);
                LoadData();
            }

            else
                MessageBox.Show("no file selected");
        }

        private void addUser()
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
            LoadData();
        }

        private void deleteUser(object obj)
        {
            var btn = obj as Button;
            var parentStack = ((Grid)btn.Parent).Children[0] as StackPanel;
            var name = parentStack.Children[0] as TextBlock;
            var city = parentStack.Children[2] as TextBlock;
            var occupation = parentStack.Children[3] as TextBlock;
            var userdata = UsersData.FirstOrDefault(obj => obj.Name == name.Text && obj.City == city.Text && obj.Occupation == occupation.Text);
            UsersData.Remove(userdata);
            UpdateData();
        }//

        private void LoadData()
        {
            Task.Run(async () =>
           {
               try
               {
                   var dataList = await textFileAccess.GetAllUserData();
                   Application.Current.Dispatcher.Invoke(() =>
                   {
                       UsersData = new ObservableCollection<UserModel>(dataList);
                       setProps();

                   });

                   await textFileAccess.UpdateFile(UsersData);
               }

               catch (Exception err)
               {
                   Application.Current.Dispatcher.Invoke(() =>
                   {
                       MessageBox.Show(err.Message);
                   });
               }
           });
        }//

        private void UpdateData()
        {
            Task.Run(async () =>
            {
                await textFileAccess.UpdateFile(UsersData);

                Application.Current.Dispatcher.Invoke(() => { setProps(); });

                await textFileAccess.UpdateFile(UsersData);
            });

        }//

        private void setProps()
        {
            var overthirty = UsersData.Where(obj => obj.Age > 30);
            var fromBudapest = UsersData.Where(obj => obj.City == "Budapest");

            OverThirty = string.Empty;
            NoFromBudapest = fromBudapest.Count();

            foreach (var user in overthirty)
            {
                if (!string.IsNullOrEmpty(OverThirty))
                    OverThirty += ", ";

                OverThirty += user.Name;
            }

            int max = 0;
            foreach (var user in UsersData)
            {
                if (user.CurrentSubjects.Count > max)
                {
                    max = user.CurrentSubjects.Count;
                    MostSubjects = user.Name;
                }
            }

        }//

    }
}
