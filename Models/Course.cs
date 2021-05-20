using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Course
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(1, 240)]
        public int Credits { get; set; }

        [Required]
        [Range(1, 8)]
        public int Semester { get; set; }


        [StringLength(100)]
        public string Programme { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "Education Level")]
        [StringLength(25)]
        public string EducationLevel { get; set; }
        [Display(Name = "First Teacher")]

      
        public int? FirstTeacherID { get; set; }
        public virtual Teacher FirstTeacher { get; set; }


        [Display(Name = "Second Teacher")]
        public int? SecondTeacherID { get; set; }
        public virtual Teacher SecondTeacher { get; set; }

        public ICollection<Enrollment> Students { get; set; }
    }
}
