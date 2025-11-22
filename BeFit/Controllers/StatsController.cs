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
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }


        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatsController
        public async Task<IActionResult> Index()
        {
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.ExerciseSession
                .Include(es => es.ExerciseType)
                .Include(es => es.Exercise)
                .Where(es => es.CreatedById == GetUserId() && es.Exercise.DateTimeBeginning >= fourWeeksAgo)
                .GroupBy(es => es.ExerciseType.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    TotalRepetitions = g.Sum(es => es.Series * es.Repetitions),
                    AverageWeight = g.Average(es => es.Weight),
                    MaxWeight = g.Max(es => es.Weight)
                })
                .ToListAsync();

            return View(stats);
        }

    }
}
