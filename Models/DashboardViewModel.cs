using System.Collections.Generic;

namespace WorkoutDiaryMVC.Models
{
    public class DashboardViewModel
    {
        public required List<Workout> UpcomingWorkouts { get; set; }
        public required List<Workout> RecentWorkouts { get; set; }
    }
}
