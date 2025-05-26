using Microsoft.AspNetCore.Mvc;
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Controllers
{
    public class PersonalBestController : Controller
    {
        private static List<PersonalBest> _records = new();

        public IActionResult Records()
        {
            return View("Records", _records);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonalBest pb)
        {
            if (ModelState.IsValid)
            {
                pb.Id = _records.Any() ? _records.Max(p => p.Id) + 1 : 1;
                _records.Add(pb);
                return RedirectToAction("Records");
            }

            return View(pb);
        }

        public IActionResult Edit(int id)
        {
            var record = _records.FirstOrDefault(p => p.Id == id);
            if (record == null) return NotFound();

            return View(record);
        }

        [HttpPost]
        public IActionResult Edit(PersonalBest updated)
        {
            var existing = _records.FirstOrDefault(p => p.Id == updated.Id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.Exercise = updated.Exercise;
                existing.MaxWeight = updated.MaxWeight;
                existing.Reps = updated.Reps;
                existing.Date = updated.Date;

                return RedirectToAction("Records");
            }

            return View(updated);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var record = _records.FirstOrDefault(p => p.Id == id);
            if (record != null)
            {
                _records.Remove(record);
            }

            return RedirectToAction("Records");
        }

    }
}

