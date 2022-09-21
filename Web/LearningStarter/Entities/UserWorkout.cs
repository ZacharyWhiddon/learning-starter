namespace LearningStarter.Entities
{
    public class UserWorkout
    {
        public int Id { get; set; }
        public Workout Workout { get; set; }
        public int WorkoutId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }

    public class UserWorkoutGetDto
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public UserGetDto User { get; set; }
        public WorkoutGetDto Workout { get; set; }
    }
}
