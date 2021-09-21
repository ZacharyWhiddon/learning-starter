using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningStarter.Entities;
using LearningStarterServer.Entities;

namespace LearningStarter.Common
{
    public static class Helpers
    {
        //This is called an extension function.  They are complicated and you shouldn't do it yet.  
        //like really don't
        public static List<ClassDto> ToDto(this List<Class> classes)
        {
            var classDtosToReturn = new List<ClassDto>();
            foreach (var @class in classes)
            {
                classDtosToReturn.Add(new ClassDto
                {
                    Capacity = @class.Capacity,
                    Id = @class.Id,
                    Subject = @class.Subject,
                    User = @class.User.ToDto(),
                    UserId = @class.UserId,
                });
            }

            return classDtosToReturn;
        }

        public static UserDto ToDto(this User user)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
            };

            return userDto;
        }
    }
}
