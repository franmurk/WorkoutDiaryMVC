namespace WorkoutDiaryMVC.Models
{
    public class ProgressEntry
    {
        public int Id { get; set; }
        public string Exercise { get; set; }
        public float Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
