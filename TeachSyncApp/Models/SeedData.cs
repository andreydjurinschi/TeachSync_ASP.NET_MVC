using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Models;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        // Получаем контекст из DI контейнера
        using (var context =
               new ApplicationDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Если пользователи уже существуют, выходим
            if (context.Users.Any())
            {
                return;
            }

            // Если роли не существуют, добавляем их
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },  // Admin
                    new Role { Name = "Manager" },  // Manager
                    new Role { Name = "Teacher" }   // Teacher
                );
            }


                // Сохраняем изменения, чтобы роли были добавлены в базу
                context.SaveChanges();

            // Добавляем пользователя с ролью
            context.Users.AddRange(
                new User
                {
                    Name = "Andrei",
                    Surname = "Djurinschi",
                    CreatedAt = DateTime.Parse("2021-09-01"),
                    Email = "andrei@gmail.com",
                    RoleId = 1, // Присваиваем роль Admin
                }
            );

            // Сохраняем изменения в базе данных
            context.SaveChanges();
        }
    }
}
