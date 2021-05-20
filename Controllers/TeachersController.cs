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
    public class TeachersController : Controller
    {
        private readonly ProjectContext _context;

        public TeachersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string TeacherAcademicRank, string TeacherDegree, string SearchString)
        {

             
            IEnumerable<Teacher> teachers = _context.Teacher;
            IQueryable<string> RankQuery = _context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct();
            IQueryable<string> DegreeQuery = _context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct();

            if (!string.IsNullOrEmpty(TeacherAcademicRank))
            {
                teachers = teachers.Where(x => x.AcademicRank == TeacherAcademicRank);
            }
            if (!string.IsNullOrEmpty(TeacherDegree))
            {
                teachers = teachers.Where(x => x.Degree == TeacherDegree);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                teachers = teachers.Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(SearchString.ToLower())).ToList();
                // teachers = teachers.Where(s => s.FullName.ToLower().Contains(SearchString.ToLower())); 
            }


            //IQueryable teacher = teachers.AsQueryable();
            // teachers = teachers.Include(m => m.FirstCourses).Include(c => c.SecondCourses).ThenInclude(m => m.Course);

            var TeacherFullNameDegreeAcademicRank = new TeacherFullNameDegreeAcademicRankViewModel
            {
                AcademicRanges = new SelectList(await RankQuery.ToListAsync()),
                Degrees = new SelectList(await DegreeQuery.ToListAsync()),
                Teachers = teachers.ToList()
            };

            return View(TeacherFullNameDegreeAcademicRank);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
        // GET: Teachers/Courses/
        public async Task<IActionResult> Courses(int id)
        {
           
            var courses = _context.Course.Where(c => c.FirstTeacherID == id || c.SecondTeacherID == id);
            courses = courses.Include(t => t.FirstTeacher).Include(t => t.SecondTeacher);

            ViewData["TeacherId"] = id;
            ViewData["TeacherAcademicRank"] = _context.Teacher.Where(t => t.Id == id).Select(t => t.AcademicRank).FirstOrDefault();
            ViewData["TeacherFullName"] = _context.Teacher.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            return View(courses);
        }

    }
}
