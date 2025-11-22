using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BeFit.Data;
using BeFit.Models;
using BeFit.DTOs;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExerciseTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }


        public ExerciseTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExerciseTypes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExerciseType.Where(e => e.CreatedById == GetUserId());
            return View(await applicationDbContext.ToListAsync());

        }

        // GET: ExerciseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseType
                .Include(e => e.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseType == null)
            {
                return NotFound();
            }

            return View(exerciseType);
        }

        // GET: ExerciseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExerciseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ExerciseTypeDTO exerciseTypeDTO)
        {
            ExerciseType exerciseType = new ExerciseType()
            {
                Id = exerciseTypeDTO.Id,
                Name = exerciseTypeDTO.Name,
                CreatedById = GetUserId()
            };

            if (ModelState.IsValid)
            {
                _context.Add(exerciseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseType);

        }

        // GET: ExerciseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseType.FindAsync(id);
            if (exerciseType == null)
            {
                return NotFound();
            }
            return View(exerciseType);
        }

        // POST: ExerciseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ExerciseTypeDTO exerciseTypeDTO)
        {
            if (id != exerciseTypeDTO.Id)
            {
                return NotFound();
            }

            ExerciseType exerciseType = new ExerciseType()
            {
                Id = exerciseTypeDTO.Id,
                Name = exerciseTypeDTO.Name,
                CreatedById = GetUserId()
            };

            if (!ExerciseTypeExists(exerciseType.Id, GetUserId()))
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(exerciseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseType);

        }

        // GET: ExerciseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseType
                .Include(e => e.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseType == null)
            {
                return NotFound();
            }

            return View(exerciseType);
        }

        // POST: ExerciseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseType = await _context.ExerciseType.FindAsync(id);
            if (exerciseType != null)
            {
                _context.ExerciseType.Remove(exerciseType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseTypeExists(int id, string userId)
        {
            return _context.ExerciseType.Any(e => e.Id == id && e.CreatedById == userId);
        }

    }
}
