using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BeFit.DTOs;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Exercise.Where(e => e.CreatedById == GetUserId());
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTimeBeginning,DateTimeEnding")] ExerciseDTO exerciseDTO)
        {
            Exercise exercise = new Exercise()
            {
                Id = exerciseDTO.Id,
                DateTimeBeginning = exerciseDTO.DateTimeBeginning,
                DateTimeEnding = exerciseDTO.DateTimeEnding,
                CreatedById = GetUserId()
            };

            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercise
                .Where(e => e.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTimeBeginning,DateTimeEnding")] ExerciseDTO exerciseDTO)
        {
            if (id != exerciseDTO.Id)
            {
                return NotFound();
            }

            Exercise exercise = new Exercise()
            {
                Id = exerciseDTO.Id,
                DateTimeBeginning = exerciseDTO.DateTimeBeginning,
                DateTimeEnding = exerciseDTO.DateTimeEnding,
                CreatedById = GetUserId()
            };

            if (!ExerciseExists(exercise.Id, GetUserId()))
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercise
                .Where(e => e.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ExerciseExists(id, GetUserId()))
            {
                var exercise = await _context.Exercise.FindAsync(id);
                _context.Exercise.Remove(exercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id, string userId)
        {
            return _context.Exercise.Any(e => e.Id == id && e.CreatedById == userId);
        }
    }
}
