using System;
using LearningStarterServer.Entities;

namespace LearningStarter.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public PreparationStep PreparationStep { get; set; }
        public int PreparationStepId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public int PreparationStepId { get; set; }
    }

    public class OrderUpdateDto
    {
        public int PreparationStepId { get; set; }
    }

    public class OrderGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PreparationStepId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string PreparationStepName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}