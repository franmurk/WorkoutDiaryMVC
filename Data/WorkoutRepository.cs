
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Data
{
    public class WorkoutRepository
    {
        private readonly List<Workout> _workouts = new();
        private readonly List<ProgressEntry> _progressEntries = new();
        private readonly List<PersonalBest> _personalBests = new();

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
        public void DeletePersonalBest(int id)
        {
            var pb = _personalBests.FirstOrDefault(p => p.Id == id);
            if (pb != null)
            {
                _personalBests.Remove(pb);

                _progressEntries.RemoveAll(pe => pe.Exercise.Equals(pb.Exercise, StringComparison.OrdinalIgnoreCase));
            }
        }


        public List<ProgressEntry> GetProgressEntries() => _progressEntries;

        public List<PersonalBest> GetPersonalBests() => _personalBests;

        public void AddProgressEntry(ProgressEntry entry)
        {
            entry.Id = _progressEntries.Count + 1;
            _progressEntries.Add(entry);

            var existingBest = _personalBests.FirstOrDefault(pb => pb.Exercise == entry.Exercise);

            if (existingBest == null)
            {
                _personalBests.Add(new PersonalBest
                {
                    Id = _personalBests.Count + 1,
                    Exercise = entry.Exercise,
                    MaxWeight = entry.Weight,
                    Date = entry.Date
                });
            }
            else if (entry.Weight > existingBest.MaxWeight)
            {
                existingBest.MaxWeight = entry.Weight;
                existingBest.Date = entry.Date;
            }
        }
    }
}
