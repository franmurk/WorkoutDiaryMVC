using Microsoft.AspNetCore.Mvc;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;
using System;
using System.Linq;

namespace WorkoutDiaryMVC.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly WorkoutRepository _repo;

        public WorkoutController(WorkoutRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var all = _repo.GetAll();

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

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Workout workout)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(workout);
                return RedirectToAction("Index");
            }
            return View(workout);
        }

        public IActionResult Edit(int id)
        {
            var workout = _repo.GetById(id);
            return workout == null ? NotFound() : View(workout);
        }

        [HttpPost]
        public IActionResult Edit(Workout workout)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(workout);
                return RedirectToAction("Index");
            }
            return View(workout);
        }

        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
