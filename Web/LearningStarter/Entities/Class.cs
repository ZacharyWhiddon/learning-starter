using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningStarterServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningStarter.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Capacity { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class ClassDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Capacity { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
    }

    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
