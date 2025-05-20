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
                _records.Add(pb);
                return RedirectToAction("Records");
            }

            return View(pb);
        }


    }
}

