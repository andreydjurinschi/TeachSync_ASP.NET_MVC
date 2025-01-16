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


        }
    }
}
