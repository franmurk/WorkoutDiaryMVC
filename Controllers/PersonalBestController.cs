using Microsoft.AspNetCore.Mvc;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WorkoutDiaryMVC.Controllers
{
    [Authorize]
    public class PersonalBestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalBestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Records()
        {
            var currentUser = User.Identity?.Name;
            var records = _context.PersonalBests
                                  .Where(pb => pb.Username == currentUser)
                                  .ToList();
            return View("Records", records);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonalBest pb)
        {
            var currentUser = User.Identity?.Name;

            if (ModelState.IsValid && currentUser != null)
            {
                var existing = _context.PersonalBests
                    .FirstOrDefault(x => x.Exercise == pb.Exercise && x.Username == currentUser);

                if (existing == null)
                {
                    pb.Username = currentUser;
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
                    Date = pb.Date,
                    Username = currentUser
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
            var currentUser = User.Identity?.Name;
            var record = _context.PersonalBests
                                 .FirstOrDefault(x => x.Id == id && x.Username == currentUser);
            if (record != null)
            {
                _context.PersonalBests.Remove(record);
                _context.SaveChanges();
            }

            return RedirectToAction("Records");
        }

        public IActionResult ProgressChart()
        {
            var currentUser = User.Identity?.Name;
            var entries = _context.ProgressEntries
                                  .Where(e => e.Username == currentUser)
                                  .ToList();
            return View(entries);
        }
    }
}
