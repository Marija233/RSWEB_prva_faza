using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class StudentFullNameStudentIdViewModel
    {
        public IList<Student> Students { get; set; }
        public IList<Student> Indexes { get; set; }
        public string StudentIndexSearch { get; set; }
        public string SearchString { get; set; }
    }
}
