using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ProjectContext _context;

        public CoursesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string CourseSemester, string CourseProgramme, string SearchString)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(m => m.Semester).Select(m => m.Semester).Distinct();
            IQueryable<string> programmeQuery = _context.Course.OrderBy(m => m.Programme).Select(m => m.Programme).Distinct();

            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(s => s.Title.ToLower().Contains(SearchString.ToLower()));
            }
            int CourseSemesterID = Convert.ToInt32(CourseSemester);
            if (CourseSemesterID != 0)
            {
                courses = courses.Where(x => x.Semester == CourseSemesterID);
            }
            if (!string.IsNullOrEmpty(CourseProgramme))
            {
                courses = courses.Where(x => x.Programme == CourseProgramme);
            }

            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher)
                  .Include(c => c.Students).ThenInclude(c => c.Student);


            var TitleSemesterProgramme = new CourseTitleSemesterProgrammeViewModel
            {
                Semesters = new SelectList(await semesterQuery.ToListAsync()),
                Programmes = new SelectList(await programmeQuery.ToListAsync()),
                Courses = await courses.ToListAsync()
            };

            return View(TitleSemesterProgramme);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student).FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName");
            ViewData["SecondTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherID,SecondTeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherID,SecondTeacherID")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher).FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
        public async Task<IActionResult> CourseStudents(int? id)
        {



            IQueryable<Student> students = _context.Student.AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();

            enrollments = enrollments.Where(s => s.CourseId == id); 

            IEnumerable<int> enrollmentsStudentsId = enrollments.Select(e => e.StudentId).Distinct();  

            students = students.Where(s => enrollmentsStudentsId.Contains(s.Id)); 

            students = students.Include(c => c.Courses).ThenInclude(c => c.Course);


            ViewData["CourseTitle"] = _context.Course.Where(t => t.Id == id).Select(t => t.Title).FirstOrDefault();
            ViewData["CourseId"] = id;

            return View(students);
        }
    }
}
