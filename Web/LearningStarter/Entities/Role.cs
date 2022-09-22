using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LearningStarter.Entities;

public class Role : IdentityRole<int>
{
    public List<UserRole> Users { get; set; } = new();
}