using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class TeacherFullNameDegreeAcademicRankViewModel
    {
        public IList<Teacher> Teachers { get; set; }
        public SelectList AcademicRanges { get; set; }
        public string TeacherAcademicRank { get; set; }
        public SelectList Degrees { get; set; }
        public string TeacherDegree { get; set; }
        public string SearchString { get; set; }
    }
}
