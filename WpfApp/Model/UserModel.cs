using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp.Model
{
    public class UserModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }
        public bool IsMarried { get; set; }
        public bool HasDiploma { get; set; }
        public List<string> CurrentSubjects { get; set; }
    }
}
