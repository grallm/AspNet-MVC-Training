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
                // Look for any trainings.
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
                        Video = "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/Ct6BUPvE2sM\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>",
                        Content = "<h3 id=\"description\">Description</h3><p>You will learn to:</p><ul><li>Be Rich</li><li>Find a life goal</li></ul><h3 id=\"part-1-be-rich\">Part 1 : Be rich</h3><p>Find money.</p><p>End</p>",
                        Price = 49.99M,
                        Image = "https://i.guim.co.uk/img/media/09de088fe70256cfae7c4bc42b6cded754545133/0_66_3500_2100/master/3500.jpg?width=445&quality=45&auto=format&fit=max&dpr=2&s=fb3f100e07c311c80942c9ed7466a322"
                    },

                    new Training
                    {
                        Title = "ABC of the Positive Psychology",
                        ReleaseDate = DateTime.Parse("06/17/2000 17:17"),
                        Category = "Personnal Developement",
                        Content = "Be Positive",
                        Price = 99.99M,
                        Image = "https://upload.wikimedia.org/wikipedia/commons/8/81/Positive_psychology_optimism.svg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}