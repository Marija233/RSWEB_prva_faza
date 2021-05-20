using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Student
    {
        [Required]
        public int Id { get; set; }
       
        [Required]

        [StringLength(10)]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }

        [Range(1, 240)]
        [Display(Name = "Acquired Credits")]
        public int? AcquiredCredits { get; set; }

        [Range(1, 8)]
        [Display(Name = "Current Semestar")]
        public int? CurrentSemestar { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "Education Level")]
        [StringLength(25)]
        public string EducationLevel { get; set; }

        public ICollection<Enrollment> Courses { get; set; }
        [Display(Name ="Full Name ")]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
