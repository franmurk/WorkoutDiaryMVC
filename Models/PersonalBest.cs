namespace WorkoutDiaryMVC.Models
{
    public class PersonalBest
    {
        public string Username { get; set; } = "";
        public int Id { get; set; } 
        public string Exercise { get; set; } = "";
        public double MaxWeight { get; set; }
        public DateTime Date { get; set; }
    }
}
