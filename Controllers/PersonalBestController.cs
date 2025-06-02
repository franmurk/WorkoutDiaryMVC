using Microsoft.AspNetCore.Mvc;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Controllers
{
    public class PersonalBestController : Controller
    {
        private readonly WorkoutRepository _repository;

        public PersonalBestController(WorkoutRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Records()
        {
            var records = _repository.GetPersonalBests();
            return View("Records", records);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repository.DeletePersonalBest(id);
            return RedirectToAction("Records");
        }

        [HttpPost]
        public IActionResult Create(PersonalBest pb)
        {
            if (ModelState.IsValid)
            {
                var entry = new ProgressEntry
                {
                    Exercise = pb.Exercise,
                    Weight = (float)pb.MaxWeight,
                    Date = pb.Date
                };

                _repository.AddProgressEntry(entry);

                return RedirectToAction("Records");
            }

            return View(pb);

        }
        public IActionResult ProgressChart()
        {
            var entries = _repository.GetProgressEntries();
            return View(entries);
        }

    }

}
