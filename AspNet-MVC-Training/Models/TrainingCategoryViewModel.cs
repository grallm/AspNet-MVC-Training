using AspNet_MVC_Training.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AspNet_MVC_Training.Models
{
    public class TrainingCategoryViewModel
    {
        public List<Training> Trainings { get; set; }
        public SelectList Categories { get; set; }
        public IList<int> UserFormations { get; set; }
        public IList<UserTraining> UserCart { get; set; }
        public string TrainingCategory { get; set; }
        public string SearchString { get; set; }
        public string UserId { get; set; }
    }
}