using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class EnrollmentTitleViewModel
    {
        public IList<Enrollment> Enrollments { get; set; }
     
        public string SearchString { get; set; }

    }
}
