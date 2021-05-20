using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Enrollment
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Range(1, 8)]
        public int? Semester { get; set; }
        [Range(1, 4)]
        public int? Year { get; set; }

        [Range(1, 100)]
        public int? Grade { get; set; }

        [Display(Name = "Seminar Url")]

        public string SeminarUrl { get; set; }

        [Display(Name = "Project Url")]

        public string ProjectUrl { get; set; }

        [Range(1, 100)]
        [Display(Name = "Exam Points")]
        public int? ExamPoints { get; set; }

        [Range(1, 100)]
        [Display(Name = "Seminar Points")]
        public int? SeminarPoints { get; set; }

        [Range(1, 100)]
        [Display(Name = "Project Points")]
        public int? ProjectPoints { get; set; }

        [Range(1, 10)]
        [Display(Name = "Additional Points")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Finish Date")]
        public DateTime? FinishDate { get; set; }
    }
}
