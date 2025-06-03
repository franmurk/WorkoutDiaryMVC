using System.ComponentModel.DataAnnotations;

namespace WorkoutDiaryMVC.Models
{
    public class Workout
    {
        public string Username { get; set; } = "";

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Notes are required.")]
        public required string Notes { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public required DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public string WorkoutType { get; set; } = "Other";  // Default je "Other"
    }
}