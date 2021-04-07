using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet_MVC_Training.Models
{
    public class Training
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Former { get; set; }
    }
}