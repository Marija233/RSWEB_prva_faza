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
    public class StudentsController : Controller
    {
        private readonly ProjectContext _context;

        public StudentsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string StudentIndexSearch, string SearchString)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<Student> indexQuery = _context.Student.AsQueryable();
            

           

            if (!string.IsNullOrEmpty(StudentIndexSearch))
            {
               
                students = students.Where(s => s.StudentId.ToLower().Contains(StudentIndexSearch.ToLower()));
            }

            IEnumerable<Student> dataList = students as IEnumerable<Student>;

            if (!string.IsNullOrEmpty(SearchString))
            {
                dataList = dataList.ToList().Where(s => (s.FullName + " " + s.LastName).ToLower().Contains(SearchString.ToLower()));
            }
            students = students.Include(s => s.Courses).ThenInclude(s => s.Course);
            var StudentFullNameStudentId = new StudentFullNameStudentIdViewModel
            {
                Indexes = students.ToList(),
                Students = dataList.ToList()
            };

            return View(StudentFullNameStudentId);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.Include(s => s.Courses).ThenInclude(s => s.Course).FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProfilePicture,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePicture,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
        public async Task<IActionResult> MyCourses(long? id)
        {
            IQueryable<Course> courses = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).AsQueryable();

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
            enrollments = enrollments.Where(s => s.StudentId == id); 
            IEnumerable<int> enrollmentsIDS = enrollments.Select(e => e.CourseId).Distinct(); 

            courses = courses.Where(s => enrollmentsIDS.Contains(s.Id)); 

            courses = courses.Include(c => c.Students).ThenInclude(c => c.Student);

            ViewData["StudentName"] = _context.Student.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["studentId"] = id;
            return View(courses);
        }

    }
}
