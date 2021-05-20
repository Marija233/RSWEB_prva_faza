using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ProjectContext _context;

        public EnrollmentsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(string SearchString)
        {
          

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
           
          

            if (!string.IsNullOrEmpty(SearchString))
            {
                enrollments = enrollments.Where(s => s.Course.Title.ToLower().Contains(SearchString.ToLower()));
            }

            enrollments = enrollments.Include(s => s.Course).Include(s => s.Student);


            var Title = new EnrollmentTitleViewModel
            {
                
               
                Enrollments = await enrollments.ToListAsync()
            };

            return View(Title);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }


        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }
        public async Task<IActionResult> StudentsbyCourse(int? id, int enrollmentYear)
        {
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<Enrollment> enrollments = _context.Enrollment.Where(e => e.Id == id);
            enrollments = enrollments.Include(e => e.Course).Include(e => e.Student).OrderBy(e => e.Student.StudentId).OrderBy(e => e.Student.StudentId);

            if (enrollmentYear != 0)
            {
                enrollments = enrollments.Where(x => x.Year == enrollmentYear);
            }
            else
            {
                
                enrollments = enrollments.Where(x => x.Year == DateTime.Now.Year);
            }


            EnrollmentFilterViewModel vm = new EnrollmentFilterViewModel
            {
                Enrollments = await enrollments.ToListAsync()
            };

            ViewData["currentYear"] = _context.Course.Where(c => c.Id == id).Select(c => c.Title).FirstOrDefault();
            return View(vm);
        }
        public async Task<IActionResult> Editstudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.Include(e => e.Course).Where(e => e.CourseId == id).FirstOrDefaultAsync(); ;
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Id"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            ViewData["StudentName"] = _context.Student.Where(s => s.Id == enrollment.StudentId).Select(s => s.FullName).FirstOrDefault();
            ViewData["CourseName"] = _context.Course.Where(s => s.Id == enrollment.CourseId).Select(s => s.Title).FirstOrDefault();
            return View(enrollment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editstudent(int id, [Bind("Id,CourseId, StudentId, Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    enrollment.Course = _context.Course.FirstOrDefault(m => m.Id == enrollment.CourseId);
                    _context.Update(enrollment);
                   await  _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("StudentsbyCourse", "Enrollments");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        
       
        public IActionResult CreateEnrollment()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

     
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnrollment([Bind("Id,CourseId,StudentId,Semester,Year")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnrollmentList));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

       
       
        public async Task<IActionResult> EnrollmentList()
        {
            var universityContext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await universityContext.ToListAsync());
        }

        
        


    }


    }


