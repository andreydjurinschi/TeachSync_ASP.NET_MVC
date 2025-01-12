using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Models.SeedData;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context =
               new ApplicationDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Если пользователи и курсы уже существуют, выходим
            if (context.Users.Any() || context.Courses.Any())
            {
                return;
            }

            // Если роли не существуют, добавляем их
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },  
                    new Role { Name = "Manager" },
                    new Role { Name = "Teacher" }   
                );
                context.SaveChanges();
            }

            // Добавляем пользователей
            var admin = new User
            {
                Name = "Andrei",
                Surname = "Djurinschi",
                CreatedAt = DateTime.Parse("2021-09-01"),
                Email = "andrei@gmail.com",
                RoleId = 1 // Присваиваем роль Admin
            };

            var teacher = new User
            {
                Name = "John",
                Surname = "Doe",
                CreatedAt = DateTime.Now,
                Email = "john.doe@example.com",
                RoleId = 3 // Роль Teacher
            };

            context.Users.AddRange(admin, teacher);
            context.SaveChanges();

            // Добавляем курс с правильным TeacherId
            context.Courses.AddRange(
                new Courses
                {
                    Name = "C# programming language",
                    Description = "Learning C# as a programming language.",
                    TeacherId = teacher.Id // Преподаватель
                }
            );

            context.SaveChanges();
        }
    }


}
