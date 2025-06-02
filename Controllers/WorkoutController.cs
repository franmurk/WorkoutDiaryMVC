using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var all = _context.Workouts.ToList();

            ViewData["WorkoutCount"] = all.Count;
            ViewData["TotalDuration"] = all.Sum(w => w.DurationInMinutes);
            ViewData["AverageDuration"] = all.Any() ? (int)all.Average(w => w.DurationInMinutes) : 0;

            var typeCounts = all
                .GroupBy(w => w.WorkoutType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToList();

            ViewData["WorkoutTypes"] = typeCounts;

            var upcoming = all
                .Where(w => w.Date > today)
                .OrderBy(w => w.Date)
                .Take(3)
                .ToList();

            var recent = all
                .Where(w => w.Date <= today)
                .OrderByDescending(w => w.Date)
                .Take(3)
                .ToList();

            var model = new DashboardViewModel
            {
                UpcomingWorkouts = upcoming,
                RecentWorkouts = recent
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new Workout
            {
                Name = "",
                Notes = "",
                Date = DateTime.Today,
                DurationInMinutes = 30,
                WorkoutType = "Other"
            });
        }

        [HttpPost]
        public IActionResult Create(Workout workout)
        {
            if (ModelState.IsValid)
            {
                _context.Workouts.Add(workout);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workout);
        }

        public IActionResult Edit(int id)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id);
            return workout == null ? NotFound() : View(workout);
        }

        [HttpPost]
        public IActionResult Edit(Workout workout)
        {
            if (ModelState.IsValid)
            {
                _context.Workouts.Update(workout);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workout);
        }

        public IActionResult Delete(int id)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Calendar()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEvents()
        {
            var workouts = _context.Workouts.ToList();

            var events = workouts.Select(w => new
            {
                title = w.Name,
                start = w.Date.ToString("yyyy-MM-dd"),
                url = Url.Action("Edit", new { id = w.Id }),
                color = GetColorByType(w.WorkoutType)
            });

            return Json(events);
        }

        public IActionResult All()
        {
            var allWorkouts = _context.Workouts.ToList();
            return View(allWorkouts);
        }

        private string GetColorByType(string type) => type?.ToLower() switch
        {
            "cardio" => "#007bff",
            "strength" => "#dc3545",
            "endurance" => "#28a745",
            _ => "#6f42c1"
        };
    }
}
