using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AspNet_MVC_Training.Areas.Identity.Data;

namespace AspNet_MVC_Training.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new IdentityDataContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<IdentityDataContext>>()))
            {
                // Look for any movies.
                if (context.Training.Any())
                {
                    return;   // DB has been seeded
                }

                context.Training.AddRange(
                    new Training
                    {
                        Title = "Become rich with Bitcoin",
                        ReleaseDate = DateTime.Parse("06/17/2000 17:17"),
                        Category = "Financial",
                        Price = 49.99M,
                        Former = "Malo G.",
                        Image = "https://i.guim.co.uk/img/media/09de088fe70256cfae7c4bc42b6cded754545133/0_66_3500_2100/master/3500.jpg?width=445&quality=45&auto=format&fit=max&dpr=2&s=fb3f100e07c311c80942c9ed7466a322"
                    },

                    new Training
                    {
                        Title = "ABC of the Positive Psychology",
                        ReleaseDate = DateTime.Parse("06/17/2000 17:17"),
                        Category = "Personnal Developement",
                        Price = 99.99M,
                        Former = "Jérémy K.",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/8/81/Positive_psychology_optimism.svg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}