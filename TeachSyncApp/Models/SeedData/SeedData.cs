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
            if (!context.ClassRooms.Any())
            {
               context.ClassRooms.AddRange(
                   new ClassRoom()
                   {
                       Name = "Classroom 1",
                       Capacity = 25,
                   }
                   );
               context.SaveChanges();
            }

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
                context.Users.AddRange(
                    new User()
                    {
                        Name = "Andrei",
                        Surname = "Djurinschi",
                        Email = "andrei@gmail.com",
                        CreatedAt = DateTime.Now,
                        RoleId = 3
                    }
                    );
                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Courses()
                    {
                        Name = "Course 1",
                        Description = "Course 1 description",
                        TeacherId = 1,
                    }
                    );
                context.SaveChanges();
            }

            if (!context.Groups.Any())
            {
                context.Groups.AddRange(
                    new Group()
                    {
                        Name = "Group 1",
                        Year = DateTime.Now.Year,
                        Capacity = 30
                    }
                    );
                context.SaveChanges();
            }

            if (!context.Topics.Any())
            {
                context.Topics.AddRange(
                new Topic() { Name = "Programming" },
                new Topic() { Name = "Mathematics" },
                new Topic() { Name = "Physics" },
                new Topic() { Name = "Chemistry" },
                new Topic() { Name = "Biology" },
                new Topic() { Name = "History" },
                new Topic() { Name = "Geography" },
                new Topic() { Name = "Literature" },
                new Topic() { Name = "Art" },
                new Topic() { Name = "Music" },
                new Topic() { Name = "Philosophy" },
                new Topic() { Name = "Sociology" },
                new Topic() { Name = "Psychology" },
                new Topic() { Name = "Economics" },
                new Topic() { Name = "Politics" },
                new Topic() { Name = "Sports" },
                new Topic() { Name = "Health" },
                new Topic() { Name = "Education" },
                new Topic() { Name = "Environment" },
                new Topic() { Name = "Technology" },
                new Topic() { Name = "Engineering" },
                new Topic() { Name = "Astronomy" },
                new Topic() { Name = "Robotics" },
                new Topic() { Name = "AI and Machine Learning" },
                new Topic() { Name = "Data Science" },
                new Topic() { Name = "Cybersecurity" },
                new Topic() { Name = "Digital Marketing" },
                new Topic() { Name = "Photography" },
                new Topic() { Name = "Video Production" },
                new Topic() { Name = "Gaming" },
                new Topic() { Name = "Web Development" },
                new Topic() { Name = "Mobile Development" },
                new Topic() { Name = "Networking" },
                new Topic() { Name = "Cloud Computing" },
                new Topic() { Name = "Databases" },
                new Topic() { Name = "Operating Systems" },
                new Topic() { Name = "Business Management" },
                new Topic() { Name = "Project Management" },
                new Topic() { Name = "Creative Writing" },
                new Topic() { Name = "Public Speaking" },
                new Topic() { Name = "Personal Finance" },
                new Topic() { Name = "Investing" },
                new Topic() { Name = "Cryptocurrency" },
                new Topic() { Name = "Travel and Tourism" },
                new Topic() { Name = "Food and Nutrition" },
                new Topic() { Name = "Fitness and Exercise" },
                new Topic() { Name = "Gardening" },
                new Topic() { Name = "Languages" },
                new Topic() { Name = "Culture and Society" }
            );
            context.SaveChanges();
            }

            if (!context.CoursesTopics.Any())
            {
                context.CoursesTopics.AddRange(
                    new CourseTopic()
                    {
                        CourseId = 1,
                        TopicId = 1
                    }
                    );
            }
            context.SaveChanges();

            if (!context.GroupCourses.Any())
            {
                context.GroupCourses.AddRange(
                    new GroupCourse()
                    {
                        GroupId = 1,
                        CourseId = 1
                    }
                    );
                context.SaveChanges();
            }

            if (!context.DaysOfWeek.Any())
            {
                context.DaysOfWeek.AddRange(
                    new WeekDays()
                    {
                        Name = "Monday",
                    },
                    new WeekDays()
                    {
                        Name = "Tuesday",
                    },
                    new WeekDays()
                    {
                        Name = "Wednesday",
                    },
                    new WeekDays()
                    {
                        Name = "Thursday",
                    },
                    new WeekDays()
                    {
                        Name = "Friday",
                    },
                    new WeekDays()
                    {
                        Name = "Saturday",
                    },
                    new WeekDays()
                    {
                        Name = "Sunday",
                    }
                    );
                context.SaveChanges();
            }

            if (!context.Schedules.Any())
            {
                context.Schedules.AddRange(
                    new Schedule()
                    {
                        ClassRoomId = 1,
                        DayOfWeekId = 1,
                        GroupCourseId = 1,
                        StartTime = TimeSpan.Parse("17:00:00"),
                        EndTime = TimeSpan.Parse("20:00:00"),
                        TeacherId = 1
                    }
                    );
                context.SaveChanges();
            }

            {
            }
        }
    }
}
