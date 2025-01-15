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
            if (!context.Topics.Any())
            {
                context.Topics.AddRange(
                    new Topic()
                    {
                        Name = "Programming"
                    },
                    new Topic()
                        {
                           Name = "Mathematics" 
                        },
                    new Topic()
                        {
                          Name = "Design"  
                        }
                    );
                context.SaveChanges();
            }
            

        }
    }
}
