namespace LearningStarter.Entities
{
    public class PreparationStep
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PreparationStepCreateDto
    {
        public string Name { get; set; }
    }

    public class PreparationStepUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PreparationStepGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}