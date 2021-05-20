using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{

    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProjectContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProjectContext>>()))
            {
                // Look for any movies.
                if (context.Student.Any() || context.Teacher.Any() || context.Course.Any())
                {
                    return;   // DB has been seeded
                }

                context.Student.AddRange(

                    new Student
                    { /*Id = 1, */
                        StudentId = "233/2018",
                        FirstName = "Anna",
                        LastName = "Park",
                        EnrollmentDate = DateTime.Parse("2020-7-6"),
                        AcquiredCredits = 132,
                        CurrentSemestar = 6,
                        EducationLevel = "Associate degree"
                    },
                     new Student
                     { /*Id = 2, */
                         StudentId = "255/2018",
                         FirstName = "Mary",
                         LastName = "Johnson",
                         EnrollmentDate = DateTime.Parse("2020-3-10"),
                         AcquiredCredits = 186,
                         CurrentSemestar = 7,
                         EducationLevel = "Bachelor's degree"
                     },
                      new Student
                      { /*Id = 3, */
                          StudentId = "332/2014",
                          FirstName = "Gale",
                          LastName = "Thompson",
                          EnrollmentDate = DateTime.Parse("2021-5-12"),
                          AcquiredCredits = 212,
                          CurrentSemestar = 8,
                          EducationLevel = "Master's degree"
                      }
                );
                context.SaveChanges();

                context.Teacher.AddRange(
                   new Teacher
                   { /*Id = 1, */
                       FirstName = "Tom",
                       LastName = "Hale",
                       Degree = "Doctoral Degree",
                       AcademicRank = "Associate Professor",
                       OfficeNumber = "107",
                       HireDate = DateTime.Parse("2012-9-5"),
                   },
                   new Teacher
                   { /*Id = 2, */
                       FirstName = "Ashley",
                       LastName = "Mitchell",
                       Degree = "Professional Degree",
                       AcademicRank = "Full Professor",
                       OfficeNumber = "304",
                       HireDate = DateTime.Parse("2015-10-11")
                   },
                    new Teacher
                    { /*Id = 3, */
                        FirstName = "Taylor",
                        LastName = "Smith",
                        Degree = "Master's Degree",
                        AcademicRank = "Assistant Professor",
                        OfficeNumber = "106",
                        HireDate = DateTime.Parse("2020-4-2")
                    }


               );
                context.SaveChanges();


                context.Course.AddRange(
                    new Course
                    { /*Id = 1, */
                        Title = "Proggraming and Algorithms",
                        Credits = 6,
                        Semester = 1,
                        Programme = "Telecommunications and Information Technology",
                        EducationLevel = "Associate degree",
                        FirstTeacherID = context.Teacher.Single(d => d.FirstName == "Taylor" && d.LastName == "Smith").Id,
                        SecondTeacherID = context.Teacher.Single(d => d.FirstName == "Tom" && d.LastName == "Hale").Id

                    },
                    new Course
                    { /*Id = 2, */
                        Title = "Information Theory",
                        Credits = 6,
                        Semester = 3,
                        Programme = "Telecommunications and Information Technology",
                        EducationLevel = "Master's degree",
                        FirstTeacherID = context.Teacher.Single(d => d.FirstName == "Taylor" && d.LastName == "Smith").Id,
                        SecondTeacherID = context.Teacher.Single(d => d.FirstName == "Ashley" && d.LastName == "Mitchell").Id
                    },
                      new Course
                      { /*Id = 3, */
                          Title = "Signals and Systems",
                          Credits = 6,
                          Semester = 3,
                          Programme = "Telecommunications and Information Technology",
                          EducationLevel = "Bachelor's degree",
                          FirstTeacherID = context.Teacher.Single(d => d.FirstName == "Tom" && d.LastName == "Hale").Id,
                          SecondTeacherID = context.Teacher.Single(d => d.FirstName == "Ashley" && d.LastName == "Mitchell").Id
                      }

                );
                context.SaveChanges();


                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment
                    {
                        CourseId = 1,
                        StudentId = 2,
                        Semester = 4,
                        Year = 2,
                        Grade = 4,
                        SeminarUrl = "https://",
                        ProjectUrl = "https://",
                        ExamPoints = 70,
                        SeminarPoints = 20,
                        ProjectPoints = 8,
                        AdditionalPoints = 3,
                        FinishDate = DateTime.Parse("2019-6-11")
                    },
                                       new Enrollment
                                       {
                                           CourseId = 3,
                                           StudentId = 3,
                                           Semester = 6,
                                           Year = 3,
                                           Grade = 5,
                                           SeminarUrl = "https://",
                                           ProjectUrl = "https://",
                                           ExamPoints = 35,
                                           SeminarPoints = 25,
                                           ProjectPoints = 20,
                                           AdditionalPoints = 10,
                                           FinishDate = DateTime.Parse("2020-2-3")
                                       },
                                      new Enrollment
                                      {
                                          CourseId = 3,
                                          StudentId = 2,
                                          Semester = 7,
                                          Year = 4,
                                          Grade = 5,
                                          SeminarUrl = "https://",
                                          ProjectUrl = "https://",
                                          ExamPoints = 90,
                                          SeminarPoints = 25,
                                          ProjectPoints = 20,
                                          AdditionalPoints = 10,
                                          FinishDate = DateTime.Parse("2013-8-11")
                                      },
                           new Enrollment
                           {
                               CourseId = 2,
                               StudentId = 1,
                               Semester = 4,
                               Year = 2,
                               Grade = 5,
                               SeminarUrl = "https://",
                               ProjectUrl = "https://",
                               ExamPoints = 95,
                               SeminarPoints = 25,
                               ProjectPoints = 20,
                               AdditionalPoints = 10,
                               FinishDate = DateTime.Parse("2017-5-11")
                           },
                  new Enrollment
                  {
                      CourseId = 1,
                      StudentId = 1,
                      Semester = 8,
                      Year = 4,
                      Grade = 6,
                      SeminarUrl = "https://",
                      ProjectUrl = "https://",
                      ExamPoints = 10,
                      SeminarPoints = 30,
                      ProjectPoints = 25,
                      AdditionalPoints = 15,
                      FinishDate = DateTime.Parse("2015-6-11")
                  }
                );

                context.SaveChanges();
            }
        }
    }
}
