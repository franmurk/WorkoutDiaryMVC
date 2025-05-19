
namespace WorkoutDiaryMVC.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
