using System;
using System.ComponentModel.DataAnnotations;

namespace AspNet_MVC_Training.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Former { get; set; }
    }
}