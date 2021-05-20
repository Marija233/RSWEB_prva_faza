using Microsoft.EntityFrameworkCore;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.Data;


namespace Project.Data
{
    public class ProjectContext : DbContext
    {
       
            public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

      
        
        public DbSet<Project.Models.Course> Course { get; set; }

            public DbSet<Project.Models.Student> Student { get; set; }

            public DbSet<Project.Models.Teacher> Teacher { get; set; }

            public DbSet<Project.Models.Enrollment> Enrollment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Enrollment>().HasOne<Student>(p => p.Student)
                .WithMany(p => p.Courses)
                .HasForeignKey(p => p.StudentId);

            builder.Entity<Enrollment>().HasOne<Course>(p => p.Course)
                .WithMany(p => p.Students)
                .HasForeignKey(p => p.CourseId);



            builder.Entity<Course>().HasOne(m => m.FirstTeacher)
                                 .WithMany(m => m.FirstCourses)
                                 .HasForeignKey(m => m.FirstTeacherID);

            builder.Entity<Course>().HasOne(m => m.SecondTeacher)
                                          .WithMany(m => m.SecondCourses)
                                          .HasForeignKey(m => m.SecondTeacherID);
        }


    }
        }

