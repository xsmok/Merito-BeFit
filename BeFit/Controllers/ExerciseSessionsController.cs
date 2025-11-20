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
    public class ExerciseSessionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ExerciseSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExerciseSessions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExerciseSession.Where(e => e.CreatedById == GetUserId());
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExerciseSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExerciseTypeId,ExerciseId")] ExerciseSessionDTO exerciseSessionDTO)
        {
            ExerciseSession exerciseSession = new ExerciseSession()
            {
                Id = exerciseSessionDTO.Id,
                ExerciseTypeId = exerciseSessionDTO.ExerciseTypeId,
                ExerciseId = exerciseSessionDTO.ExerciseId,
                CreatedById = GetUserId()
            };

            if (ModelState.IsValid)
            {
                _context.Add(exerciseSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseSession);
        }

        // GET: ExerciseSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseSession = await _context.ExerciseSession
                .Where(e => e.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseSession == null)
            {
                return NotFound();
            }
            return View(exerciseSession);
        }

        // POST: ExerciseSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseTypeId,ExerciseId")] ExerciseSessionDTO exerciseSessionDTO)
        {
            if (id != exerciseSessionDTO.Id)
            {
                return NotFound();
            }

            ExerciseSession exerciseSession = new ExerciseSession()
            {
                Id = exerciseSessionDTO.Id,
                ExerciseTypeId = exerciseSessionDTO.ExerciseTypeId,
                ExerciseId = exerciseSessionDTO.ExerciseId,
                CreatedById = GetUserId()
            };

            if (!ExerciseSessionExists(exerciseSession.Id, GetUserId()))
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(exerciseSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseSession);
        }

        // GET: ExerciseSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseSession = await _context.ExerciseSession
                .Where(e => e.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseSession == null)
            {
                return NotFound();
            }

            return View(exerciseSession);
        }

        // POST: ExerciseSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ExerciseSessionExists(id, GetUserId()))
            {
                var exerciseSession = await _context.ExerciseSession.FindAsync(id);
                _context.ExerciseSession.Remove(exerciseSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseSessionExists(int id, string userId)
        {
            return _context.ExerciseSession.Any(e => e.Id == id && e.CreatedById == userId);
        }
    }
}
