
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Data
{
    public class WorkoutRepository
    {
        private readonly List<Workout> _workouts = new();
        private int _idCounter = 1;

        public WorkoutRepository()
        {

            _workouts.AddRange(new[]
            {

        new Workout { Id = _idCounter++, Name = "Push Day", Date = DateTime.Today, Notes = "Chest and Triceps" },
        new Workout { Id = _idCounter++, Name = "Pull Day", Date = DateTime.Today.AddDays(3), Notes = "Back and Biceps" }
    });
        }

        public List<Workout> GetAll() => _workouts;

        public Workout? GetById(int id) => _workouts.FirstOrDefault(w => w.Id == id);

        public void Add(Workout workout)
        {
            workout.Id = _idCounter++;
            _workouts.Add(workout);
        }

        public void Update(Workout workout)
        {
            var existing = GetById(workout.Id);
            if (existing != null)
            {
                existing.Name = workout.Name;
                existing.Notes = workout.Notes;
                existing.Date = workout.Date;
            }
        }

        public void Delete(int id)
        {
            var workout = GetById(id);
            if (workout != null)
                _workouts.Remove(workout);
        }
    }
}
