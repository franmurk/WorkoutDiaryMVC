
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Data
{
    public class WorkoutRepository
    {
        private readonly List<Workout> _workouts = new();
        private int _idCounter = 1;

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
                existing.WorkoutType = workout.WorkoutType;
                existing.DurationInMinutes = workout.DurationInMinutes;


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
