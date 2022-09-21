namespace LearningStarter.Entities
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WorkoutType WorkoutType { get; set; }
        public int WorkoutTypeId { get; set; }
    }

    public class WorkoutGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkoutTypeName { get; set; }
    }

    public class WorkoutCreateDto
    {
        public string Name { get; set; }
        public int WorkoutTypeId { get; set; }
    }

    public class WorkoutUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkoutTypeId { get; set; }
    }
}
