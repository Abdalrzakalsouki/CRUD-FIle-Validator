using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.DataAccessHandler
{
    class TextFileAccess
    {
        private static string dataPath = string.Empty;

        public TextFileAccess()
        {
        }

        public void SetFilePath(string path) => dataPath = path;

        public async Task<List<UserModel>> GetAllUserData()
        {
            var usersData = new List<UserModel>();

            if (new FileInfo(dataPath).Length == 0)
                return usersData;

            using (var stream = new StreamReader(dataPath))
            {
                while (!stream.EndOfStream)
                {
                    var usermodel = new UserModel();
                    var rawData = await stream.ReadLineAsync();
                    var dataSplit = rawData.Split(';');

                    if (dataSplit.Length < 7)
                        throw new Exception("Data Format Error");

                    var culInfo = new CultureInfo("en-US", false).TextInfo;

                    usermodel.Name = culInfo.ToTitleCase(dataSplit[0]);
                    usermodel.Age = Int32.Parse(dataSplit[1]);
                    usermodel.City = culInfo.ToTitleCase(dataSplit[2]);
                    usermodel.Occupation = dataSplit[3];
                    usermodel.IsMarried = bool.Parse(dataSplit[4]);
                    usermodel.HasDiploma = bool.Parse(dataSplit[5]);
                    usermodel.CurrentSubjects = new List<string>();

                    var subjects = dataSplit[6];
                    var subjectSplit = subjects.Split(',');

                    foreach (var subject in subjectSplit)
                        usermodel.CurrentSubjects.Add(subject);

                    usersData.Add(usermodel);
                }
            }

            return usersData;
        }

        public async Task UpdateFile(ObservableCollection<UserModel> UsersData)
        {
            File.WriteAllText(dataPath, string.Empty);
            using (var stream = new StreamWriter(dataPath))
            {

                foreach (var userdata in UsersData)
                {
                    var inlineText = $"{userdata.Name};{userdata.Age};{userdata.City};{userdata.Occupation};{userdata.IsMarried.ToString()};{userdata.HasDiploma.ToString()};";

                    foreach (var subject in userdata.CurrentSubjects)
                        inlineText += $"{subject},";

                    inlineText = inlineText.Remove(inlineText.Length - 1);

                    await stream.WriteLineAsync(inlineText);
                }
            }
        }//

        public void UpdateFile(UserModel UsersData)
        {
            using (var stream = new StreamWriter(dataPath, true))
            {
                var inlineText = $"{UsersData.Name};{UsersData.Age};{UsersData.City};{UsersData.Occupation};{UsersData.IsMarried.ToString()};{UsersData.HasDiploma.ToString()};";

                foreach (var subject in UsersData.CurrentSubjects)
                    inlineText += $"{subject},";

                inlineText = inlineText.Remove(inlineText.Length - 1);

                stream.WriteLineAsync(inlineText);
            }
        }//

    }
}
