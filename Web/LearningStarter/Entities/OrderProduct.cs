namespace LearningStarter.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }

    public class OrderProductCreateDto
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }

    public class OrderProductUpdateDto
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }

    public class OrderProductGetDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}