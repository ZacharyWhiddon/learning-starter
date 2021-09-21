﻿using LearningStarter.Entities;
using LearningStarterServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningStarterServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired();

            modelBuilder.Entity<Class>()
                .Property(x => x.Capacity)
                .IsRequired();

            modelBuilder.Entity<Class>()
                .Property(x => x.Subject)
                .IsRequired();

            modelBuilder.Entity<Class>()
                .Property(x => x.UserId)
                .IsRequired();

        }
    }
}
