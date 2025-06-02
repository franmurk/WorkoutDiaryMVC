using Microsoft.EntityFrameworkCore;
using WorkoutDiaryMVC.Models;

namespace WorkoutDiaryMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<PersonalBest> PersonalBests { get; set; }
        public DbSet<ProgressEntry> ProgressEntries { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
