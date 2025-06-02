using Microsoft.AspNetCore.Mvc;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;
using System.Linq;

namespace WorkoutDiaryMVC.Controllers
{
    public class PersonalBestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalBestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Records()
        {
            var records = _context.PersonalBests.ToList();
            return View("Records", records);
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
                var existing = _context.PersonalBests.FirstOrDefault(x => x.Exercise == pb.Exercise);

                if (existing == null)
                {
                    _context.PersonalBests.Add(pb);
                }
                else if (pb.MaxWeight > existing.MaxWeight)
                {
                    existing.MaxWeight = pb.MaxWeight;
                    existing.Date = pb.Date;
                    _context.PersonalBests.Update(existing);
                }

                var progress = new ProgressEntry
                {
                    Exercise = pb.Exercise,
                    Weight = (float)pb.MaxWeight,
                    Date = pb.Date
                };

                _context.ProgressEntries.Add(progress);
                _context.SaveChanges();

                return RedirectToAction("Records");
            }

            return View(pb);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var record = _context.PersonalBests.FirstOrDefault(x => x.Id == id);
            if (record != null)
            {
                _context.PersonalBests.Remove(record);
                _context.SaveChanges();
            }

            return RedirectToAction("Records");
        }

        public IActionResult ProgressChart()
        {
            var entries = _context.ProgressEntries.ToList();
            return View(entries);
        }
    }
}
