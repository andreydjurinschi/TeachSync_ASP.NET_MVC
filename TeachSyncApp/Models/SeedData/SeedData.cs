using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Models.SeedData;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context =
               new ApplicationDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role
                    {
                        Name = "Admin"
                    },
                    new Role
                    {
                        Name = "Manager"
                    },
                    new Role
                    {
                        Name = "Teacher"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                /*context.Users.Add(new User
                {
                    Name = "Andrey", Surname = "Djurinschi", Email = "andrey@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 1
                });
                context.Users.Add(new User
                {
                    Name = "Ivan", Surname = "Ivanov", Email = "ivan@gmail.com", CreatedAt = DateTime.Now, RoleId = 2
                });
                context.Users.Add(new User
                {
                    Name = "Petr", Surname = "Petrov", Email = "petr@gmail.com", CreatedAt = DateTime.Now, RoleId = 3
                });
                context.Users.Add(new User
                {
                    Name = "Anna", Surname = "Sidorova", Email = "anna@gmail.com", CreatedAt = DateTime.Now, RoleId = 1
                });
                context.Users.Add(new User
                {
                    Name = "Maria", Surname = "Kuznetsova", Email = "maria@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 2
                });
                context.Users.Add(new User
                {
                    Name = "Oleg", Surname = "Semenov", Email = "oleg@gmail.com", CreatedAt = DateTime.Now, RoleId = 3
                });
                context.Users.Add(new User
                {
                    Name = "Sergey", Surname = "Fedorov", Email = "sergey@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 1
                });
                context.Users.Add(new User
                {
                    Name = "Natalia", Surname = "Smirnova", Email = "natalia@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 2
                });
                context.Users.Add(new User
                {
                    Name = "Elena", Surname = "Vasilieva", Email = "elena@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 3
                });
                context.Users.Add(new User
                {
                    Name = "Dmitry", Surname = "Gorbachev", Email = "dmitry@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 1
                });
                context.Users.Add(new User
                {
                    Name = "Yulia", Surname = "Popova", Email = "yulia@gmail.com", CreatedAt = DateTime.Now, RoleId = 2
                });
                context.Users.Add(new User
                {
                    Name = "Viktor", Surname = "Zaharov", Email = "viktor@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 3
                });*/
                context.Users.Add(new User
                {
                    Name = "Alexey", Surname = "Volkov", Email = "alexey@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 1, Password = "password1234"
                });
               /* context.Users.Add(new User
                {
                    Name = "Irina", Surname = "Komarova", Email = "irina@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 2
                });
                context.Users.Add(new User
                {
                    Name = "Kirill", Surname = "Tikhonov", Email = "kirill@gmail.com", CreatedAt = DateTime.Now,
                    RoleId = 3
                });*/

                context.SaveChanges();
            }

           /* if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Courses
                    {
                        Name = "Introduction to Python",
                        Description = "Basic concepts and syntax of Python programming",
                        TeacherId = 4
                    },
                    new Courses
                    {
                        Name = "Data Science with R",
                        Description = "Learn data analysis and visualization using R",
                        TeacherId = 7
                    },
                    new Courses
                    {
                        Name = "Web Development Fundamentals",
                        Description = "Building websites using HTML, CSS, and JavaScript",
                        TeacherId = 10
                    },
                    new Courses
                    {
                        Name = "Mobile App Development",
                        Description = "Creating mobile applications for Android and iOS",
                        TeacherId = 13
                    },
                    new Courses
                    {
                        Name = "Machine Learning Basics",
                        Description = "Introduction to machine learning algorithms and techniques",
                        TeacherId = 16
                    }
                );
                context.SaveChanges();
            }

            if (!context.Groups.Any())
            {
                context.Groups.AddRange(
                    new Group
                    {
                        Name = "I2302",
                        Year = 2020,
                        Capacity = 20
                    },
                    new Group
                    {
                        Name = "I2303",
                        Year = 2020,
                        Capacity = 25
                    },
                    new Group
                    {
                        Name = "I2304",
                        Year = 2020,
                        Capacity = 30
                    },
                    new Group
                    {
                        Name = "I2305",
                        Year = 2021,
                        Capacity = 20
                    },
                    new Group
                    {
                        Name = "I2306",
                        Year = 2021,
                        Capacity = 25
                    },
                    new Group
                    {
                        Name = "I2307",
                        Year = 2021,
                        Capacity = 30
                    },
                    new Group
                    {
                        Name = "I2308",
                        Year = 2022,
                        Capacity = 20
                    },
                    new Group
                    {
                        Name = "I2309",
                        Year = 2022,
                        Capacity = 25
                    },
                    new Group
                    {
                        Name = "I2310",
                        Year = 2022,
                        Capacity = 30
                    },
                    new Group
                    {
                        Name = "I2311",
                        Year = 2023,
                        Capacity = 20
                    },
                    new Group
                        {
                            Name = "I2312",
                            Year = 2023,
                            Capacity = 25
                        }
                        );
                context.SaveChanges();
            }

            if (!context.Topics.Any())
            {
                context.Topics.AddRange(
                    new Topic { Name = "Programming" },
                    new Topic { Name = "Basic Programming" },
                    new Topic { Name = "OOP" },
                    new Topic { Name = "Web Development" },
                    new Topic { Name = "Backend Development" },
                    new Topic { Name = "Frontend Development" },
                    new Topic { Name = "Databases" },
                    new Topic { Name = "Algorithms" },
                    new Topic { Name = "Data Structures" },
                    new Topic { Name = "Mobile Development" },
                    new Topic { Name = "Game Development" },
                    new Topic { Name = "Software Design" },
                    new Topic { Name = "Testing" },
                    new Topic { Name = "Version Control" },
                    new Topic { Name = "CI/CD" },
                    new Topic { Name = "Cloud Computing" },
                    new Topic { Name = "Machine Learning" },
                    new Topic { Name = "Artificial Intelligence" },
                    new Topic { Name = "Cybersecurity" },
                    new Topic { Name = "DevOps" }
                );
                context.SaveChanges();
            }

            if (!context.DaysOfWeek.Any())
            {
                context.DaysOfWeek.AddRange(
                    new WeekDays { Name = "Monday" },
                    new WeekDays { Name = "Tuesday" },
                    new WeekDays { Name = "Wednesday" },
                    new WeekDays { Name = "Thursday" },
                    new WeekDays { Name = "Friday" },
                    new WeekDays { Name = "Saturday" },
                    new WeekDays { Name = "Sunday" }
                    
                    );
                context.SaveChanges();*/
            }
        }
    }
    

