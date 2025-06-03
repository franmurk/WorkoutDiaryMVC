namespace WorkoutDiaryMVC.Models
{
    public class ProgressEntry
    {
        public string Username { get; set; } = "";
        public int Id { get; set; }
        public string Exercise { get; set; } = "";
        public float Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
