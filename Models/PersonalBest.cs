namespace WorkoutDiaryMVC.Models
{
    public class PersonalBest
    {
        public int Id { get; set; } 
        public string Exercise { get; set; } = "";
        public double MaxWeight { get; set; }
        public int Reps { get; set; }
        public DateTime Date { get; set; }
    }
}
