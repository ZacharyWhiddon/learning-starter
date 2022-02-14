namespace LearningStarter.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductCreateDto
    {
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductUpdateDto
    {
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductGetDto
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}