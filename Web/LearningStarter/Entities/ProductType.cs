namespace LearningStarter.Entities
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductTypeCreateDto
    {
        public string Name { get; set; }
    }

    public class ProductTypeUpdateDto
    {
        public string Name { get; set; }
    }

    public class ProductTypeGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}