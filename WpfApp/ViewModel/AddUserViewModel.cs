using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using WpfApp.DataAccessHandler;
using WpfApp.Model;
using WpfApp.ViewModel.Commands;

namespace WpfApp.ViewModel
{
    class AddUserViewModel : BaseViewModel
    {
        private string _name { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                var culInfo = new CultureInfo("en-US", false).TextInfo;
                _name = culInfo.ToTitleCase(value);
                OnPropertyChanged();
            }
        }

        private string _age { get; set; }
        public string Age
        {
            get => _age; set
            {
                if (value.AreDigitsOnly())
                    _age = value;
                OnPropertyChanged();
            }
        }

        private string _city { get; set; }
        public string City
        {
            get => _city;
            set
            {
                var culInfo = new CultureInfo("en-US", false).TextInfo;
                _city = culInfo.ToTitleCase(value); OnPropertyChanged();
            }
        }

        private string _occupation { get; set; }
        public string Occupation { get => _occupation; set { _occupation = value; OnPropertyChanged(); } }

        private bool _isMarriedTrue { get; set; }
        public bool IsMarriedTrue { get => _isMarriedTrue; set { _isMarriedTrue = value; OnPropertyChanged(); } }

        private bool _isMarriedFalse { get; set; }
        public bool IsMarriedFalse { get => _isMarriedFalse; set { _isMarriedFalse = value; OnPropertyChanged(); } }

        private bool _hasDiplomaTrue { get; set; }
        public bool HasDiplomaTrue { get => _hasDiplomaTrue; set { _hasDiplomaTrue = value; OnPropertyChanged(); } }

        private bool _hasDiplomaFalse { get; set; }
        public bool HasDiplomaFalse { get => _hasDiplomaFalse; set { _hasDiplomaFalse = value; OnPropertyChanged(); } }

        private string _subjects { get; set; }
        public string Subjects { get => _subjects; set { _subjects = value; OnPropertyChanged(); } }

        public RelayCommand AddUserCommand { get; set; }

        public AddUserViewModel()
        {
            HasDiplomaTrue = true;
            IsMarriedTrue = true;

            AddUserCommand = new RelayCommand(obj => { adduser(); });
        }

        private void adduser()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Age) ||
                string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Occupation) || string.IsNullOrEmpty(Subjects))
            {
                MessageBox.Show("some fields are empty");
                return;
            }

            UserModel userModel = new UserModel();
            userModel.Name = Name;
            userModel.Age = Int32.Parse(Age);
            userModel.City = City;
            userModel.Occupation = Occupation;
            userModel.IsMarried = (IsMarriedTrue == true) ? true : false;
            userModel.HasDiploma = (HasDiplomaTrue == true) ? true : false;
            userModel.CurrentSubjects = new List<string>();

            if (!Subjects.Contains(','))
            {
                MessageBox.Show("Invalid format");
                return;
            }

            var subjects = Subjects.Split(',');

            foreach (var subject in subjects)
                userModel.CurrentSubjects.Add(subject);

            TextFileAccess textFileAccess = new TextFileAccess();
            textFileAccess.UpdateFile(userModel);
            MessageBox.Show("new student added");

            Name = string.Empty;
            Age = string.Empty;
            City = string.Empty;
            Occupation = string.Empty;
            Subjects = string.Empty;
        }//

    }

    public static class StringExtensions
    {
        public static bool AreDigitsOnly(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            int num = 0;
            var val = Int32.TryParse(text, out num);

            if (val == false)
                return false;

            if (num >= 0 && num <= 99)
                return true;
            else
                return false;
        }
    }
}
