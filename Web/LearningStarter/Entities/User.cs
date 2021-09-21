using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LearningStarter.Entities;

namespace LearningStarterServer.Entities
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
